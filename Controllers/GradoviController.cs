using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Courses.Models;
using Courses.Contexts;
using Courses.Helpers;
using Courses.ViewModels;
using Vereyon.Web;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator)]
    public class GradoviController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public GradoviController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        public IActionResult Index()
        {
            List<Grad> sviGradovi = _databaseContext.Gradovi.Include(x => x.Drzava).ToList();

            return View(sviGradovi);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            GradoviDodavanjeIzmjenaViewModel viewModel = new GradoviDodavanjeIzmjenaViewModel
            {
                Grad = new GradViewModel(),
                Drzave = _databaseContext.Drzave.ToList()
            };

            return View("Forma", viewModel);
        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var grad = _databaseContext.Gradovi.Find(id);
            if (grad == null)
            {
                return RedirectToAction("Index");
            }

            GradoviDodavanjeIzmjenaViewModel viewModel = new GradoviDodavanjeIzmjenaViewModel
            {
                Grad = new GradViewModel(grad),
                Drzave = _databaseContext.Drzave.ToList()
            };

            return View("Forma", viewModel);
        }

        [HttpPost]
        public IActionResult Snimi(GradoviDodavanjeIzmjenaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Forma", model);

            Grad grad;

            if (model.Grad.Id != 0)
            {
                grad = _databaseContext.Gradovi.Find(model.Grad.Id);
            }
            else
            {
                grad = new Grad();

                _databaseContext.Gradovi.Add(grad);
            }

            grad.Naziv = model.Grad.Naziv;
            grad.PostanskiBroj = model.Grad.PostanskiBroj;
            grad.DrzavaId = model.Grad.DrzavaId;

            _databaseContext.SaveChanges();

            if (model.Grad.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali grad");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili grad");
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var grad = _databaseContext.Gradovi.Find(id);
            if (grad == null)
            {
                return RedirectToAction("Index");
            }

            _databaseContext.Gradovi.Remove(grad);
            _databaseContext.SaveChanges();
            _flashMessage.Confirmation("Uspješno ste izbrisali grad");

            return RedirectToAction("Index");
        }
    }
}
