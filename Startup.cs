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
        // Using a dictionary object as a cache so templates are not read from disk for each request.
        Dictionary<string, Func<object, string>> templates = new Dictionary<string, Func<object, string>>();

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<TemplateLoader>();
            services.AddTransient<ITemplateProvider, FileSystemTemplateProvider>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<TemplateLoader>();
            services.AddTransient<ITemplateProvider, CachedTemplateProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole();

            //app.Use((context, next) =>
            //{                
            //    //Reponse is plain text unless I add this. Not sure why or the best place to add this.
            //    context.Response.ContentType = "text/html";
            //    return next();
            //});

            
            
            ////Loop through Views directory recursively
            //foreach (string file in Directory.EnumerateFiles("Views", "*.hbs", SearchOption.AllDirectories))
            //{
            //    // If file is not a partial
            //    if (file.IndexOf(@"\partials\") == -1)
            //    {
            //        //compile and use the file path as the key for the dictionary cache object
            //        templates.Add(file.ToLower(), Handlebars.Compile(File.ReadAllText(file)));
            //    }
            //    else
            //    {
            //        // If it is a partial, must be handled differently.   
            //        using (var reader = new StringReader(File.ReadAllText(file)))
            //        {
            //            // Derive partial name from path, but remove "views\partials\" and ".hbs" suffix so partials can be referenced simply as {{> header}}
            //            string partialName = file.ToLower().Replace(@"views\partials\", "").Replace(".hbs", "");
            //            var partialTemplate = Handlebars.Compile(reader);
            //            Handlebars.RegisterTemplate(partialName, partialTemplate);
            //        }
            //    }          
            //}
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                // Ideally we would only load the templates into cache in production. Otherwise server needs restart on every template change even if using "dotnet watch run".
            }

            //var routeBuilder = new RouteBuilder(app);  

            //// Index Page Route
            //routeBuilder.MapGet("", context => {                
            //    Dictionary<string, object> data = new Dictionary<string, object>(); // Apparently needed to use a dictionary in order to add "body" after the data object is passed to the render function. Otherwise it was throwing errors.
            //    data["pageTitle"] = "This is a page title rendered into the layout";
            //    data["message"] = "Home Page Data. Need to support a loop in data structure.";                
            //    //data["users"] = [{"firstName": "Mike", "lastName": "Testing"}];  // Not sure how to accomplish this. Need to expose loop data that is not strongly typed objects to templates 
            //    return context.Response.WriteAsync(render(@"views\index.hbs", data)); // ideally this could be written in a cleaner way. For example return render(@"views\index.hbs", data); or 
            //    //return new HandlebarsResult("Hello Index!"); //tried to implement "HandlebarsResult". Was unsuccessful.
            //});

            //// About Page Route
            //routeBuilder.MapGet("about", context => {
            //    Dictionary<string, object> data = new Dictionary<string, object>(); // Apparently needed to use a dictionary in order to add "body" after the data object is passed to the render function. Otherwise it was throwing errors.
            //    data["pageTitle"] = "This is a page title rendered into the layout";
            //    data["message"] = "About Page Dynamic Data Working";
            //    return context.Response.WriteAsync(render(@"views\about.hbs", data, "custom")); // ideally this could be written in a cleaner way. For example return render(@"views\index.hbs", data); or 
            //});

            //var routes = routeBuilder.Build();
            //app.UseRouter(routes); 

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }



   
        public string render(string path, dynamic data, string layout = "default")
        {
            Func<object, string> bodyTemplate;
            Func<object, string> layoutTemplate;     
            // Load body of main template from cache
            templates.TryGetValue(path, out bodyTemplate);
            // set parsed output to "body" property.
            data["body"] = bodyTemplate(data);
            // Load Layout from cache
            templates.TryGetValue(@"views\layouts\"+ layout + ".hbs", out layoutTemplate);
            // return parsed layout with data["body"] injected.
            return layoutTemplate(data);
        }
     
    }
}
