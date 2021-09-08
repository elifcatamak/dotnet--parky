using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _npRepository;
        private readonly ITrailRepository _trailRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(INationalParkRepository npRepository, ITrailRepository trailRepository,
                              ILogger<HomeController> logger)
        {
            _npRepository = npRepository;
            _trailRepository = trailRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var indexVM = new IndexVM()
            {
                NationalParks = await _npRepository.GetAllAsync(StaticDetails.NationalParkAPIPath),
                Trails = await _trailRepository.GetAllAsync(StaticDetails.TrailAPIPath)
            };

            return View(indexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}