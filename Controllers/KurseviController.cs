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
    [Autentifikacija(Uloga.Administrator)]
    public class KurseviController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public KurseviController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            IPagedList<Kurs> sviKursevi = _databaseContext.Kursevi.Include(x => x.Oblast).ToPagedList(page ?? Konfiguracija.InicijalnaStranica, Konfiguracija.BrojZapisaPoStranici);

            return View(sviKursevi);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            var model = new KursDodajViewModel()
            {
                OblastStavke = _databaseContext.Oblasti.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Naziv.ToString() }).ToList(),
                DatumPocetka=DateTime.Now,
                DatumZavrsetka=DateTime.Now
            };
            return View("Forma", model);
        }

        [HttpPost]
        public IActionResult Snimi(KursDodajViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.OblastStavke = _databaseContext.Oblasti.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Naziv.ToString() }).ToList();

                return View("Forma", model);
            }

            Kurs k;
            if (model.Id == 0)
            {
                k = new Kurs();
                k.DatumPocetka = DateTime.Now;
                _databaseContext.Add(k);
            }
            else
            {
                k = _databaseContext.Kursevi.Find(model.Id);
            }


            if (model.Ikona != null)
            {
                var memoryStream = new MemoryStream();


                model.Ikona.CopyTo(memoryStream);
                var j = memoryStream.ToArray();
                k.Ikona = j;

            }


            k.Naziv = model.Naziv;
            k.Cijena = model.Cijena;
            k.OblastId = model.OblastId;
            k.Opis = model.Opis;
            k.DatumPocetka = model.DatumPocetka;
            k.DatumZavrsetka = model.DatumZavrsetka;
            k.BrojSati = model.BrojSati;


            _databaseContext.SaveChanges();

            if (model.Id == 0)
            {
                _flashMessage.Confirmation("Uspješno ste dodali kurs");
            }
            else
            {
                _flashMessage.Confirmation("Uspješno ste izmjenili kurs");
            }




            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            Kurs k = _databaseContext.Kursevi.Find(id);
            if (k == null)
                return RedirectToAction("Index");

            var model = new KursDodajViewModel()
            {
                Naziv = k.Naziv,
                Id = k.Id,
                OblastId = k.OblastId,
                OblastStavke = _databaseContext.Oblasti.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Naziv.ToString() }).ToList(),
                Opis = k.Opis,
                Cijena = k.Cijena,
                DatumPocetka = k.DatumPocetka,
                DatumZavrsetka = k.DatumZavrsetka,
                BrojSati = k.BrojSati



            };

            return View("Forma", model);
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var kurs = _databaseContext.Kursevi.Find(id);
            if (kurs == null)
            {
                return RedirectToAction("Index");
            }

            _databaseContext.Kursevi.Remove(kurs);
            _databaseContext.SaveChanges();

            _flashMessage.Confirmation("Uspješno ste izbrisali kurs");


            return RedirectToAction("Index");
        }
    }
}
