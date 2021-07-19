using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ParkyDbContext _db;

        public TrailRepository(ParkyDbContext db)
        {
            _db = db;
        }

        public ICollection<Trail> GetTrails()
        {
            var trails = _db.Trails.Include(x => x.NationalPark)
                .OrderBy(x => x.Name).ToList();

            return trails;
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            var trails = _db.Trails.Include(x => x.NationalPark)
                .Where(x => x.NationalParkId == nationalParkId).ToList();

            return trails;
        }

        public Trail GetTrail(int id)
        {
            var trail = _db.Trails.Include(x => x.NationalPark)
                .FirstOrDefault(x => x.Id == id);

            return trail;
        }

        public bool TrailExists(string name)
        {
            var value = _db.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool TrailExists(int id)
        {
            var value = _db.Trails.Any(x => x.Id == id);

            return value;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);

            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);

            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);

            return Save();
        }

        public bool Save()
        {
            var value = _db.SaveChanges() >= 0 ? true : false;

            return value;
        }
    }
}