using MessageBoard_2.Models.Thread;
using MessageBoard_2.Services;
using Microsoft.AspNet.Identity;
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
		public int currentSectionId = 99;

		// GET: Thread
		[Route("")]
		public ActionResult Index(int sectionId)
        {
			int id = (int)sectionId;
			var service = CreateThreadService();
			var model = service.GetThreadsBySection(id);
			currentSectionId = id;
			return View(model);
		}

		public ActionResult Create()
		{
			return View();
		}

		//SET
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ThreadCreate model)
		{
			model.SectionID = currentSectionId;

			if (!ModelState.IsValid) return View(model);

			var service = CreateThreadService();

			if (service.CreateThread(model))
			{
				TempData["ResultSaved"] = "Your thread was created.";
				return RedirectToAction("Index");
			};

			ModelState.AddModelError("", "Thread could not be created.");

			return View(model);
		}

		public ActionResult Details(Guid id)
		{
			var svc = CreateThreadService();
			var model = svc.GetThreadById(id);

			return View(model);
		}

		public ActionResult Edit(Guid id)
		{
			var service = CreateThreadService();
			var detail = service.GetThreadById(id);
			var model =
				new ThreadEdit
				{
					ThreadID = detail.ThreadID,
					Title = detail.Title
				};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Guid id, ThreadEdit model)
		{
			if (!ModelState.IsValid) return View(model);

			if (model.ThreadID != id)
			{
				ModelState.AddModelError("", "ID Mismatch");
				return View(model);
			}

			var service = CreateThreadService();

			if (service.UpdateThread(model))
			{
				TempData["ResultSaved"] = "Thread was updated.";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("", "Thread could not be updated.");
			return View(model);
		}

		public ActionResult Delete(Guid id)
		{
			var svc = CreateThreadService();
			var model = svc.GetThreadById(id);

			return View(model);
		}

		[HttpPost]
		[ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteSection(Guid id)
		{
			var service = CreateThreadService();

			service.DeleteThread(id);

			TempData["SaveResult"] = "The thread was deleted";

			return RedirectToAction("Index");
		}

		//Helper methods
		private ThreadService CreateThreadService()
		{
			var userId = Guid.Parse(User.Identity.GetUserId());
			var service = new ThreadService(userId);
			return service;
		}
	}
}