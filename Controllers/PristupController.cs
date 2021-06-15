using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Courses.Helpers;
using Courses.Contexts;
using Courses.Models;
using Courses.ViewModels;
using Vereyon.Web;

namespace Courses.Controllers
{
    public class PristupController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public PristupController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }



        [HttpGet]
        public IActionResult Prijava()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Prijava(PristupPrijavaViewModel model)
        {
            var korisnik = _databaseContext.Korisnici.FirstOrDefault(k => k.KorisnickoIme == model.KorisnickoIme);

            if (korisnik == null || !Kriptografija.Validiraj(model.Lozinka, korisnik.LozinkaSalt, korisnik.LozinkaHash))
            {
                ModelState.AddModelError(string.Empty, "Korisničko ime i/ili kozinka nisu pronađeni.");

                return View(model);
            }

            korisnik.DatumPosljednjePrijave = DateTime.Now;
            _databaseContext.Korisnici.Update(korisnik);
            _databaseContext.SaveChanges();

            HttpContext.Session.SetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika, korisnik);


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profil()
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            KorisniciDodavanjeIzmjenaViewModel viewModel = new KorisniciDodavanjeIzmjenaViewModel
            {
                Korisnik = new KorisnikViewModel(logiraniKorisnik),
                Gradovi = _databaseContext.Gradovi.ToList()
            };

            return View("Profil", viewModel);
        }

        [HttpPost]
        public IActionResult Snimi(KorisniciDodavanjeIzmjenaViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Gradovi = _databaseContext.Gradovi.ToList();

                return View("Profil", model);

            }

            Korisnik korisnik;

            korisnik = _databaseContext.Korisnici.Find(model.Korisnik.Id);

            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            if (model.Korisnik.ProfilnaFotografija != null)
            {
                korisnik.ProfilnaFotografija = model.Korisnik.ProfilnaFotografija.ToByteArray();

                logiraniKorisnik.ProfilnaFotografija = korisnik.ProfilnaFotografija;
            }

            korisnik.Ime = model.Korisnik.Ime;
            logiraniKorisnik.Ime = model.Korisnik.Ime;

            korisnik.Prezime = model.Korisnik.Prezime;
            logiraniKorisnik.Prezime = model.Korisnik.Prezime;

            korisnik.Email = model.Korisnik.Email;
            logiraniKorisnik.Email = model.Korisnik.Email;

            korisnik.KorisnickoIme = model.Korisnik.KorisnickoIme;
            logiraniKorisnik.KorisnickoIme = model.Korisnik.KorisnickoIme;

            korisnik.LozinkaSalt = Guid.NewGuid().ToString();
            korisnik.LozinkaHash = Kriptografija.Hashiraj(model.Korisnik.Lozinka, korisnik.LozinkaSalt);

            korisnik.GradId = model.Korisnik.GradId;
            logiraniKorisnik.GradId = model.Korisnik.GradId;

            HttpContext.Session.SetObject(Konfiguracija.KljucLogiranogKorisnika, logiraniKorisnik);

            _databaseContext.SaveChanges();
            _flashMessage.Confirmation("Uspješno ste izmjenili svoj profil");

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult Odjava()
        {
            HttpContext.Session.SetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika, null);

            return RedirectToAction("Prijava");
        }
    }
}
