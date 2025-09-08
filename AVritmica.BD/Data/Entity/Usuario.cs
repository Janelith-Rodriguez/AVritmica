using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(Email), Name = "Usuario_UQ", IsUnique = true)]
    public class Usuario : EntityBase
    {
        [Required(ErrorMessage = "El nombre del usuario es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre tiene como máximo{1} caracteres.")]        
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido del usuario es obligatorio")]
        [MaxLength(100, ErrorMessage = "El Apellido tiene como máximo{1} caracteres.")]
        public string Apellido { get; set; }

        [EmailAddress(ErrorMessage = "Formato incorecto de mail")]
        [MaxLength(150, ErrorMessage = "El Mail puede tener como máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña del usuario es obligatorio")]
        [MaxLength(200, ErrorMessage = "La contraseña puede tener como máximo{1} caracteres.")]
        public string Contrasena { get; set; }

        [Phone(ErrorMessage = "Formato incorrecto de numeración de teléfono")]
        [MaxLength(20, ErrorMessage = "El Número de Teléfono/celular puede tener como máximo {1} caracteres.")]
        public string Telefono { get; set; }

        [MaxLength(250, ErrorMessage = "La direccion tiene como máximo{1} caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El tipo de usuario es obligatorio")]
        [MaxLength(50, ErrorMessage = "El  tipo de usuario puede tener como máximo{1} caracteres.")]
        public string TipoUsuario { get; set; }

        // Relación: un usuario puede tener muchas Compras, Carritos y Consultas
        public List<Compra> Compras { get; set; }
        public List<Carrito> Carritos { get; set; } 
        public List<Consulta> Consultas { get; set; }
        
    }
}
