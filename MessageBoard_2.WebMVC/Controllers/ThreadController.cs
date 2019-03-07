using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard_2.WebMVC.Controllers
{
    public class ThreadController : Controller
    {
        // GET: Thread
        public ActionResult Index()
        {
            return View();
        }
    }
}