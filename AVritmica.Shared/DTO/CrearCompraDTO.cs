using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class CrearCompraDTO
    {
        public DateTime FechaCompra { get; set; } = DateTime.UtcNow;

        public string Descripcion { get; set; } = string.Empty;
    }
}
