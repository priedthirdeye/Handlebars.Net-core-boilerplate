﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Newtonsoft.Json;

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
                message = "Home Page Data. Using typed and anonymous object",
                users = GetUsers()
            };

            return this.Handlebars(model);
        }

        [HttpGet("{tenantName}/index")]
        public IActionResult TenantIndex(string tenantName)
        {
            var model = new
            {
                tenant = new  { name = tenantName },
                pageTitle = "This is a page title rendered into the layout",
                message = "Home Page Data. Using typed and anonymous object",
                users = GetUsers()
            };

            return this.Handlebars(model, "index");
        }


        [HttpGet("[action]")]
        public IActionResult IndexAnonymousObjects()
        {
            var model = new
            {
                pageTitle = "This is a page title rendered into the layout",
                message = "Home Page Data using anonymous objects.",
                users = new object[]
                {
                    new
                       {
                           firstName = "Oliver",
                           lastName = "Twist"
                       },
                        new
                       {
                           firstName = "Peter",
                           lastName = "pan"
                       },
                         new
                       {
                           firstName = "John",
                           lastName = "Do"
                      },
                          new
                       {
                           firstName = "Anonymous",
                           lastName = "Object"
                      }
                }
            };

            return this.Handlebars(model, "index");
        }

        [HttpGet("[action]")]
        public IActionResult IndexDynamic()
        {

            dynamic model = new ExpandoObject();

            model.pageTitle = "This is a page title rendered into the layout";
            model.message = "Home Page Data using expando object.";

            dynamic user1 = new ExpandoObject();
            user1.firstName = "Oliver";
            user1.lastName = "Twist";

            dynamic user2 = new ExpandoObject();
            user2.firstName = "Expando";
            user2.lastName = "Object";

            model.users = new List<object>
            {
                user1,
                user2
            };

            return this.Handlebars((object)model, "index");
        }

        [HttpGet("[action]")]
        public IActionResult IndexJson()
        {
            var model = JsonConvert.DeserializeObject(@"{
                pageTitle: ""This is a page title rendered into the layout"",
                message: ""Home page data loaded from JSON"",
                users: [
                    { ""firstName"": ""Oliver"", ""lastName"": ""Twist""}, 
                    { ""firstName"": ""Peter"", ""lastName"": ""Pan""},
                    { ""firstName"": ""Jay"", ""lastName"": ""Son""}
                ]
            }");

            return this.Handlebars(model, "index");
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
