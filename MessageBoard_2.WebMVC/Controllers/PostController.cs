using MessageBoard_2.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard_2.WebMVC.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
			var model = new PostListItem[0];
			return View(model);
		}
    }
}