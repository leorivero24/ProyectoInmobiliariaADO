using ProyectoInmobiliariaADO.Models;
using System.Collections.Generic;

namespace ProyectoInmobiliariaADO.Models.ViewModels
{
    public class ContratoDetailsViewModel
    {
        public required Contrato Contrato { get; set; }
        public required Inmueble Inmueble { get; set; }
        public required Inquilino Inquilino { get; set; }
        public List<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
