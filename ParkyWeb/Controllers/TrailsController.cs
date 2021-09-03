using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepository;
        private readonly ITrailRepository _trailRepository;

        public TrailsController(INationalParkRepository npRepository, ITrailRepository trailRepository)
        {
            _npRepository = npRepository;
            _trailRepository = trailRepository;
        }

        // GET
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> GetAllTrails()
        {
            var trails = await _trailRepository.GetAllAsync(StaticDetails.TrailAPIPath);

            return Json(new {data = trails});
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParkList = await _npRepository.GetAllAsync(StaticDetails.NationalParkAPIPath);

            var trailsVM = new TrailsVM()
            {
                NationalParkList = nationalParkList.Select(i => new SelectListItem // For dropdown
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            // Insert
            if (id == null)
            {
                return View(trailsVM);
            }

            // Update
            trailsVM.Trail = await _trailRepository.GetAsync(StaticDetails.TrailAPIPath, id.GetValueOrDefault());

            if (trailsVM.Trail == null)
            {
                return NotFound();
            }

            return View(trailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM trailsVm)
        {
            if (ModelState.IsValid)
            {
                if (trailsVm.Trail.Id == 0) // Create
                {
                    await _trailRepository.CreateAsync(StaticDetails.TrailAPIPath, trailsVm.Trail);
                }
                else // Update
                {
                    await _trailRepository.UpdateAsync(StaticDetails.TrailAPIPath + trailsVm.Trail.Id, trailsVm.Trail);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(trailsVm);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _trailRepository.DeleteAsync(StaticDetails.TrailAPIPath, id);

            if (success)
            {
                return Json(new {success = true, message = "Delete successful"});
            }

            return Json(new {success = false, message = "Delete is not successful"});
        }
    }
}