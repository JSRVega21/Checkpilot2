using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    [NotMapped]
    public class RecordLog
    {
        [StringLength(256)]
        [Comment("Usuario que creo el registro")]
        public string CreatedBy { get; set; } = string.Empty;
        [Comment("Fecha y hora de creación del registro")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [StringLength(256)]
        [Comment("Ultimo usuario que modificó el registro")]
        public string UpdatedBy { get; set; } = string.Empty;
        [Comment("Ultima fecha y hora de actualización del registro")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Comment("Registro activo")]
        public bool IsActive { get; set; } = true;
        [Comment("Es un registro del sistema, los registros del sistema no pueden ser eliminados")]
        public bool IsSystem { get; set; } = false;

        [Comment("Estatus de sincronización del registro")]
        public SyncStatus SyncStatus { get; set; }
        [Comment("Ultima fecha de sincronización")]
        public DateTime SyncDate { get; set; }

        [StringLength(64)]
        [Comment("Código identificador del objeto representado en el registro")]
        public string ObjectKey { get; set; } = string.Empty;
        [StringLength(36)]
        [Comment("Identificador único del registro, asignado en el momento de creación")]
        public string RecordKey { get; set; } = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return CreatedDate == UpdatedDate ? $"Creado: {CreatedDate}" : $"Actualizado: {UpdatedDate}";
        }
    }
}

