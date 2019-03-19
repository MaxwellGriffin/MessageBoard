using MessageBoard_2.Data;
using MessageBoard_2.Models.Account;
using MessageBoard_2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageBoard_2.WebMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
			var ctx = new ApplicationDbContext();
			ViewBag.Name = new SelectList((ctx.Roles).ToList(), "Name", "Name");
			var svc = new AdminService();
			return View(svc.GetAccounts());
		}

		[HttpPost]
		public ActionResult Index(AccountListItem model)
		{


			return Index();
		}
	}
}