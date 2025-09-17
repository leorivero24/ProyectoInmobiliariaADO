namespace ProyectoInmobiliariaADO.Models
{
    public class Inmueble
    {
        public int Id { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Ambientes { get; set; }
        public decimal Superficie { get; set; }
        public decimal Precio { get; set; }
        public int PropietarioId { get; set; }
        public string Estado { get; set; } = "Disponible";
        public string? Observaciones { get; set; }

        public string? PropietarioNombre { get; set; } 
        public string? PropietarioApellido { get; set; }
    }
}
