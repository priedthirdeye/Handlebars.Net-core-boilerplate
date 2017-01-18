using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HandebarsDotNetCore
{
    [Route("")]
    public class SampleController : Controller
    {
        private IEnumerable<Models.User> GetUsers()
        {
            return new Models.User[]
                    {
                       new Models.User
                       {
                           firstName = "Oliver",
                           lastName = "Twist"
                       },
                        new Models.User
                       {
                           firstName = "Peter",
                           lastName = "pan"
                       },
                         new Models.User
                       {
                           firstName = "John",
                           lastName = "Do"
                       },
                    };
        }

        // GET: api/values
        [HttpGet()]
        public IActionResult Index()
        {
            var model = new
            {
                pageTitle = "This is a page title rendered into the layout",
                message = "Home Page Data. Need to support a loop in data structure.",
                users = GetUsers()
            };

            return this.Handlebars(model);
        }

        // GET api/values/5
        [HttpGet("[action]")]
        public IActionResult About()
        {
            return this.Handlebars(new Dictionary<string, object>
            {
                ["pageTitle"] = "This is a page title rendered into the layout",
                ["message"] = "About Page Dynamic Data Working"
            });
        }
    }
}
