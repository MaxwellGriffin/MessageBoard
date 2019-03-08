using MessageBoard_2.Models.Thread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard_2.WebMVC.Controllers
{
	[Authorize]
    public class ThreadController : Controller
    {
        // GET: Thread
        public ActionResult Index()
        {
			var model = new ThreadListItem[0];
			return View(model);
        }
    }
}