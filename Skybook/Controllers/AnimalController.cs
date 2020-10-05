using Microsoft.AspNet.Identity;
using Skybook.Models.Animal;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    public class AnimalController : Controller
    {
        // GET: Animal
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new AnimalService(userId);
            var model = service.GetAnimals();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnimalCreate model)
        {
            if (!ModelState.IsValid) return View(model);

            var service = CreateAnimalService();

            if (service.CreateAnimal(model))
            {
                TempData["SaveResult"] = "Your Animal has been created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The Animal could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreateAnimalService();
            var model = svc.GetAnimalById(id);

            return View(model);
        }


        private AnimalService CreateAnimalService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new AnimalService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreateAnimalService();
            var detail = service.GetAnimalById(id);
            var model =
                new AnimalEdit
                {
                    AnimalId = detail.AnimalId,
                    Name = detail.Name,
                    Description = detail.Description,
                    PlanetId = detail.PlanetId
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AnimalEdit model)
        {
            if (!ModelState.IsValid) return View(model);

            if (model.AnimalId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreateAnimalService();

            if (service.UpdateAnimal(model))
            {
                TempData["SaveResult"] = "Your Animal was updated.";
                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var svc = CreateAnimalService();
            var model = svc.GetAnimalById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAnimal(int id)
        {
            var service = CreateAnimalService();

            service.DeleteAnimal(id);

            TempData["SaveResult"] = "Your Animal was deleted";

            return RedirectToAction("Index");
        }
    }
}