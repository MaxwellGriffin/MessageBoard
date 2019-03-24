using MessageBoard_2.Models.Post;
using MessageBoard_2.Models.Thread;
using MessageBoard_2.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MessageBoard_2.WebMVC.Controllers
{
	[Authorize]
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(string threadId) //recieves new thread id
        {
			//TODO: Add thread name
			Guid CurrentThreadID = Guid.Parse(threadId);
			var service = CreatePostService();
			var threadService = CreateThreadService();
			var model = service.GetPostsByThread(CurrentThreadID);
			model = AssignCreatorTypes(model); //rank users based on post count
			this.Session["currentThread"] = CurrentThreadID.ToString(); //sets current thread.
			//Session["currentThread"] should only be used when redirecting to the index page from a view.
			ViewBag.Title = threadService.GetThreadTitle(CurrentThreadID);
			ViewBag.Op = threadService.GetThreadCreatorID(CurrentThreadID);
			ViewBag.UserID = Guid.Parse(User.Identity.GetUserId());
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(string body, object nullObject) //Used with the inline reply form
		{
			var newPost = new PostCreate()
			{
				Body = body
			};
			Create(newPost);
			//return RedirectToAction("Index", new { threadId = Session["currentThread"] }); //provides index parameter
			return Redirect(Url.RouteUrl(new { controller = "Post", action = "Index", threadId = Session["currentThread"] }) + "#new_reply");
		}

		public ActionResult Create()
		{
			var threadService = CreateThreadService();
			ViewBag.Thread = threadService.GetThreadTitle(Guid.Parse(Session["currentThread"].ToString()));
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PostCreate model)
		{
			model.ThreadID = Guid.Parse(this.Session["currentThread"].ToString()); //important
			//TODO: model state is not valid because not all properties are filled ?
			if (!ModelState.IsValid) //coming back true no matter what
			{
				return View(model);
			}

			var service = CreatePostService();

			if (service.CreatePost(model))
			{
				TempData["ResultSaved"] = "Post was created.";
				return Redirect(Url.RouteUrl(new { controller = "Post", action = "Index", threadId = Session["currentThread"] }) + "#new_reply");
			}

			ModelState.AddModelError("", "Post could not be created.");

			return View(model);
		}

		public ActionResult Edit(Guid id) //TODO: check if user is owner/admin before allowing edit
		{//TODO: Make this a postlistitem
			var service = CreatePostService();
			var model = service.GetPostById(id);
			ViewBag.Thread = Session["currentThread"];
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Guid id, PostEdit model)
		{
			if (!ModelState.IsValid) return View(model);

			if (model.PostID != id)
			{
				ModelState.AddModelError("", "ID Mismatch");
				return View(model);
			}

			var service = CreatePostService();

			if (service.UpdatePost(model))
			{
				TempData["ResultSaved"] = "Post was updated.";
				return RedirectToAction("Index", new { threadId = Session["currentThread"] }); //provides index parameter
			}

			ModelState.AddModelError("", "Post could not be updated.");
			return View(model);
		}

		public ActionResult Delete(Guid id)
		{
			var svc = CreatePostService();
			var model = svc.GetPostById(id);

			return View(model);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteSection(Guid id)
		{
			var service = CreatePostService();

			service.DeletePost(id);

			TempData["SaveResult"] = "Post was deleted";

			return RedirectToAction("Index");
		}

		//Helper methods
		private PostService CreatePostService()
		{
			var userId = Guid.Parse(User.Identity.GetUserId());
			var service = new PostService(userId);
			return service;
		}

		private ThreadService CreateThreadService()
		{
			var userId = Guid.Parse(User.Identity.GetUserId());
			var service = new ThreadService(userId);
			return service;
		}

		private IEnumerable<PostListItem> AssignCreatorTypes(IEnumerable<PostListItem> model)
		{
			foreach (var item in model)
			{
				int posts = item.CreatorPostCount;
				if (posts < 10)
					item.CreatorType = "Noob";
				else if (posts >= 10 && posts < 30)
					item.CreatorType = "Regular";
				else
					item.CreatorType = "Veteran";
			}
			return model;
		}
	}
}