using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.Shared.DTO
{
    public class ReporteUsuariosDTO
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int TotalCarritos { get; set; }
        public int CarritosActivos { get; set; }
        public int CarritosConfirmados { get; set; }
        public decimal TotalCompras { get; set; }
        public int ConsultasRealizadas { get; set; }
        public string Telefono { get; set; } = string.Empty;
    }

    public class ResumenUsuariosDTO
    {
        public int TotalUsuarios { get; set; }
        public int UsuariosClientes { get; set; }
        public int UsuariosAdministradores { get; set; }
        public int UsuariosVendedores { get; set; }
        public int TotalCarritos { get; set; }
        public int CarritosActivos { get; set; }
        public decimal TotalVentasUsuarios { get; set; }
        public List<UsuariosActivosDTO> UsuariosMasActivos { get; set; } = new List<UsuariosActivosDTO>();
    }

    public class UsuariosActivosDTO
    {
        public string Usuario { get; set; } = string.Empty;
        public int CarritosTotales { get; set; }
        public decimal TotalCompras { get; set; }
    }
}
