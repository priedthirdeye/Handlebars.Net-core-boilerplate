using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using System.IO;
using HandlebarsDotNet;

namespace HandebarsDotNetCore
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole();

            app.Use((context, next) =>
            {                
                //Reponse is plain text unless I add this. Not sure why or the best place to add this.
                context.Response.ContentType = "text/html";
                return next();
            });

            // Using a dictionary object as a cache so templates are not read from disk for each request.
            Dictionary<string, Func<object, string>> templates = new Dictionary<string, Func<object, string>>();
            
            //Loop through Views directory recursively
            foreach (string file in Directory.EnumerateFiles("Views", "*.hbs", SearchOption.AllDirectories))
            {
                // If file is not a partial
                if (file.IndexOf(@"\partials\") == -1)
                {
                    //compile and use the file path as the key for the dictionary cache object
                    templates.Add(file.ToLower(), Handlebars.Compile(File.ReadAllText(file)));
                }
                else
                {
                    // If it is a partial, must be handled differently.   
                    using (var reader = new StringReader(File.ReadAllText(file)))
                    {
                        // Derive partial name from path, but remove "views\partials\" and ".hbs" suffix so partials can be referenced simply as {{> header}}
                        string partialName = file.ToLower().Replace(@"views\partials\", "").Replace(".hbs", "");
                        var partialTemplate = Handlebars.Compile(reader);
                        Handlebars.RegisterTemplate(partialName, partialTemplate);
                    }

                }          
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                // Ideally we would only load the templates into cache in production. Otherwise server needs restart on every template change even if using "dotnet watch run".
            }

            var routeBuilder = new RouteBuilder(app);

            // Index Page Route
            routeBuilder.MapGet("", context => {
                var data = new
                {                    
                    message = "Home Page Dynamic Data Working"
                };
                //Apparently this is an efficient way to retreive a value from a dictionary object by key
                Func<object, string> template;
                templates.TryGetValue(@"views\index.hbs", out template);
                return context.Response.WriteAsync(template(data));
            });

            // About Page Route
            routeBuilder.MapGet("about", context => {
                var data = new
                {
                    message = "Home Page Dynamic Data Working"
                };
                Func<object, string> template;
                templates.TryGetValue(@"views\about.hbs", out template);
                return context.Response.WriteAsync(template(data));
            });

            var routes = routeBuilder.Build(); 
            app.UseRouter(routes);
        }
    }
}
