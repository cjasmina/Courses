using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Courses.Models;

namespace Courses.Helpers
{
    public class Autentifikacija : TypeFilterAttribute
    {
        public Autentifikacija(params Uloga[] uloge) : base(typeof(AsyncActionFilter))
        {
            Arguments = new object[]
            {
                uloge
            };
        }
    }

    public class AsyncActionFilter : IAsyncActionFilter
    {
        private Uloga[] Uloge { get; }

        public AsyncActionFilter(Uloga[] uloge)
        {
            Uloge = uloge;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context != null)
            {
                var logiraniKorisnik = context.HttpContext.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);
                if (logiraniKorisnik == null)
                {
                    context.Result = new RedirectToActionResult("Prijava", "Pristup", null);
                    return;
                }

                if (Uloge.Contains(logiraniKorisnik.Uloga) && next != null)
                {
                    await next();
                    return;
                }
                
                context.Result = new RedirectToActionResult("Index", "Greska", new { greska = 401 });
            }
        }
    }
}
