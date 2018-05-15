using System;
using System.Collections.Generic;
using System.Linq;

namespace WebModule
{
    public class ResourceInfoList : List<ResourceInfo>
    {
        public void Add(string fullName, string fileName)
        {
            ResourceInfo resourceInfo = new ResourceInfo()
            {
                FullName = fullName,
                FileName = fileName
            };

            base.Add(resourceInfo);
        }
    }
}
