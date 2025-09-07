using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    public class Categoria : EntityBase
    {
        public string Nombre { get; set; } 
        public string Descripcion { get; set; }
    }
}
