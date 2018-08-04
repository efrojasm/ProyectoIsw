using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventos.Models
{
    public class EventoData
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Cupo { get; set; }
        public string Lugar { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }
        public string UserName { get; set; }
    }
}
