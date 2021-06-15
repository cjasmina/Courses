using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("TipoviObavijesti")]
    public class TipObavijesti : Bazni
    {
        [Required]
        public string Naziv { get; set; }
    }
}