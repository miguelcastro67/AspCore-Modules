using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WebModule
{
    public class ModuleManager
    {
        //IEnumerable<ModuleInfo> _Modules = null;
        IEnumerable<ModuleBase> _Modules = null;

        public IEnumerable<ModuleBase> Modules
        {
            get { return _Modules; }
        }

        public void Register(Type moduleType)
        {
            ModuleBase module = Activator.CreateInstance(moduleType) as ModuleBase;
            if (module != null)
            {
                Assembly assembly = moduleType.Assembly;
                IEnumerable<string> resources = assembly.GetManifestResourceNames();
                module.Resources = new ResourceInfoList();

                // TODO: need to isolate the file name (and possible folder path) from the resource
                // example:
                //   SampleModule1.Views.Sample1.Index.cshtml
                //     namespace = SampleModule1
                //     folder path = Views/Sample1
                //     file name = Index.chtml
                //   SampleModule1.wwwroot.css.site.css
                //     namespace = SampleModule1
                //     folder path = wwwroot/css
                //     file name = site.css

                // last "." section is extension (cshtml, css)
                // use project's namespace to eliminate namespace section
                // what's left is path and file

                string assemblyName = assembly.GetName().Name; // also default namespace so MUST BE KEPT THE SAME
                foreach (string resourceItem in resources)
                {
                    string[] parts = resourceItem.Split('.');
                    string extension = parts[parts.Length - 1];

                    ResourceInfo resourceInfo = new ResourceInfo()
                    {
                        FullName = resourceItem,
                        Extension = extension
                    };

                    string resourcePath = resourceItem.Replace(assemblyName + ".", ""); // not used yet

                    module.Resources.Add(resourceInfo);
                }

                if (_Modules == null)
                    _Modules = new List<ModuleBase>();

                ((List<ModuleBase>)_Modules).Add(module);
            }
        }

        public void RegisterAll()
        {
            string folder = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetEntryAssembly().CodeBase).Path));
            string[] files = Directory.GetFiles(folder, "*.dll");
            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    IEnumerable<string> embeddedResources = assembly.GetManifestResourceNames();
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsSubclassOf(typeof(ModuleBase)))
                        {
                            //ModuleBase module = Activator.CreateInstance(type) as ModuleBase;
                            //if (module != null)
                            //{
                            //    module.Resources = new ResourceInfoList();
                            //    module.RegisterResources(module.Resources);
                            //    
                            //    string assemblyName = assembly.GetName().Name; // also default namespace so MUST BE KEPT THE SAME
                            //    foreach (ResourceInfo resourceInfo in module.Resources)
                            //    {
                            //        // ensure resource is indeed there
                            //        string embeddedResource = embeddedResources.FirstOrDefault(item => item == resourceInfo.FullName);
                            //        if (!string.IsNullOrWhiteSpace(embeddedResource))
                            //        {
                            //            string[] parts = resourceInfo.FullName.Split('.');
                            //            string extension = parts[parts.Length - 1];
                            //            string resourcePath = resourceInfo.FullName.Replace(assemblyName + ".", "")
                            //                                                       .Replace("." + resourceInfo.FileName, "")
                            //                                                       .Replace("wwwroot.", "")
                            //                                                       .Replace(".", "/");
                            //            resourceInfo.Extension = extension;
                            //            resourceInfo.Path = resourcePath;
                            //        }
                            //    }

                            //    if (_Modules == null)
                            //        _Modules = new List<ModuleBase>();

                            //    ((List<ModuleBase>)_Modules).Add(module);
                            //}

                            Register(type);
                            break;
                        }
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
