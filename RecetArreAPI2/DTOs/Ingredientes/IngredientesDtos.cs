namespace RecetArreAPI2.DTOs.Ingredientes
{
    public class IngredienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string UnidadMedida { get; set; } = default!;
        public string? Descripcion { get; set; }
    }

    public class IngredienteCreacionDto
    {
        public string Nombre { get; set; } = default!;
        public string UnidadMedida { get; set; } = default!;
        public string? Descripcion { get; set; }
    }

    public class IngredienteModificacionDto
    {
        public string Nombre { get; set; } = default!;
        public string UnidadMedida { get; set; } = default!;
        public string? Descripcion { get; set; }
    }

}
