using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SampleModule2.Controllers
{
    //[Route("module/sample2")]
    public class Sample2Controller : Controller
    {
        //[Route("home")]
        public ActionResult Index()
        {
            ViewBag.Message = "Your sample 2 module home page.";

            return View();
        }
    }
}