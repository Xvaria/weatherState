using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using weatherState;

namespace weatherState
{
    [ApiController]
    public class Endpoints : Controller
    {
        [HttpGet("queries")]
        public IActionResult Queries()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        [HttpGet("searches")]
        public IActionResult Searchs(string city)
        {
            var json = Requests.news();
            SqlServer.Connection(ref json);
            ViewData["Message"] = json.name;
            return View();
        }
    }
}
