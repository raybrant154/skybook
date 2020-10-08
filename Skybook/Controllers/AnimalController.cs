using Microsoft.AspNet.Identity;
using Skybook.Data;
using Skybook.Models.Animal;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            var service = CreateAnimalService();

            if (service.CreateAnimal(file, model))
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
                    Image = detail.Image,
                    PlanetId = detail.PlanetId
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AnimalEdit model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            if (model.AnimalId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreateAnimalService();

            if (service.UpdateAnimal(file, model))
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

        public ActionResult RetrieveImage(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new AnimalService(userId);
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