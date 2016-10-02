using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACI_Web.Models;
using ACI_Web.Models.DataAccess;
using ACI_Web.Models.ViewModels;

namespace ACI_Web.Controllers
{
    public class RegistroEscaneoController : Controller
    {


        public ActionResult OrdenAtencion()
        {
            List<ACI_OrdenAtencion> ordenatencion = new List<ACI_OrdenAtencion>();
            List<Paciente_Cliente> modelo = new List<Paciente_Cliente>();
            try
            {
                using (ACI_ModelConection model = new ACI_ModelConection())
                {
                    ordenatencion = model.ACI_OrdenAtencion.ToList();
                    foreach (ACI_OrdenAtencion item in ordenatencion)
                    {
                        modelo.Add(
                            new Paciente_Cliente()
                            {
                                IdOrdenAtencion = item.idOrdenAtencion,
                                NombrePaciente = item.ACI_Paciente.nombrePaciente,
                                EdadPaciente = item.ACI_Paciente.edadPaciente,
                                NombreCliente = item.ACI_Paciente.ACI_Cliente.nombreCliente,
                                Estado = item.estadoAtencion
                            }
                            );
                    }
                }

                return View(modelo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DetalleOrdenAtencion(int id)
        {
            ACI_OrdenAtencion ordenatencion = new ACI_OrdenAtencion();
            Paciente_Cliente_Orden modelo = new Paciente_Cliente_Orden();
            try
            {
                PopulateMotivos();
                using (ACI_ModelConection model = new ACI_ModelConection())
                {
                    ordenatencion = model.ACI_OrdenAtencion.Where(d => d.idOrdenAtencion == id).FirstOrDefault();

                    modelo.IdOrdenAtencion = ordenatencion.idOrdenAtencion;
                    modelo.FechaOrdenAtencion = ordenatencion.fechaOrdenAtencion;
                    modelo.EstadoOrden = ordenatencion.estadoAtencion;
                    modelo.idCliente = ordenatencion.ACI_Paciente.ACI_Cliente.idCliente;
                    modelo.nombreCliente = ordenatencion.ACI_Paciente.ACI_Cliente.nombreCliente;
                    modelo.DNICliente = ordenatencion.ACI_Paciente.ACI_Cliente.dniCliente;
                    modelo.IdPaciente = ordenatencion.ACI_Paciente.idPaciente;
                    modelo.FechaNacimientoPaciente = ordenatencion.ACI_Paciente.fechaNacimientoPaciente;
                    modelo.TipoPaciente = ordenatencion.ACI_Paciente.tipoPaciente;
                    modelo.RazaPaciente = ordenatencion.ACI_Paciente.razaPaciente;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(modelo);
        }


        [HttpPost]
        public ActionResult DetalleOrdenAtencion(FormCollection collection)
        {
            try
            {
                ACI_OrdenAtencion orden = new ACI_OrdenAtencion();
                using (ACI_ModelConection model = new ACI_ModelConection())
                {
                    int codigo=Convert.ToInt32(collection["IdOrdenAtencion"].Trim());
                    orden = model.ACI_OrdenAtencion.Where(d =>d.idOrdenAtencion == codigo).FirstOrDefault();

                    orden.chipImplantado = Convert.ToBoolean(Convert.ToBoolean(collection["ChipImplantado"].Split(',')[0]));
                    if (orden.chipImplantado)
                    {
                        orden.motivoImplantacion = collection["MotivoImplantacion"];
                        if (orden.motivoImplantacion == "Defectuoso")
                            orden.estadoAtencion = "Pendiente de implantación";
                        if (orden.motivoImplantacion == "Actualizar información")
                            orden.estadoAtencion = "Terminado";
                    }
                    else
                    {
                        orden.motivoImplantacion = string.Empty;
                        orden.estadoAtencion = "Pendiente de implantación";
                    }

                    orden.observaciones = collection["Observaciones"];


                    model.SaveChanges();
                }
                return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        private void PopulateMotivos()
        {
            try
            {
                List<SelectListItem> estados = new List<SelectListItem>();
                estados.Add(new SelectListItem() { Text = "Defectuoso", Value = "Defectuoso" });
                estados.Add(new SelectListItem() { Text = "Actualizar información", Value = "Actualizar información" });

                ViewBag.MotivoImplantacion = new SelectList(estados, "Value", "Text", 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



    }
}
