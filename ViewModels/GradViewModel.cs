using System.ComponentModel.DataAnnotations;

using Courses.Models;

namespace Courses.ViewModels
{

    public class GradViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Poštanski broj je obavezan")]
        public string PostanskiBroj { get; set; }

        [Required(ErrorMessage = "Država je obavezna")]
        public int DrzavaId { get; set; }

        public GradViewModel() { }

        public GradViewModel(Grad model)
        {
            Id = model.Id;
            Naziv = model.Naziv;
            PostanskiBroj = model.PostanskiBroj;
            DrzavaId = model.DrzavaId;
        }
    }
}
