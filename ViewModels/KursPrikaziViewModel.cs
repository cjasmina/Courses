using System;

namespace Courses.ViewModels
{
    public class KursPrikaziViewModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int BrojSati { get; set; }

        public int Cijena { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }

        public string Opis { get; set; }
        public string Oblast { get; set; }
        public string IkonaString { get; set; }
        public byte[] Ikona { get; set; }
    }
}
