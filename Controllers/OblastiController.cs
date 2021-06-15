using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;
using Vereyon.Web;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator)]
    public class OblastiController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public OblastiController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var podaci = _databaseContext.Oblasti.ToList();
            return View(podaci);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            return View("Forma", new Oblast());
        }

        [HttpPost]
        public IActionResult Snimi(Oblast model)
        {
            Oblast oblast;

            if (model.Id != 0)
            {
                oblast = _databaseContext.Oblasti.Find(model.Id);
            }
            else
            {
                oblast = new Oblast();
                _databaseContext.Oblasti.Add(oblast);
            }

            oblast.Naziv = model.Naziv;

            _databaseContext.SaveChanges();
            if (model.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali oblast");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili oblast");
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var oblast = _databaseContext.Oblasti.Find(id);

            return View("Forma", oblast);
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var oblast = _databaseContext.Oblasti.Find(id);
            if (oblast == null)
            {
                return RedirectToAction("Index");
            }

            _databaseContext.Oblasti.Remove(oblast);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali oblast");


            return RedirectToAction("Index");
        }
    }
}
