using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.Extensions.Caching.Memory;

namespace HandebarsDotNetCore
{
    public class CachedTemplateProvider : ITemplateProvider
    {
        private IMemoryCache memoryCache;
        private TemplateLoader templateLoader;

        public CachedTemplateProvider(IMemoryCache memoryCache, TemplateLoader templateLoader)
        {
            this.memoryCache = memoryCache;
            this.templateLoader = templateLoader;
        }

        public IHandlebars GetEnvironment()
        {
            return this.memoryCache.GetOrCreate($"tmpl_environment", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.templateLoader.CreateEnvironment();
            });
        }

        public Func<object, string> GetTemplate(string key)
        {
            return this.memoryCache.GetOrCreate($"tmpl_compiled_{key}", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.templateLoader.GetTemplate(this.GetEnvironment(),key);
            });
        }

        public Func<object, string> GetLayoutTemplate(string key)
        {
            return this.memoryCache.GetOrCreate($"tmpl_layout_compiled_{key}", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.templateLoader.GetLayoutTemplate(this.GetEnvironment(), key);
            });
        }
    }
}
