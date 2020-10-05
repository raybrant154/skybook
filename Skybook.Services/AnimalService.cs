using Skybook.Data;
using Skybook.Models.Animal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Services
{
    public class AnimalService
    {
        private readonly Guid _userId;
        public AnimalService(Guid userId)
        {
            _userId = userId;
        }


        public bool CreateAnimal(AnimalCreate model)
        {
            var entity =
                new Animal()
                {
                    
                    Name = model.Name,
                    Description = model.Description,
                    PlanetId = model.PlanetId
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Animals.Add(entity);
                return ctx.SaveChanges() == 1;
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

        public bool UpdateAnimal(AnimalEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Animals
                    .Single(e => e.AnimalId == model.AnimalId && e.Planet.StarSystem.OwnerId == _userId);

                entity.AnimalId = model.AnimalId;
                entity.Name = model.Name;
                entity.Description = model.Description;
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
    }
}
