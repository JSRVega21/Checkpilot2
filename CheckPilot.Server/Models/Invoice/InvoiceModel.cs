using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public class InvoiceModel
    {
        public int DocEntry { get; set; }
        public string NumAtCard { get; set; }
        public int DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public decimal DocTotal { get; set; }
        public List<InvoiceLineModel> DocumentLines { get; set; }
    }
}