namespace ProyectoInmobiliariaADO.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Importe { get; set; }
        public string? MedioPago { get; set; }
        public string? Observaciones { get; set; }

        public string Estado { get; set; } = "Activo";

        // Propiedades para mostrar info legible del contrato
        public string? ContratoInmuebleDireccion { get; set; }
        public string? ContratoInmuebleTipo { get; set; }
        public string? ContratoInquilinoNombre { get; set; }
        public string? ContratoInquilinoApellido { get; set; }

        public string? UsuarioCreacionPago { get; set; }
        public DateTime? FechaCreacionPago { get; set; }   // <- ahora nullable
        public string? UsuarioAnulacionPago { get; set; }
        public DateTime? FechaAnulacionPago { get; set; }

        // Relación con contrato (opcional, si quieres usarlo en el ViewModel)
        public Contrato? Contrato { get; set; }
    }
}
