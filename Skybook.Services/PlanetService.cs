using Skybook.Data;
using Skybook.Models.Animal;
using Skybook.Models.Planet;
using Skybook.Models.Plant;
using Skybook.Models.Rock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Skybook.Services
{
    public class PlanetService
    {
        private readonly Guid _userId;
        public PlanetService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreatePlanet(HttpPostedFileBase file, PlanetCreate model)
        {
            model.Image = ConvertToBytes(file);

            var entity =
                new Planet()
                {
                    Name = model.Name,
                    PlanetType = model.PlanetType,
                    Minerals = model.Minerals,
                    SpecialBuried = model.SpecialBuried,
                    SentinelActivity = model.SentinelActivity,
                    StarSystemId = model.StarSystemId,
                    Image = model.Image
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Planets.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var q = from temp in db.Planets where temp.PlanetId == Id select temp.Image;
                byte[] cover = q.First();
                return cover;
            }
        }

        public IEnumerable<PlanetListItem> GetPlanets()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Planets
                    .Where(e => e.StarSystem.OwnerId == _userId)
                    .Select(
                        e =>
                        new PlanetListItem
                        {
                            PlanetId = e.PlanetId,
                            Name = e.Name,
                            PlanetType = e.PlanetType,
                            Minerals = e.Minerals,
                            SpecialBuried = e.SpecialBuried,
                            SentinelActivity = e.SentinelActivity,
                            StarSystemId = e.StarSystemId
                        }
                        );
                return query.ToArray();
            }
        }

        public PlanetDetail GetPlanetById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Planets
                    .Single(e => e.PlanetId == id && e.StarSystem.OwnerId == _userId);
                return
                    new PlanetDetail
                    {
                        PlanetId = entity.PlanetId,
                        Name = entity.Name,
                        PlanetType = entity.PlanetType,
                        Minerals = entity.Minerals,
                        SpecialBuried = entity.SpecialBuried,
                        SentinelActivity = entity.SentinelActivity,
                        StarSystemId = entity.StarSystemId,

                        Animals = ctx.Animals.Where(e => e.PlanetId == id).Select(e => new AnimalListItem
                        {
                            AnimalId = e.AnimalId,
                            Name = e.Name,
                            Description = e.Description,

                        }).ToList(),

                        Plants = ctx.Plants.Where(e => e.PlanetId == id).Select(e => new PlantListItem
                        {
                            PlantId = e.PlantId,
                            Name = e.Name,
                            PrimaryElement = e.PrimaryElement,
                            SecondaryElement = e.SecondaryElement,
                            Description = e.Description,
                        }).ToList(),

                        Rocks = ctx.Rocks.Where(e => e.RockId == id).Select(e => new RockListItem
                        {
                            RockId = e.RockId,
                            Name = e.Name,
                            PrimaryElement = e.PrimaryElement,
                            SecondaryElement = e.SecondaryElement,
                            Description = e.Description,
                        }).ToList(),
                    };
             }
        }

        public bool UpdatePlanet(HttpPostedFileBase file, PlanetEdit model)
        {
                model.Image = ConvertToBytes(file);

            using (var ctx = new ApplicationDbContext())
            {

                var entity =
                    ctx.Planets
                    .Single(e => e.PlanetId == model.PlanetId && e.StarSystem.OwnerId == _userId);

                entity.PlanetId = model.PlanetId;
                entity.Name = model.Name;
                entity.PlanetType = model.PlanetType;
                entity.Minerals = model.Minerals;
                entity.SpecialBuried = model.SpecialBuried;
                entity.SentinelActivity = model.SentinelActivity;
                entity.StarSystemId = model.StarSystemId;
                entity.Image = model.Image;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeletePlanet(int planetId)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Planets
                    .Single(e => e.PlanetId == planetId && e.StarSystem.OwnerId == _userId);

                ctx.Planets.Remove(entity);

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
