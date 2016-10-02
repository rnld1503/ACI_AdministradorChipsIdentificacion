using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACI_Web.Models.ViewModels
{
    public class Chip_OrdenAtencion
    {
        public int IdOrdenAtencion { get; set; }
        public int IdChip { get; set; }
        public string EstadoOrdenAtencion{ get; set; }
        public string EstadoChip { get; set; }
    }
}