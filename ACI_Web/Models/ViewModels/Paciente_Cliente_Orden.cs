using System;
using System.Collections.Generic;

namespace ACI_Web.Models.ViewModels
{
    public class Paciente_Cliente_Orden
    {
        public int IdOrdenAtencion { get; set; }
        public DateTime FechaOrdenAtencion { get; set; }
        public string EstadoOrden { get; set; }
        public string Observaciones { get; set; }
        public bool ChipImplantado { get; set; }
        public string MotivoImplantacion { get; set; }        

        public int CodigoChip { get; set; }
        public string EstadoChip { get; set; }


        public int idCliente { get; set; }
        public string nombreCliente { get; set; }
        public string DNICliente { get; set; }

        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; }
        public DateTime FechaNacimientoPaciente { get; set; }
        public string TipoPaciente { get; set; }
        public string RazaPaciente { get; set; }

    }
}