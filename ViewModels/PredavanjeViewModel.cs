using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

using Courses.Models;
using System;

namespace Courses.ViewModels
{
    public class PredavanjeViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Naziv je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezan")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Datum je obavezan")]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Kurs je obavezan")]
        public int KursId { get; set; }

        public PredavanjeViewModel()
        {
        }

        public PredavanjeViewModel(Predavanje model)
        {
            Id = model.Id;
            Naziv = model.Naziv;
            Opis = model.Opis;
            Datum = model.DatumPredavanja;
            KursId = model.KursId;
        }
    }
}

