using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public class User : IRecordLogger
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Se debe ingresar un nombre de usuario")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Se debe ingresar el correo")]

        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        [Required(ErrorMessage = "Se debe ingresar una contraseña")]
        public string? UserPassword { get; set; }
        [Required(ErrorMessage = "Se debe seleccionar un ROL")]
        public int? UserRoleId { get; set; }
        public string? UserRole { get; set; }
        public RecordLog? RecordLog { get; set; } = new RecordLog();

    }
}

