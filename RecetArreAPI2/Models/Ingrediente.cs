using System.ComponentModel.DataAnnotations;

namespace RecetArreAPI2.Models
{
    public class Ingrediente
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = default!;

        [Required]
        public string? UnidadMedida { get; set; }
        
        [StringLength(50)]
        public string? Descripcion { get; set; }

        

    }
}
