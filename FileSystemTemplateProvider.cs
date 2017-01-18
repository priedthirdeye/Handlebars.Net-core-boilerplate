using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandlebarsDotNet;
using System.IO;

namespace HandebarsDotNetCore
{
    public class FileSystemTemplateProvider : ITemplateProvider
    {
        private TemplateLoader templateLoader;

        public FileSystemTemplateProvider(TemplateLoader templateLoader)
        {
            this.templateLoader = templateLoader;
        }
        
        public Func<object, string> GetTemplate(string key)
        {
            return this.templateLoader.GetTemplate(this.templateLoader.CreateEnvironment(), key);
        }

        public Func<object, string> GetLayoutTemplate(string key)
        {
            return this.templateLoader.GetLayoutTemplate(this.templateLoader.CreateEnvironment(), key);
        }

    }
}
