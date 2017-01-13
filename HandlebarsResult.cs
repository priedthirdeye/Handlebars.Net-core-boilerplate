using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace HandebarsDotNetCore
{
    internal class HandlebarsResult : ActionResult
    {
        private byte[] _stringAsByteArray;
        public HandlebarsResult(string stringToWrite)
        {
            _stringAsByteArray = Encoding.ASCII.GetBytes(stringToWrite);
        }
        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            return context.HttpContext.Response.Body.WriteAsync(_stringAsByteArray, 0, _stringAsByteArray.Length);
        }
    }
}
