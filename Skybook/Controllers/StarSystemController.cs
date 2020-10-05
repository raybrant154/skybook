using Microsoft.AspNet.Identity;
using Skybook.Data;
using Skybook.Models;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    [Authorize]
    public class StarSystemController : Controller
    {
        // GET: StarSystem
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new StarSystemService(userId);
            var model = service.GetStarSystems();

            return View(model);
        }

        
        //GET
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StarSystemCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateStarSystemService();

            if (service.CreateStarSystem(model))
            {
                TempData["SaveResult"] = "Your Star System created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The Star System could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateStarSystemService();
            var model = svc.GetStarSystemById(id);

            return View(model);
        }
        private StarSystemService CreateStarSystemService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new StarSystemService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreateStarSystemService();
            var detail = service.GetStarSystemById(id);
            var model =
                new StarSystemEdit
                {
                    StarSystemId = detail.StarSystemId,
                    Name = detail.Name,
                    Race = detail.Race,
                    Economy = detail.Economy,
                    Conflict = detail.Conflict
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StarSystemEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.StarSystemId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreateStarSystemService();

            if (service.UpdateStarSystem(model))
            {
                TempData["SaveResult"] = "Your Star System was updated.";
                return RedirectToAction("Index");

            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            var svc = CreateStarSystemService();
            var model = svc.GetStarSystemById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStarSystem(int id)
        {
            var service = CreateStarSystemService();

            service.DeleteStarSystem(id);

            TempData["SaveResult"] = "Your Star System was deleted";

            return RedirectToAction("Index");
        }

        

    }
}