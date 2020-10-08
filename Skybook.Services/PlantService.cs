using Skybook.Data;
using Skybook.Models.Plant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Skybook.Services
{
    public class PlantService
    {
        private readonly Guid _userId;
        public PlantService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePlant(HttpPostedFileBase file, PlantCreate model)
        {
            model.Image = ConvertToBytes(file);

            var entity =
                new Plant()
                {
                    Name = model.Name,
                    PrimaryElement = model.PrimaryElement,
                    SecondaryElement = model.SecondaryElement,
                    Description = model.Description,
                    PlanetId = model.PlanetId,
                    Image = model.Image
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Plants.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var q = from temp in db.Plants where temp.PlantId == Id select temp.Image;
                byte[] cover = q.First();
                return cover;
            }
        }

        public IEnumerable<PlantListItem> GetPlants()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Plants
                    .Where(e => e.Planet.StarSystem.OwnerId == _userId)
                    .Select(
                        e =>
                        new PlantListItem
                        {
                            PlantId = e.PlantId,
                            Name = e.Name,
                            PrimaryElement = e.PrimaryElement,
                            SecondaryElement = e.SecondaryElement,
                            Description = e.Description,
                            PlanetId = e.PlanetId
                            
                        }
                        );
                return query.ToArray();
            }
        }

        public PlantDetail GetPlantById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == id && e.Planet.StarSystem.OwnerId == _userId);
                return
                    new PlantDetail
                    {
                        PlantId = entity.PlantId,
                        Name = entity.Name,
                        PrimaryElement = entity.PrimaryElement,
                        SecondaryElement = entity.SecondaryElement,
                        Description = entity.Description,
                        PlanetId = entity.PlanetId
                    };
            }
        }

        public bool UpdatePlant(HttpPostedFileBase file, PlantEdit model)
        {
            model.Image = ConvertToBytes(file);

            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == model.PlantId && e.Planet.StarSystem.OwnerId == _userId);

                entity.PlantId = model.PlantId;
                entity.Name = model.Name;
                entity.PrimaryElement = model.PrimaryElement;
                entity.SecondaryElement = model.SecondaryElement;
                entity.Description = model.Description;
                entity.PlanetId = model.PlanetId;
                entity.Image = model.Image;

                return ctx.SaveChanges() == 1;

            }
        }

        public bool DeletePlant(int plantId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Plants
                    .Single(e => e.PlantId == plantId && e.Planet.StarSystem.OwnerId == _userId);

                ctx.Plants.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}
