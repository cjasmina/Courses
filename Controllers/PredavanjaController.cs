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
using System.Collections.Generic;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Edukator)]
    public class PredavanjaController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public PredavanjaController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            IPagedList<Predavanje> svaPredavanja = _databaseContext.Predavanja
                .Include(x => x.Kurs)
                .Include(x => x.Prisustva)
                .ToPagedList(page ?? Konfiguracija.InicijalnaStranica, Konfiguracija.BrojZapisaPoStranici);

            return View(svaPredavanja);
        }


        [HttpGet]
        public IActionResult Dodavanje()
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            PredavanjaDodavanjeIzmjenaViewModel viewModel = new PredavanjaDodavanjeIzmjenaViewModel
            {

                Predavanje = new PredavanjeViewModel
                {
                    Datum = DateTime.Now
                },
                Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList(),
            };

            return View("Forma", viewModel);
        }

        [HttpGet]
        public IActionResult Prisustvo(int id)
        {
            PredavanjaDodavanjeIzmjenaViewModel viewModel = new PredavanjaDodavanjeIzmjenaViewModel
            {
                Predavanje = new PredavanjeViewModel
                {
                    Id = id
                },
                Polaznici = _databaseContext.Prisustva
                                            .Include(x => x.Predavanje)
                                            .Include(x => x.KursKorisnik)
                                            .Where(x => x.PredavanjeId == id)
                                            .Select(x => new PredavanjaDodavanjeIzmjenaViewModel.OdabraniPolaznikViewModel
                                            {
                                                Ime = x.KursKorisnik.Korisnik.Ime,
                                                KursKorisnikId = x.KursKorisnikId,
                                                Prisutan = x.Prisutan,
                                                Prezime = x.KursKorisnik.Korisnik.Prezime,
                                                Napomena = x.Napomena
                                            })
                                            .ToList()
            };

            return View("Prisustvo", viewModel);
        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            var predavanje = _databaseContext.Predavanja.Include(x => x.Prisustva).SingleOrDefault(x => x.Id == id);

            if (predavanje == null)
            {
                return RedirectToAction("Index");
            }

            PredavanjaDodavanjeIzmjenaViewModel viewModel = new PredavanjaDodavanjeIzmjenaViewModel
            {
                Predavanje = new PredavanjeViewModel(predavanje),
                Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList()
            };

            return View("Forma", viewModel);
        }


        [HttpPost]
        public IActionResult SnimiPrisustvo(PredavanjaDodavanjeIzmjenaViewModel model)
        {
            var prethodnaPrisustva = _databaseContext.Prisustva.Where(x => x.PredavanjeId == model.Predavanje.Id).ToList();
            _databaseContext.Prisustva.RemoveRange(prethodnaPrisustva);
            _databaseContext.SaveChanges();

            _databaseContext.Prisustva.AddRange(model.Polaznici.Select(x => new Models.Prisustvo
            {
                PredavanjeId = model.Predavanje.Id,
                KursKorisnikId = x.KursKorisnikId,
                Napomena = x.Napomena,
                Prisutan = x.Prisutan
            }).ToList());
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste evidentirali prisutne");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Snimi(PredavanjaDodavanjeIzmjenaViewModel model)
        {
            var logiraniKorisnik = HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);

            if (!ModelState.IsValid)
            {
                model.Kursevi = _databaseContext.KursKorisnici.Where(x => x.KorisnikId == logiraniKorisnik.Id).Select(x => x.Kurs).ToList();

                return View("Forma", model);
            }

            Predavanje predavanje;

            if (model.Predavanje.Id != 0)
            {
                predavanje = _databaseContext.Predavanja.Include(x => x.Prisustva).ThenInclude(x => x.KursKorisnik).FirstOrDefault(x => x.Id == model.Predavanje.Id);
            }
            else
            {
                predavanje = new Predavanje
                {
                    DatumPredavanja = DateTime.Now
                };

                _databaseContext.Predavanja.Add(predavanje);
            }

            predavanje.Naziv = model.Predavanje.Naziv;
            predavanje.Opis = model.Predavanje.Opis;
            predavanje.DatumPredavanja = model.Predavanje.Datum;
            predavanje.KursId = model.Predavanje.KursId;

            _databaseContext.SaveChanges();

            if (model.Predavanje.Id == 0)
            {
                predavanje.Prisustva = _databaseContext.KursKorisnici.Where(x => x.KursId == model.Predavanje.KursId && x.Korisnik.Uloga == Uloga.Polaznik)
                                                              .Select(x => new Prisustvo
                                                              {
                                                                  PredavanjeId = predavanje.Id,
                                                                  KursKorisnikId = x.Id,
                                                                  Prisutan = false
                                                              })
                                                              .ToList();

                if(predavanje.Prisustva.Any())
                {
                    _databaseContext.SaveChanges();
                }

                _flashMessage.Confirmation("Uspješno ste dodali predavanje");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili predavanje");
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var predavanje = _databaseContext.Predavanja.Include(x => x.Prisustva).FirstOrDefault(x => x.Id == id);
            if (predavanje == null)
            {
                return RedirectToAction("Index");
            }

            _databaseContext.Prisustva.RemoveRange(predavanje.Prisustva);
            _databaseContext.Predavanja.Remove(predavanje);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali predavanje");

            return RedirectToAction("Index");

        }
    }
}
