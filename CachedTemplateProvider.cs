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
        private ITemplateProvider innerProvider;
        private IMemoryCache memoryCache;

        public CachedTemplateProvider(IMemoryCache memoryCache)
        {
            this.innerProvider = new FileSystemTemplateProvider();
            this.memoryCache = memoryCache;
        }

        public IHandlebars GetEnvironment()
        {
            return this.memoryCache.GetOrCreate($"tmpl_environment", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.innerProvider.GetEnvironment();
            });
        }

        public Func<object, string> GetTemplate(string key)
        {
            return this.memoryCache.GetOrCreate($"tmpl_compiled_{key}", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.innerProvider.GetTemplate(key);
            });
        }

        public Func<object, string> GetLayoutTemplate(string key)
        {
            return this.memoryCache.GetOrCreate($"tmpl_layout_compiled_{key}", (entry) =>
            {
                entry.Priority = CacheItemPriority.High;
                return this.innerProvider.GetLayoutTemplate(key);
            });
        }
    }
}
