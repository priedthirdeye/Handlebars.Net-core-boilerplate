using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HandebarsDotNetCore
{
    public static class HandlebarsResultExtension
    {
        public static HandlebarsResult Handlebars(this Controller controller, object model, [CallerMemberName] string templateName = null,  string layout = "default")
        {
            return new HandlebarsResult(templateName.ToLowerInvariant(), model, layout);
        }
    }
}
