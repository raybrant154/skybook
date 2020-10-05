using Skybook.Data;
using Skybook.Models.Rock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Services
{
    public class RockService
    {
        private readonly Guid _userId;
        public RockService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateRock(RockCreate model)
        {
            var entity =
                new Rock()
                {
                    Name = model.Name,
                    PrimaryElement = model.PrimaryElement,
                    SecondaryElement = model.SecondaryElement,
                    Description = model.Description,
                    PlanetId = model.PlanetId
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Rocks.Add(entity);
                return ctx.SaveChanges() == 1;
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

        public bool UpdateRock(RockEdit model)
        {
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
    }
}

