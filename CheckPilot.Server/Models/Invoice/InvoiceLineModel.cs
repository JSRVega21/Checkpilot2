using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public class InvoiceLineModel
    {
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; } 
        public decimal Quantity { get; set; }
        public decimal OpenCreQty { get; set; } 
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
    }

}
