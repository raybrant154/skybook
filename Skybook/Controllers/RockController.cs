using Microsoft.AspNet.Identity;
using Skybook.Data;
using Skybook.Models.Rock;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    [Authorize]
    public class RockController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Rock
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RockService(userId);
            var model = service.GetRocks();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RockCreate model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            var service = CreateRockService();

            if (service.CreateRock(file, model))
            {
                TempData["SaveResult"] = "Your Rock has been created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The Rock could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateRockService();
            var model = svc.GetRockById(id);

            return View(model);
        }

        private RockService CreateRockService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RockService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreateRockService();
            var detail = service.GetRockById(id);
            var model =
                new RockEdit
                {
                    RockId = detail.RockId,
                    Name = detail.Name,
                    PrimaryElement = detail.PrimaryElement,
                    SecondaryElement = detail.SecondaryElement,
                    Description = detail.Description,
                    Image = detail.Image,
                    PlanetId = detail.PlanetId
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RockEdit model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            if (model.RockId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreateRockService();

            if (service.UpdateRock(file, model))
            {
                TempData["SaveResult"] = "Your Rock was updated.";
                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateRockService();
            var model = svc.GetRockById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRock(int id)
        {
            var service = CreateRockService();

            service.DeleteRock(id);

            TempData["SaveResult"] = "Your Rock was deleted";

            return RedirectToAction("Index");
        }

        public ActionResult RetrieveImage(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new RockService(userId);
            byte[] cover = service.GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
    }
}