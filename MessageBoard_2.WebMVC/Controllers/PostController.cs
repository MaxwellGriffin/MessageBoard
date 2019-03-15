using MessageBoard_2.Models.Post;
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
			this.Session["currentThread"] = CurrentThreadID.ToString(); //sets current thread.
			//Session["currentThread"] should only be used when redirecting to the index page from a view.
			ViewBag.Title = threadService.GetThreadTitle(CurrentThreadID);
			ViewBag.Op = threadService.GetThreadCreatorID(CurrentThreadID);
			ViewBag.UserID = Guid.Parse(User.Identity.GetUserId());
			return View(model);
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
			model.ThreadID = Guid.Parse(this.Session["currentThread"].ToString()); //important(?)
			//TODO: model state is not valid because not all properties are filled ?
			//if (!ModelState.IsValid) return View(model); 

			var service = CreatePostService();

			if (service.CreatePost(model))
			{
				TempData["ResultSaved"] = "Post was created.";
				return RedirectToAction("Index", new { threadId = Session["currentThread"] }); //provides index parameter
			}

			ModelState.AddModelError("", "Post could not be created.");

			return View(model);
		}

		public ActionResult Edit(Guid id) //TODO: check if user is owner/admin before allowing edit
		{
			var service = CreatePostService();
			var detail = service.GetPostById(id);
			var model =
				new PostEdit
				{
					PostID = detail.PostID,
					Body = detail.Body
				};
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
	}
}