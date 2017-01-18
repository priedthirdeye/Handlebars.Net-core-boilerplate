using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;

namespace HandebarsDotNetCore
{
    public class HandlebarsResult : ContentResult
    {
        private string layout;
        private object model;
        private string templateName;

        public HandlebarsResult(string templateName, object model, string layout)
        {
            this.templateName = templateName;
            this.model = model;
            this.layout = layout;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var templateProvider = context.HttpContext.RequestServices.GetRequiredService<ITemplateProvider>();
            var bodyTemplate = templateProvider.GetTemplate(this.templateName);

            var body = bodyTemplate(model);

            var layoutTemplate = templateProvider.GetLayoutTemplate(this.layout);

            var page = layoutTemplate(new LayoutViewModel
            {
                Model = model,
                Body = body
            });

            this.ContentType = "text/html";
            this.Content = page;

            return base.ExecuteResultAsync(context);
        }
    }
}
