using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;
using Vereyon.Web;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator)]
    public class DrzaveController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public DrzaveController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var sveDrzave = _databaseContext.Drzave.ToList();

            return View(sveDrzave);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            return View("Forma", new Drzava());
        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var drzava = _databaseContext.Drzave.Find(id);

            return View("Forma", drzava);
        }

        [HttpPost]
        public IActionResult Snimi(Drzava model)
        {
            if (!ModelState.IsValid)
                return View("Forma", model);

            Drzava drzava;

            if (model.Id != 0)
            {
                drzava = _databaseContext.Drzave.Find(model.Id);
            }
            else
            {
                drzava = new Drzava();

                _databaseContext.Drzave.Add(drzava);
            }

            drzava.Naziv = model.Naziv;

            _databaseContext.SaveChanges();

            if (model.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali državu");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili državu");
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var drzava = _databaseContext.Drzave.Find(id);

            _databaseContext.Drzave.Remove(drzava);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali državu");


            return RedirectToAction("Index");
        }
    }
}
