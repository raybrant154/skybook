using Skybook.Data;
using Skybook.Models;
using Skybook.Models.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybook.Services
{
    public class StarSystemService
    {
        private readonly Guid _userId;
        public StarSystemService(Guid userId)
        {
            _userId = userId;
        }
        public bool CreateStarSystem(StarSystemCreate model)
        {
            var entity =
                new StarSystem()
                {
                    OwnerId = _userId,
                    Name = model.Name,
                    Race = model.Race,
                    Economy = model.Economy,
                    Conflict = model.Conflict
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.StarSystems.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public IEnumerable<StarSystemListItem> GetStarSystems()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .StarSystems
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new StarSystemListItem
                        {
                            StarSystemId = e.StarSystemId,
                            Name = e.Name,
                            Race = e.Race,
                            Economy = e.Economy,
                            Conflict = e.Conflict,
                            
                            
                        }
                        );
                return query.ToArray();

            }
        }

        public StarSystemDetail GetStarSystemById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .StarSystems
                        .Single(e => e.StarSystemId == id && e.OwnerId == _userId);
                return
                    new StarSystemDetail
                    {
                        StarSystemId = entity.StarSystemId,
                        Name = entity.Name,
                        Race = entity.Race,
                        Economy = entity.Economy,
                        Conflict = entity.Conflict,
                        
                        
                        Planets = ctx.Planets.Where(e => e.StarSystemId == id).Select(e => new PlanetListItem {
                            PlanetId = e.PlanetId,
                            Name = e.Name,
                            PlanetType = e.PlanetType,
                            Minerals = e.Minerals,
                            SpecialBuried = e.SpecialBuried,
                            SentinelActivity = e.SentinelActivity,

                        }).ToList(),
                    };
            }
        }

        public bool UpdateStarSystem(StarSystemEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .StarSystems
                        .Single(e => e.StarSystemId == model.StarSystemId && e.OwnerId == _userId);

                entity.StarSystemId = model.StarSystemId;
                entity.Name = model.Name;
                entity.Race = model.Race;
                entity.Economy = model.Economy;
                entity.Conflict = model.Conflict;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteStarSystem(int starSystemId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .StarSystems
                        .Single(e => e.StarSystemId == starSystemId && e.OwnerId == _userId);

                ctx.StarSystems.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }

}
