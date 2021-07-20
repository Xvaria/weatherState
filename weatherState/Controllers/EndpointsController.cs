using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace weatherState.Controllers
{
    public class EndpointsController : Controller
    {
        [Route("now")]
        public ActionResult Now()
        {
            return View();
        }

        [Route("queries")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Queries()
        {
            var json = Json(Requests.create());
            return json;
        }

        [Route("searches")]
        public IActionResult Searches()
        {
            var json = Requests.create();
            SqlServer.Connection(ref json);
            return View();
        }

        [Route("queries/new")]
        [HttpPost]
        public ActionResult AddComment(City data)
        {
            Console.WriteLine(data.city);
            if (data.city != null) {
                Comment.comment = data.city;
            }
            Requests.create();
            return Content("Success :)");
        }

    }
}
