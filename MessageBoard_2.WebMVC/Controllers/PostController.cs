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
            model = ParseEmotes(model); //parse emotes into HTML
            model = ParseFormatting(model);
            this.Session["currentThread"] = CurrentThreadID.ToString(); //sets current thread.
            //Session["currentThread"] should only be used when redirecting to the index page from a view.
            ViewBag.Title = threadService.GetThreadTitle(CurrentThreadID);
            ViewBag.Op = threadService.GetThreadCreatorID(CurrentThreadID); //TODO: re-add op highlighting
            ViewBag.UserID = Guid.Parse(User.Identity.GetUserId());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string body, object nullObject) //Used with the inline reply form
        {
            var newPost = new PostCreate()
            {
                ThreadID = Guid.Parse(this.Session["currentThread"].ToString()),
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
            var model = new PostCreate
            {
                ThreadID = Guid.Parse(Session["currentThread"].ToString()),
                Body = ""
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostCreate model)
        {
            //model.ThreadID = Guid.Parse(this.Session["currentThread"].ToString()); //important
            //TODO: model state is not valid because not all properties are filled ?
            if (!ModelState.IsValid) //TODO: bug - ModelState.IsValid returning false for null threadID, even though it is added above. Temporary fix.
            {
                ModelState.AddModelError("", "Your post must contain at least 2 characters.");
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

            model = AssignCreatorTypes(model); //rank users based on post count
            ViewBag.Thread = Session["currentThread"];
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, PostListItem model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreatePostService();

            //if (!User.IsInRole("Admin") || User.Identity.GetUserId() != service.GetPostById(id).CreatorID.ToString())
            //{
            //    return RedirectToAction("Index");
            //}

            if (model.PostID != id)
            {
                ModelState.AddModelError("", "ID Mismatch");
                return View(model);
            }

            if (service.UpdatePost(model))
            {
                TempData["ResultSaved"] = "Post was updated.";
                return Redirect(Url.RouteUrl(new { controller = "Post", action = "Index", threadId = Session["currentThread"] }) + "#new_reply");
            }

            ModelState.AddModelError("", "Post could not be updated.");
            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            var svc = CreatePostService();
            var model = svc.GetPostById(id);
            model = AssignCreatorTypes(model);
            ViewBag.Thread = Session["currentThread"];
            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(Guid id)
        {
            var service = CreatePostService();

            //if (!User.IsInRole("Admin") || User.Identity.GetUserId() != service.GetPostById(id).CreatorID.ToString())
            //{
            //    return RedirectToAction("Index");
            //}

            service.DeletePost(id);

            TempData["SaveResult"] = "Post was deleted";

            return RedirectToAction("Index", new { threadId = Session["currentThread"] }); //provides index parameter

        }

        #region helper methods
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

        Dictionary<String, int> PostAmounts = new Dictionary<string, int>()
        {
            {"Noob", 10 },
            {"Regular", 30 }
            //veteran = 30+
        };

        private IEnumerable<PostListItem> AssignCreatorTypes(IEnumerable<PostListItem> model)
        {
            foreach (var item in model)
            {
                int posts = item.CreatorPostCount;
                if (posts < PostAmounts["Noob"])
                    item.CreatorType = "Noob";
                else if (posts >= PostAmounts["Noob"] && posts < PostAmounts["Regular"])
                    item.CreatorType = "Regular";
                else
                    item.CreatorType = "Veteran";
            }
            return model;
        }

        private PostListItem AssignCreatorTypes(PostListItem model)
        {
            int posts = model.CreatorPostCount;
            if (posts < PostAmounts["Noob"])
                model.CreatorType = "Noob";
            else if (posts >= PostAmounts["Noob"] && posts < PostAmounts["Regular"])
                model.CreatorType = "Regular";
            else
                model.CreatorType = "Veteran";
            return model;
        }

        private IEnumerable<PostListItem> ParseEmotes(IEnumerable<PostListItem> model)
        {
            foreach (var item in model)
            {
                item.Body = item.Body.Replace(":eyeroll:", "<img src=\"content/emotes/eyeroll.png\" class=\"emote\" />");
                item.Body = item.Body.Replace(":wink:", "<img src=\"content/emotes/wink.png\" class=\"emote\" />");
                item.Body = item.Body.Replace(":lol:", "<img src=\"content/emotes/lol.png\" class=\"emote\" />");
                item.Body = item.Body.Replace(":blah:", "<img src=\"content/emotes/blah.png\" class=\"emote\" />");
                item.Body = item.Body.Replace(":shades:", "<img src=\"content/emotes/shades.png\" class=\"emote\" />");

            }
            return model;
        }

        private IEnumerable<PostListItem> ParseFormatting(IEnumerable<PostListItem> model)
        {
            var imgLinkParser = new ImgLinkParser();
            foreach (var item in model)
            {
                item.Body = item.Body.Replace("[b]", "<b>");
                item.Body = item.Body.Replace("[/b]", "</b>");

                item.Body = item.Body.Replace("[i]", "<i>");
                item.Body = item.Body.Replace("[/i]", "</i>");

                item.Body = item.Body.Replace("[h]", "<h3>");
                item.Body = item.Body.Replace("[/h]", "</h3>");

                item.Body = item.Body.Replace("[u]", "<u>");
                item.Body = item.Body.Replace("[/u]", "</u>");

                item.Body = imgLinkParser.Parse(item.Body);
            }
            return model;
        }

        #endregion
    }
}