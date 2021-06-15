using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;
using Courses.ViewModels;

using X.PagedList;
using Vereyon.Web;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
    public class ObavijestiController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public ObavijestiController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            IPagedList<Obavijest> sveObavijesti = _databaseContext.Obavijesti.Include(x => x.TipObavijesti).Include(x => x.Kurs).ToPagedList(page ?? Konfiguracija.InicijalnaStranica, Konfiguracija.BrojZapisaPoStranici);

            return View(sveObavijesti);
        }

        [HttpGet]
        public IActionResult Dodatak(int id)
        {
            var obavijest = _databaseContext.Obavijesti.Find(id);
            if (obavijest == null || obavijest.Dodatak == null)
            {
                return RedirectToAction("Index");
            }

            return File(obavijest.Dodatak, "application/octet-stream", obavijest.DodatakNaziv);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            ObavijestiDodavanjeIzmjenaViewModel viewModel = new ObavijestiDodavanjeIzmjenaViewModel
            {
                Obavijest = new ObavijestViewModel(),
                TipoviObavijesti = _databaseContext.TipoviObavijesti.ToList(),
                Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList()
            };

            return View("Forma", viewModel);
        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);
            var obavijest = _databaseContext.Obavijesti.Include(x => x.Kurs).FirstOrDefault(x => x.Id == id);
            if (obavijest == null)
            {
                return RedirectToAction("Index");
            }

            ObavijestiDodavanjeIzmjenaViewModel viewModel = new ObavijestiDodavanjeIzmjenaViewModel
            {
                Obavijest = new ObavijestViewModel(obavijest),
                TipoviObavijesti = _databaseContext.TipoviObavijesti.ToList(),
                Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList()

            };

            return View("Forma", viewModel);
        }

        [HttpPost]
        public IActionResult Snimi(ObavijestiDodavanjeIzmjenaViewModel model)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            if (model.Obavijest.Id == 0 && model.Obavijest.NaslovnaFotografija == null)
                ModelState.AddModelError(nameof(model.Obavijest.NaslovnaFotografija), "Naslovna fotografija je obavezna");

            if (!ModelState.IsValid)
            {
                model.TipoviObavijesti = _databaseContext.TipoviObavijesti.ToList();

                return View("FormaEdukator", model);
            }

            if (!ModelState.IsValid)
            {
                model.Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList();

                return View("FormaEdukator", model);
            }

            Obavijest obavijest;

            if (model.Obavijest.Id != 0)
            {
                obavijest = _databaseContext.Obavijesti.Find(model.Obavijest.Id);

                obavijest.DatumAzuriranja = DateTime.Now;
            }
            else
            {
                obavijest = new Obavijest
                {
                    DatumObjave = DateTime.Now,
                    DatumAzuriranja = DateTime.Now
                };

                _databaseContext.Obavijesti.Add(obavijest);
            }

            if (model.Obavijest.NaslovnaFotografija != null)
            {
                obavijest.NaslovnaFotografija = model.Obavijest.NaslovnaFotografija.ToByteArray();
            }

            if (model.Obavijest.Dodatak != null)
            {
                obavijest.DodatakNaziv = model.Obavijest.Dodatak.FileName;
                obavijest.Dodatak = model.Obavijest.Dodatak.ToByteArray();
            }

            obavijest.Naziv = model.Obavijest.Naziv;
            obavijest.KratakOpis = model.Obavijest.KratakOpis;
            obavijest.Opis = model.Obavijest.Opis;
            obavijest.TipObavijestiId = model.Obavijest.TipObavijestiId;
            obavijest.KursId = model.Obavijest.KursId;

            _databaseContext.SaveChanges();

            if (model.Obavijest.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali edukatora");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili edukatora");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var obavijest = _databaseContext.Obavijesti.Find(id);
            if (obavijest == null)
            {
                return RedirectToAction("Index");
            }

            _databaseContext.Obavijesti.Remove(obavijest);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali obavijest");


            return RedirectToAction("Index");
        }
    }
}