using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator)]
    public class TipoviObavijestiController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public TipoviObavijestiController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sveTipoviObavijesti = _databaseContext.TipoviObavijesti.ToList();

            return View(sveTipoviObavijesti);
        }

        [HttpGet]
        public IActionResult Dodavanje()
        {
            return View("Forma", new TipObavijesti());
        }

        [HttpGet]
        public IActionResult Izmjena(int id)
        {
            var tipObavijesti = _databaseContext.TipoviObavijesti.Find(id);

            return View("Forma", tipObavijesti);
        }

        [HttpPost]
        public IActionResult Snimi(TipObavijesti model)
        {
            TipObavijesti tipObavijesti;

            if (model.Id != 0)
            {
                tipObavijesti = _databaseContext.TipoviObavijesti.Find(model.Id);
            }
            else
            {
                tipObavijesti = new TipObavijesti();

                _databaseContext.TipoviObavijesti.Add(tipObavijesti);
            }

            tipObavijesti.Naziv = model.Naziv;

            _databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Izbrisi(int id)
        {
            var tipObavijesti = _databaseContext.TipoviObavijesti.Find(id);

            _databaseContext.TipoviObavijesti.Remove(tipObavijesti);
            _databaseContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
