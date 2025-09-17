// namespace ProyectoInmobiliariaADO.Models
// {
//     public class Contrato
//     {
//         public int Id { get; set; }
//         public int InmuebleId { get; set; }
//         public int InquilinoId { get; set; }
//         public DateTime FechaInicio { get; set; }
//         public DateTime? FechaFin { get; set; }
//         public decimal Monto { get; set; }
//         public string Periodicidad { get; set; } = "Mensual";
//         public string Estado { get; set; } = "Vigente";
//         public string? Observaciones { get; set; }

//         public string? InmuebleDireccion { get; set; }
// public string? InquilinoNombre { get; set; }

//     }
// }


using System;

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
        public string Periodicidad { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public string? Observaciones { get; set; }

        // 🔹 Propiedades extra (no están en la tabla contrato, pero se cargan en joins)
        public string? InmuebleDireccion { get; set; }
        public string? InmuebleTipo { get; set; }

        public string? InquilinoNombre { get; set; }
        public string? InquilinoApellido { get; set; }
        public string? InquilinoDNI { get; set; }

        // 🔹 Propiedad calculada (para mostrar el inquilino completo)
        public string NombreCompletoInquilino
        {
            get => $"{InquilinoNombre} {InquilinoApellido} - DNI: {InquilinoDNI}";
        }

        // 🔹 Propiedad calculada (para mostrar el inmueble con dirección y tipo)
        public string InmuebleResumen
        {
            get => $"{InmuebleDireccion} ({InmuebleTipo})";
        }
    }
}
