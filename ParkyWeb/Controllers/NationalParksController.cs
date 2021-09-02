using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepository;

        public NationalParksController(INationalParkRepository npRepository)
        {
            _npRepository = npRepository;
        }

        // GET
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            var nationalParks = await _npRepository.GetAllAsync(StaticDetails.NationalParkAPIPath);

            return Json(new {data = nationalParks});
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalPark = new NationalPark();

            // Insert
            if (id == null)
            {
                return View(nationalPark);
            }

            // Update
            nationalPark = await _npRepository.GetAsync(StaticDetails.NationalParkAPIPath, id.GetValueOrDefault());

            if (nationalPark == null)
            {
                return NotFound();
            }

            return View(nationalPark);
        }
    }
}