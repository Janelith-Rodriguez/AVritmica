using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVritmica.BD.Data.Entity
{
    [Index(nameof(Nombre), Name = "Consulta_UQ", IsUnique = true)]
    public class Consulta : EntityBase
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "El nombre de la consulta es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre tiene como máximo{1} caracteres.")]
        public string Nombre { get; set; }

        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [MaxLength(150, ErrorMessage = "El Mail puede tener como máximo{1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El mensaje de la consulta es obligatorio")]
        [MaxLength(500, ErrorMessage = "El mensaje tiene como máximo{1} caracteres.")]
        public string Mensaje { get; set; }

        [Required(ErrorMessage = "La fecha de envio es obligatorio")]
        public DateTime Fecha_Envio { get; set; }
    }
}
