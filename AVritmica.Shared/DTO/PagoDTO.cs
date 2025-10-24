using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class PagoDTO
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.UtcNow;
        public string MetodoPago { get; set; } = string.Empty;
        public decimal MontoPagado { get; set; }
        public string EstadoPago { get; set; } = "Completado";
        public decimal Saldo { get; set; }
    }
}