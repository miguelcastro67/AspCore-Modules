using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using System.Reflection;
using WebModule;

namespace HostSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _ModuleManager = new ModuleManager();
            _ModuleManager.RegisterAll();
        }

        public IConfiguration Configuration { get; }
        ModuleManager _ModuleManager = null;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorOptions(o =>
            {
                if (_ModuleManager.Modules != null)
                {
                    foreach (ModuleBase module in _ModuleManager.Modules)
                    {
                        Assembly assembly = module.GetType().Assembly;
                        string resourceNamespace = assembly.GetName().Name;
                        EmbeddedFileProvider fileProvider = new EmbeddedFileProvider(assembly);

                        o.FileProviders.Add(fileProvider);
                    }
                }
            }).ConfigureApplicationPartManager(apm =>
            {
                if (_ModuleManager.Modules != null)
                {
                    foreach (ModuleBase module in _ModuleManager.Modules)
                    {
                        Assembly assembly = module.GetType().Assembly;
                        var part = new AssemblyPart(assembly);
                        apm.ApplicationParts.Add(part);
                    }
                }
            });

            // ------------------------------------

            //foreach (ModuleBase module in _ModuleManager.Modules)
            //{
            //    //foreach (ResourceInfo resourceInfo in module.Resources)
            //    //{
            //    //    if (resourceInfo.Extension.ToLower() == "cshtml")
            //    //    {
            //    //        services.Configure<RazorViewEngineOptions>(o =>
            //    //        {
            //    //            o.ViewLocationFormats.Add("/" + resourceInfo.Path + "/" + resourceInfo.FileName);
            //    //        });
            //    //    }
            //    //}
            //}

            // ------------------------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // ------------------------------------

            if (_ModuleManager.Modules != null)
            {
                foreach (ModuleBase module in _ModuleManager.Modules)
                {
                    Assembly assembly = module.GetType().Assembly;
                    string resourceNamespace = assembly.GetName().Name;
                    EmbeddedFileProvider fileProvider = new EmbeddedFileProvider(assembly);

                    foreach (ResourceInfo resourceInfo in module.Resources)
                    {
                        var options = new StaticFileOptions()
                        {
                            FileProvider = new EmbeddedFileProvider(assembly),
                            RequestPath = "" //new PathString("/" + resourceInfo.Path)
                        };

                        app.UseStaticFiles(options);
                    }

                    //IDirectoryContents dirContents = fileProvider.GetDirectoryContents("");
                    //foreach (IFileInfo fileInfo in dirContents)
                    //{
                    //    Stream viewStream = assembly.GetManifestResourceStream(resourceNamespace + "." + fileInfo.Name);
                    //    TextReader reader = new StreamReader(viewStream);
                    //    string viewCode = reader.ReadToEnd();
                    //}
                }
            }

            // ------------------------------------

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
