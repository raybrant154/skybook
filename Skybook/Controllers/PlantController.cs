using Microsoft.AspNet.Identity;
using Skybook.Data;
using Skybook.Models.Plant;
using Skybook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skybook.Controllers
{
    [Authorize]

    public class PlantController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: Plant
        public ActionResult Index()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlantService(userId);
            var model = service.GetPlants();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantCreate model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            var service = CreatePlantService();

            if (service.CreatePlant(file, model))
            {
                TempData["SaveResult"] = "Your Plant has been created.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "The Plant could not be created.");

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var svc = CreatePlantService();
            var model = svc.GetPlantById(id);

            return View(model);
        }

        private PlantService CreatePlantService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlantService(userId);
            return service;
        }

        public ActionResult Edit(int id)
        {
            var service = CreatePlantService();
            var detail = service.GetPlantById(id);
            var model =
                new PlantEdit
                {
                    PlantId = detail.PlantId,
                    Name = detail.Name,
                    PrimaryElement = detail.PrimaryElement,
                    SecondaryElement = detail.SecondaryElement,
                    Description = detail.Description,
                    PlanetId = detail.PlanetId,
                    Image = detail.Image
                };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PlantEdit model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];

            if (!ModelState.IsValid) return View(model);

            if (model.PlantId != id)
            {
                ModelState.AddModelError("", "This ID does not work");
                return View(model);
            }

            var service = CreatePlantService();

            if (service.UpdatePlant(file, model))
            {
                TempData["SaveResult"] = "Your Plant was updated.";
                return RedirectToAction("Index");

            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var svc = CreatePlantService();
            var model = svc.GetPlantById(id);

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePlant(int id)
        {
            var service = CreatePlantService();

            service.DeletePlant(id);

            TempData["SaveResult"] = "Your Plant was deleted";

            return RedirectToAction("Index");
        }

        public ActionResult RetrieveImage(int id)
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new PlantService(userId);
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