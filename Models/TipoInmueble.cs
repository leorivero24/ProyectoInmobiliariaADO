
using System;

namespace ProyectoInmobiliariaADO.Models
{
    public class TipoInmueble
    {
        public int Id { get; set; }

        // <<< Esta propiedad debe existir para las vistas >>>
        public string Descripcion { get; set; } 

        public string Estado { get; set; } = "Activo";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}

