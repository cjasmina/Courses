using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using Courses.Models;

namespace Courses.ViewModels
{
    public class ObavijestViewModel
    {
        public int Id { get; set; }

        public IFormFile NaslovnaFotografija { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezan")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Kratak opis je obavezan")]
        public string KratakOpis { get; set; }

        [Required(ErrorMessage = "Tip obavijesti je obavezan")]
        public int TipObavijestiId { get; set; }
        public int? KursId { get; set; }
        public Kurs Kurs { get; set; }

        public IFormFile Dodatak { get; set; }

        public ObavijestViewModel()
        {
        }

        public ObavijestViewModel(Obavijest model)
        {
            Id = model.Id;
            Naziv = model.Naziv;
            Opis = model.Opis;
            KratakOpis = model.KratakOpis;
            TipObavijestiId = model.TipObavijestiId;
            KursId = model.KursId;
            Kurs = model.Kurs;
        }
    }
}
