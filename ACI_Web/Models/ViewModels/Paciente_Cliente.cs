using System;
using System.Collections.Generic;

namespace ACI_Web.Models.ViewModels
{
    public class Paciente_Cliente
    {
        public int IdOrdenAtencion { get; set; }
        public string NombrePaciente { get; set; }
        public int EdadPaciente { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }

    }
}