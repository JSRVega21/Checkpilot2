using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public class InvoicePhoto : IRecordLogger
    {
        public int InvoicePhotoId { get; set;}
        public int? DocEntry { get; set; }
        public string? NumAtCard { get; set; }
        public int? DocNum { get; set; }
        public byte[]? BytePhoto { get; set; }
        public byte[]? ByteSignature { get; set; }
        public string? Location { get; set; }
        public RecordLog? RecordLog { get; set; } = new RecordLog();
    }
}