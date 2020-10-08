using Skybook.Data;
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
    public class RockService
    {
        private readonly Guid _userId;
        public RockService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateRock(HttpPostedFileBase file, RockCreate model)
        {
            model.Image = ConvertToBytes(file);

            var entity =
                new Rock()
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
                ctx.Rocks.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var q = from temp in db.Rocks where temp.RockId == Id select temp.Image;
                byte[] cover = q.First();
                return cover;
            }
        }

        public IEnumerable<RockListItem> GetRocks()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Rocks
                    .Where(e => e.Planet.StarSystem.OwnerId == _userId)
                    .Select(
                        e =>
                        new RockListItem
                        {
                            RockId = e.RockId,
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

        public RockDetail GetRockById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Rocks
                    .Single(e => e.RockId == id && e.Planet.StarSystem.OwnerId == _userId);
                return
                    new RockDetail
                    {
                        RockId = entity.RockId,
                        Name = entity.Name,
                        PrimaryElement = entity.PrimaryElement,
                        SecondaryElement = entity.SecondaryElement,
                        Description = entity.Description,
                        PlanetId = entity.PlanetId
                    };
            }
        }

        public bool UpdateRock(HttpPostedFileBase file, RockEdit model)
        {
            model.Image = ConvertToBytes(file);

            using (var ctx = new ApplicationDbContext())
            {

                var entity =
                    ctx
                    .Rocks
                    .Single(e => e.RockId == model.RockId && e.Planet.StarSystem.OwnerId == _userId);

                entity.RockId = model.RockId;
                entity.Name = model.Name;
                entity.PrimaryElement = model.PrimaryElement;
                entity.SecondaryElement = model.SecondaryElement;
                entity.Description = model.Description;
                entity.Image = model.Image;
                entity.PlanetId = model.PlanetId;

                return ctx.SaveChanges() == 1;

            }
        }

        public bool DeleteRock(int rockId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Rocks
                    .Single(e => e.RockId == rockId && e.Planet.StarSystem.OwnerId == _userId);

                ctx.Rocks.Remove(entity);

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

