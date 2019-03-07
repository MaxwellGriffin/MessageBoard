using MessageBoard_2.Models.Section;
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
	public class SectionController : Controller
    {
        // GET: Section
        public ActionResult Index()
		{
			var service = CreateSectionService();
			var model = service.GetSectionsAll();

			return View(model);
		}

		public ActionResult Create() //GET
		{
			return View();
		}

		//SET
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(SectionCreate model)
		{
			if (!ModelState.IsValid) return View(model);

			var service = CreateSectionService();

			if (service.CreateSection(model))
			{
				TempData["ResultSaved"] = "Your section was created.";
				return RedirectToAction("Index");
			};

			ModelState.AddModelError("", "Section could not be created.");

			return View(model);
		}

		//Helper methods
		private SectionService CreateSectionService()
		{
			var userId = Guid.Parse(User.Identity.GetUserId());
			var service = new SectionService(userId);
			return service;
		}
	}
}