using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace weatherState.Controllers
{
    public class EndpointsController : Controller
    {
        [Route("/")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("now")]
        public ActionResult Now()
        {
            return View();
        }

        [Route("queries")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Queries()
        {
            var json = Requests.create();
            return Json(json);
        }

        [Route("searches")]
        public IActionResult Searches()
        {
            SqlServer.select();
            return Content("Success :)");
        }

        [Route("queries/new")]
        [HttpPost]
        public ActionResult AddComment(City data)
        {
            Console.WriteLine(data.city);
            if (data.city != null) {
                Comment.comment = data.city;
            }
            var json = Requests.create();
            SqlServer.Connection(ref json);
            string jsonString = JsonConvert.SerializeObject(json);
            Console.WriteLine(jsonString);
            return Content("Success :)");
        }

    }
}
