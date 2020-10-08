using Skybook.Data;
using Skybook.Models.Animal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Skybook.Services
{
    public class AnimalService
    {
        private readonly Guid _userId;
        public AnimalService(Guid userId)
        {
            _userId = userId;
        }


        public bool CreateAnimal(HttpPostedFileBase file, AnimalCreate model)
        {
            model.Image = ConvertToBytes(file);

            var entity =
                new Animal()
                {

                    Name = model.Name,
                    Description = model.Description,
                    PlanetId = model.PlanetId,
                    Image = model.Image
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Animals.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var q = from temp in db.Animals where temp.AnimalId == Id select temp.Image;
                byte[] cover = q.First();
                return cover;
            }
        }

        public IEnumerable<AnimalListItem> GetAnimals()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Animals
                    .Where(e => e.Planet.StarSystem.OwnerId == _userId)
                    .Select(
                        e =>
                        new AnimalListItem
                        {
                            AnimalId = e.AnimalId,
                            Name = e.Name,
                            Description = e.Description,
                            PlanetId = e.PlanetId
                            
                            
                        }
                        );
                return query.ToArray();
            }
        }

        public AnimalDetail GetAnimalById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Animals
                    .Single(e => e.AnimalId == id && e.Planet.StarSystem.OwnerId == _userId);
                return
                    new AnimalDetail
                    {
                        AnimalId = entity.AnimalId,
                        Name = entity.Name,
                        Description = entity.Description,
                        PlanetId = entity.PlanetId
                    };
            }
        }

        public bool UpdateAnimal(HttpPostedFileBase file, AnimalEdit model)
        {
            model.Image = ConvertToBytes(file);

            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Animals
                    .Single(e => e.AnimalId == model.AnimalId && e.Planet.StarSystem.OwnerId == _userId);

                entity.AnimalId = model.AnimalId;
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Image = model.Image;
                entity.PlanetId = model.PlanetId;

                return ctx.SaveChanges() == 1;

            }
        }

        public bool DeleteAnimal(int animalId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Animals
                    .Single(e => e.AnimalId == animalId && e.Planet.StarSystem.OwnerId == _userId);

                ctx.Animals.Remove(entity);

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
