
namespace ProyectoInmobiliariaADO.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public int InmuebleId { get; set; }
        public int InquilinoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal Monto { get; set; }
        public string Periodicidad { get; set; } = "Mensual";
        public string Estado { get; set; } = "Vigente";
        public string? Observaciones { get; set; }

        // Propiedades para mostrar info legible del inmueble e inquilino
        public string? InmuebleDireccion { get; set; }
        public string? InmuebleTipo { get; set; }
        public string? InquilinoNombre { get; set; }
        public string? InquilinoApellido { get; set; }
        public string? InquilinoDNI { get; set; }

        // --- Auditoría ---
        public string? UsuarioCreacionContrato { get; set; }

        public DateTime? FechaCreacionContrato { get; set; }
        public string? UsuarioAnulacionContrato { get; set; }
        public DateTime? FechaAnulacionContrato { get; set; }
    }
}
