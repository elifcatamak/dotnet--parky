using System.IO;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                // Image related
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0) // If image is uploaded
                {
                    byte[] p1 = null;

                    using (var fileStream1 = files[0].OpenReadStream()) // This will read the file
                    {
                        using (var memoryStream1 = new MemoryStream())
                        {
                            fileStream1.CopyTo(memoryStream1);
                            p1 = memoryStream1.ToArray();
                        }
                    }

                    nationalPark.Image = p1;
                }
                else // If image is not uploaded
                {
                    if (nationalPark.Id != 0)
                    {
                        var dataFromDb =
                            await _npRepository.GetAsync(StaticDetails.NationalParkAPIPath, nationalPark.Id);
                        nationalPark.Image = dataFromDb.Image;
                    }
                }

                if (nationalPark.Id == 0) // Create
                {
                    await _npRepository.CreateAsync(StaticDetails.NationalParkAPIPath, nationalPark);
                }
                else // Update
                {
                    await _npRepository.UpdateAsync(StaticDetails.NationalParkAPIPath + nationalPark.Id, nationalPark);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }
    }
}