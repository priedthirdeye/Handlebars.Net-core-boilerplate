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
        public IHandlebars GetEnvironment()
        {
            var environment = Handlebars.Create();
            foreach (var file in Directory.EnumerateFiles("Views/partials"))
            {
                using (var reader = new StringReader(File.ReadAllText(file)))
                {
                    // Derive partial name from path, but remove "views\partials\" and ".hbs" suffix so partials can be referenced simply as {{> header}}
                    string partialName = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
                    var partialTemplate = environment.Compile(reader);
                    environment.RegisterTemplate(partialName, partialTemplate);
                }
            }

            return environment;
        }

        public Func<object, string> GetTemplate(string key)
        {
            return GetEnvironment().Compile(File.ReadAllText(Path.Combine("Views", key + ".hbs")));
        }

        public Func<object, string> GetLayoutTemplate(string key)
        {
            return GetEnvironment().Compile(File.ReadAllText(Path.Combine("Views/layouts", key + ".hbs")));
        }

    }
}
