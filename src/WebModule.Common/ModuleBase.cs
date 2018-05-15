using System;
using System.Linq;

namespace WebModule
{
    public class ModuleBase
    {
        public ResourceInfoList Resources { get; internal set; }
        
        public virtual void RegisterResources(ResourceInfoList resources)
        {            
        }

        /*
         * menus and navigation contribution
         * notification
         * versioning (version update notification)
         * dashboard component
         * other
         */
    }
}
