using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using Courses.Models;

namespace Courses.ViewModels
{
    public class KorisnikViewModel
    {
        public int Id { get; set; }

        public IFormFile ProfilnaFotografija { get; set; }

        [Required(ErrorMessage = "Ime je obavezno")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Email je obavezan")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        public string KorisnickoIme { get; set; }

        public string Lozinka { get; set; }

        [Required(ErrorMessage = "Grad je obavezan")]
        public int GradId { get; set; }
        public int KursId { get; set; }


        public KorisnikViewModel()
        {
        }

        public KorisnikViewModel(Korisnik model)
        {
            Id = model.Id;
            Ime = model.Ime;
            Prezime = model.Prezime;
            Email = model.Email;
            KorisnickoIme = model.KorisnickoIme;
            GradId = model.GradId;
        }
    }
}

