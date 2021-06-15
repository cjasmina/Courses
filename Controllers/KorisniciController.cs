using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using X.PagedList;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;
using Courses.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using Vereyon.Web;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
    public class KorisniciController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public KorisniciController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [Autentifikacija(Uloga.Administrator)]
        public IActionResult Edukatori(int? page)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            IPagedList<Korisnik> edukatori = _databaseContext.Korisnici
                .Where(x => x.Id != logiraniKorisnik.Id && x.Uloga == Uloga.Edukator)
                .Include(x => x.Grad)
                .ToPagedList(page ?? Konfiguracija.InicijalnaStranica, Konfiguracija.BrojZapisaPoStranici);


            return View(edukatori);
        }


        [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
        public IActionResult Polaznici(int? page)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            IPagedList<Korisnik> polaznici = _databaseContext.Korisnici
                .Where(x => x.Id != logiraniKorisnik.Id && x.Uloga == Uloga.Polaznik)
                .Include(x => x.Grad)
                .ToPagedList(page ?? Konfiguracija.InicijalnaStranica, Konfiguracija.BrojZapisaPoStranici);


            return View(polaznici);
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator)]
        public IActionResult DodavanjeEdukatora()
        {
            KorisniciDodavanjeIzmjenaViewModel viewModel = new KorisniciDodavanjeIzmjenaViewModel
            {
                Korisnik = new KorisnikViewModel(),
                Gradovi = _databaseContext.Gradovi.ToList(),
                Kursevi = _databaseContext.Kursevi.Select(x => new KorisniciDodavanjeIzmjenaViewModel.OdabraniKursViewModel {
                    KursId = x.Id,
                    Naziv = x.Naziv,
                    Odabran = false
                }).ToList()
            };
            
            return View("FormaEdukator", viewModel);
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
        public IActionResult DodavanjePolaznika()
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            KorisniciDodavanjeIzmjenaViewModel viewModel = new KorisniciDodavanjeIzmjenaViewModel
            {
                Korisnik = new KorisnikViewModel(),
                Gradovi = _databaseContext.Gradovi.ToList(),
                Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => new KorisniciDodavanjeIzmjenaViewModel.OdabraniKursViewModel
                {
                    KursId = x.KursId,
                    Naziv = x.Kurs.Naziv,
                    Odabran = false
                }).ToList()
            };

            return View("FormaPolaznik", viewModel);
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator)]
        public IActionResult IzmjenaEdukatora(int id)
        {
            var korisnik = _databaseContext.Korisnici.Include(x => x.Kursevi).SingleOrDefault(x => x.Id == id);
            if (korisnik == null)
            {
                return RedirectToAction("Edukatori");
            }

            KorisniciDodavanjeIzmjenaViewModel viewModel = new KorisniciDodavanjeIzmjenaViewModel
            {
                Korisnik = new KorisnikViewModel(korisnik),
                Gradovi = _databaseContext.Gradovi.ToList(),
                Kursevi = _databaseContext.Kursevi.Select(x => new KorisniciDodavanjeIzmjenaViewModel.OdabraniKursViewModel
                {
                    KursId = x.Id,
                    Naziv = x.Naziv,
                    Odabran = _databaseContext.KursKorisnici.Any(y => y.KorisnikId == korisnik.Id && y.KursId == x.Id)
                }).ToList()
            };

            return View("FormaEdukator", viewModel);
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
        public IActionResult IzmjenaPolaznika(int id)
        {
            var korisnik = _databaseContext.Korisnici.Include(x => x.Kursevi).SingleOrDefault(x => x.Id == id);
            if (korisnik == null)
            {
                return RedirectToAction("Polaznici");
            }

            KorisniciDodavanjeIzmjenaViewModel viewModel = new KorisniciDodavanjeIzmjenaViewModel
            {
                Korisnik = new KorisnikViewModel(korisnik),
                Gradovi = _databaseContext.Gradovi.ToList(),
                Kursevi = _databaseContext.Kursevi.Select(x => new KorisniciDodavanjeIzmjenaViewModel.OdabraniKursViewModel
                {
                    KursId = x.Id,
                    Naziv = x.Naziv,
                    Odabran = _databaseContext.KursKorisnici.Any(y => y.KorisnikId == korisnik.Id && y.KursId == x.Id)
                }).ToList()
            };

            return View("FormaPolaznik", viewModel);
        }


        [HttpPost]
        [Autentifikacija(Uloga.Administrator)]
        public IActionResult SnimiEdukatora(KorisniciDodavanjeIzmjenaViewModel model)
        {
          
            if (!ModelState.IsValid)
            {
                model.Gradovi = _databaseContext.Gradovi.ToList();

                return View("FormaEdukator", model);
            }

            Korisnik korisnik;

            if (model.Korisnik.Id != 0)
            {
                korisnik = _databaseContext.Korisnici.Include(x => x.Kursevi).ThenInclude(x => x.Kurs).FirstOrDefault(x => x.Id == model.Korisnik.Id);
            }
            else 
            {
               
                if (string.IsNullOrEmpty(model.Korisnik.Lozinka))
                {
                    ModelState.AddModelError(string.Empty, "Lozinka je obavezna");

                    model.Gradovi = _databaseContext.Gradovi.ToList();

                    return View("FormaEdukator", model);
                }

                korisnik = new Korisnik
                {
                    DatumRegistracije = DateTime.Now
                };

                _databaseContext.Korisnici.Add(korisnik);
            }

            if (model.Korisnik.ProfilnaFotografija != null)
            {
                korisnik.ProfilnaFotografija = model.Korisnik.ProfilnaFotografija.ToByteArray();
            }

            korisnik.Ime = model.Korisnik.Ime;
            korisnik.Prezime = model.Korisnik.Prezime;
            korisnik.Email = model.Korisnik.Email;
            korisnik.KorisnickoIme = model.Korisnik.KorisnickoIme;

            if (!string.IsNullOrEmpty(model.Korisnik.Lozinka))
            {
                korisnik.LozinkaSalt = Guid.NewGuid().ToString();
                korisnik.LozinkaHash = Kriptografija.Hashiraj(model.Korisnik.Lozinka, korisnik.LozinkaSalt);
            }

            korisnik.Uloga = Uloga.Edukator;
            korisnik.GradId = model.Korisnik.GradId;
            
            korisnik.Kursevi = model.Kursevi.Where(x => x.Odabran).Select(x => new KursKorisnik {
                KursId = x.KursId,
                KorisnikId = korisnik.Id,
            }).ToList();

            _databaseContext.SaveChanges();

            if(model.Korisnik.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali edukatora");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili edukatora");
            }

            return RedirectToAction("Edukatori");
        }
             

        [HttpPost]
        [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
        public IActionResult SnimiPolaznika(KorisniciDodavanjeIzmjenaViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.Gradovi = _databaseContext.Gradovi.ToList();

                return View("FormaPolaznik", model);
            }

            Korisnik korisnik;

            if (model.Korisnik.Id != 0)
            {
                korisnik = _databaseContext.Korisnici.Include(x => x.Kursevi).ThenInclude(x => x.Kurs).FirstOrDefault(x => x.Id == model.Korisnik.Id);
            }
            else
            {
                if (string.IsNullOrEmpty(model.Korisnik.Lozinka))
                {
                    ModelState.AddModelError(string.Empty, "Lozinka je obavezna");

                    model.Gradovi = _databaseContext.Gradovi.ToList();

                    return View("FormaPolaznik", model);
                }

                korisnik = new Korisnik
                {
                    DatumRegistracije = DateTime.Now
                };

                _databaseContext.Korisnici.Add(korisnik);
            }

            if (model.Korisnik.ProfilnaFotografija != null)
            {
                korisnik.ProfilnaFotografija = model.Korisnik.ProfilnaFotografija.ToByteArray();
            }

            korisnik.Ime = model.Korisnik.Ime;
            korisnik.Prezime = model.Korisnik.Prezime;
            korisnik.Email = model.Korisnik.Email;
            korisnik.KorisnickoIme = model.Korisnik.KorisnickoIme;
            if (!string.IsNullOrEmpty(model.Korisnik.Lozinka))
            {
                korisnik.LozinkaSalt = Guid.NewGuid().ToString();
                korisnik.LozinkaHash = Kriptografija.Hashiraj(model.Korisnik.Lozinka, korisnik.LozinkaSalt);
            }
            korisnik.Uloga = Uloga.Polaznik;
            korisnik.GradId = model.Korisnik.GradId;

            korisnik.Kursevi = model.Kursevi.Where(x => x.Odabran).Select(x => new KursKorisnik
            {
                KursId = x.KursId,
                KorisnikId = korisnik.Id,
            }).ToList();

            _databaseContext.SaveChanges();

            if (model.Korisnik.Id == 0)
            {
                MailSender.Send(model.Korisnik.Email, "Dobrodošli na Courses", $"Poštovani,<br/><br/>Vaš račun je uspješno kreiran na <strong>Courses</strong> aplikaciji. Pristupni podaci su:<br/>Korisničko ime: <b>{model.Korisnik.KorisnickoIme}</b><br>Lozinka: <b>{model.Korisnik.Lozinka}</b><br/><br>Želimo Vam ugodno korištenje i puno sreće u učenju,<br/>Vaš Courses tim");

                _flashMessage.Confirmation("Uspješno ste dodali polaznika");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili polaznika");
            }

            return RedirectToAction("Polaznici");
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator)]
        public IActionResult IzbrisiEdukatora(int id)
        {
            var edukator = _databaseContext.Korisnici.Find(id);
            if (edukator == null)
            {
                return RedirectToAction("Edukatori");
            }

            _databaseContext.Korisnici.Remove(edukator);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali edukatora");

            return RedirectToAction("Edukatori");
        }


        [HttpGet]
        [Autentifikacija(Uloga.Administrator, Uloga.Edukator)]
        public IActionResult IzbrisiPolaznika(int id)
        {
            var polaznik = _databaseContext.Korisnici.Find(id);
            if (polaznik == null)
            {
                return RedirectToAction("Polaznici");
            }

            _databaseContext.Korisnici.Remove(polaznik);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali polaznika");

            return RedirectToAction("Polaznici");
        }
    }
}
