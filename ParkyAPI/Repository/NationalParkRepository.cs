using System.Collections.Generic;
using System.Linq;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ParkyDbContext _db;

        public NationalParkRepository(ParkyDbContext db)
        {
            _db = db;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            var nationalParks = _db.NationalParks.OrderBy(x => x.Name).ToList();

            return nationalParks;
        }

        public NationalPark GetNationalPark(int id)
        {
            var nationalPark = _db.NationalParks.FirstOrDefault(x => x.Id == id);

            return nationalPark;
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());

            return value;
        }

        public bool NationalParkExists(int id)
        {
            bool value = _db.NationalParks.Any(x => x.Id == id);

            return value;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);

            return Save();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);

            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);

            return Save();
        }

        public bool Save()
        {
            bool value = _db.SaveChanges() >= 0 ? true : false;

            return value;
        }
    }
}