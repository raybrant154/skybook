using Microsoft.AspNet.Identity;
using Skybook.Data;
using Skybook.Models.Planet;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    public class PlanetController : Controller
    {
        // GET: Planet
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlanetService(userId);
            var model = service.GetPlanets();

            return View(model);
        }

        //GET
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanetCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreatePlanetService();

            if (service.CreatePlanet(model))
            {
                TempData["SaveResult"] = "Your Planet has been created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The Planet could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreatePlanetService();
            var model = svc.GetPlanetById(id);

            return View(model);
        }

        private PlanetService CreatePlanetService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlanetService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreatePlanetService();
            var detail = service.GetPlanetById(id);
            var model =
                new PlanetEdit
                {
                    PlanetId = detail.PlanetId,
                    Name = detail.Name,
                    PlanetType = detail.PlanetType,
                    Minerals = detail.Minerals,
                    SpecialBuried = detail.SpecialBuried,
                    SentinelActivity = detail.SentinelActivity,
                    StarSystemId = detail.StarSystemId,
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PlanetEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.PlanetId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreatePlanetService();

            if (service.UpdatePlanet(model))
            {
                TempData["SaveResult"] = "Your Planet was updated.";
                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var svc = CreatePlanetService();
            var model = svc.GetPlanetById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePlanet(int id)
        {
            var service = CreatePlanetService();

            service.DeletePlanet(id);

            TempData["SaveResult"] = "Your Planet was deleted";

            return RedirectToAction("Index");
        }
    }
}