using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.ViewModels
{
    public class KursPrikaziVM
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public int BrojSati { get; set; }

        public int Cijena { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }

        public string Opis { get; set; }

        public string Oblast { get; set; }

        public string IkonicaString { get; set; }
        public byte[] Ikonica { get; set; }
    }
}
