using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Courses.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Courses.Helpers
{
    public static class Ekstenzije
    {
        public static byte[] ToByteArray(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static bool ImaPristup(this HttpContext context, params Uloga[] uloge)
        {
            if (uloge == null || !uloge.Any())
                return false; 

            var logiraniKorisnik = context.Session.GetObject<Korisnik>(Konfiguracija.KljucLogiranogKorisnika);
            if (logiraniKorisnik == null)
                return false;

            return uloge.Contains(logiraniKorisnik.Uloga);
        }
    }
}
