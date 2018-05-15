using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SampleModule1.Controllers
{
    //[Route("module/sample1")]
    public class Sample1Controller : Controller
    {
        //[Route("home")]
        public ActionResult Index()
        {
            ViewBag.Message = "Your sample 1 module home page.";

            return View();
        }
    }
}