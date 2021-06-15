using Microsoft.AspNetCore.Mvc;

using Courses.Models;
using Courses.Helpers;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Polaznik, Uloga.Edukator, Uloga.Administrator)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
