using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACI_Web.Models;
using System.Configuration;
using ACI_Web.Models.DataAccess;
using ACI_Web.Models.ViewModels;


namespace ACI_Web.Controllers
{
    public class AsignacionOrdenAtencionController : Controller
    {
        //
        // GET: /AsignacionOrdenAtencion/

        public ActionResult Busqueda()
        {
            try
            {
                PopulateEstadosChip();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetEstadoOrden(int codOrden)
        {
            try
            {
                string estado = string.Empty;
                using (ACI_ModelConection model = new ACI_ModelConection())
                {

                    bool exist = model.ACI_OrdenAtencion.Where(f => f.idOrdenAtencion == codOrden).Any();
                    if (!exist)
                    {
                        return Json(new { success = false, responseText = "No se encontró la orden de atención " + codOrden }, JsonRequestBehavior.AllowGet);
                    }else {
                        estado = model.ACI_OrdenAtencion.Where(f => f.idOrdenAtencion == codOrden).FirstOrDefault().estadoAtencion;
                    }
                    
                }

                if (estado == ConfigurationManager.AppSettings["strEstadoAtencion1"])
                    return Json(new { success = true, responseText = estado }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, responseText = "No se puede asignar el chip a la orden" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetEstadoChip(int codChip)
        {
            try
            {
                string estado = string.Empty;
                using (ACI_ModelConection model = new ACI_ModelConection())
                {
                    bool exist = model.ACI_Chip.Where(f => f.idChip == codChip).Any();

                    if(exist){
                        estado = model.ACI_Chip.Where(f => f.idChip == codChip).FirstOrDefault().estado;
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "No se encontró el Chip " + codChip }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = true, responseText = estado }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GuardarEstadoChip(int codChip, string estado)
        {
            try
            {
                bool cambiardefectousoAactivo = true;
                ACI_Chip chip = new ACI_Chip();
                if (estado == ConfigurationManager.AppSettings["strEstadochip2"])
                {
                    using (ACI_ModelConection model = new ACI_ModelConection())
                    {
                        if (model.ACI_Chip_OrdenAtencion.Where(f => f.ACI_Chip.idChip == codChip).Count() > 0)
                            cambiardefectousoAactivo = false;
                    }
                }

                if (cambiardefectousoAactivo)
                {
                    using (ACI_ModelConection model = new ACI_ModelConection())
                    {
                        chip = model.ACI_Chip.Where(f => f.idChip == codChip).FirstOrDefault();
                        chip.estado = estado;
                        model.SaveChanges();
                    }
                    return Json(new { success = true, responseText = "Se actualizó el estado del chip" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "El chip ya esta asignado a una orden. No se puede cambiar el estado del chip" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AsignarOrdenAtencion(int codChip, int codOrden)
        {
            try
            {
                ACI_Chip_OrdenAtencion chip = new ACI_Chip_OrdenAtencion();
                ACI_OrdenAtencion orden = new ACI_OrdenAtencion();
                ACI_Chip_OrdenAtencion validar;

                /////////////////////////////////////////////////////////////
                bool validardefectouso = true;
                string estadodefectouso=ConfigurationManager.AppSettings["strEstadochip2"];
                using (ACI_ModelConection model = new ACI_ModelConection()) {
                    if (model.ACI_Chip.Where(g => g.idChip == codChip && g.estado == estadodefectouso).Count() > 0)
                        validardefectouso = false;
                }
                if( !validardefectouso )
                    return Json(new { success = false, responseText = "El chip tiene un estado " + estadodefectouso + ". No se puede asignar el chip a la orden" }, JsonRequestBehavior.AllowGet);
                /////////////////////////////////////////////////////////////


                /////////////////////////////////////////////////////////////
                bool validarestadoorden = true;
                string estadodefectousoorden=ConfigurationManager.AppSettings["strEstadoAtencion1"];
                using (ACI_ModelConection model = new ACI_ModelConection()) {
                    if (model.ACI_OrdenAtencion.Where(d => d.idOrdenAtencion == codOrden && d.estadoAtencion != estadodefectousoorden).Count() > 0)
                        validarestadoorden = false;
                }
                if (!validarestadoorden)
                    return Json(new { success = false, responseText = "La orden no tiene un estado " + estadodefectousoorden + " . No se puede asignar la orden al chip" }, JsonRequestBehavior.AllowGet);
                /////////////////////////////////////////////////////////////



                using (ACI_ModelConection model = new ACI_ModelConection())
                {
                    validar = model.ACI_Chip_OrdenAtencion.Where(f => f.idChip == codChip && f.idOrdenAtencion == codOrden).FirstOrDefault();
                    if (validar == null)
                    {
                        if (model.ACI_Chip_OrdenAtencion.Count() > 0)
                            chip.idChipOrdenAtencion = model.ACI_Chip_OrdenAtencion.Max(d => d.idOrdenAtencion) + 1;
                        else
                            chip.idChipOrdenAtencion = 1;
                        chip.idOrdenAtencion = codOrden;
                        chip.idChip = codChip;
                        model.ACI_Chip_OrdenAtencion.Add(chip);


                        orden = model.ACI_OrdenAtencion.Where(f => f.idOrdenAtencion == codOrden).First();
                        orden.estadoAtencion = ConfigurationManager.AppSettings["strEstadoAtencion3"];

                        model.SaveChanges();
                    }
                }

                if (validar == null)
                {
                    return Json(new { success = true, responseText = "El chip  ha sido asignado a la orden de atención" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "El chip no se puede asignar a la orden de atencion porque ya tiene asignado" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        private void PopulateEstadosChip()
        {
            try
            {
                List<SelectListItem> estados = new List<SelectListItem>();
                estados.Add(new SelectListItem() { Text = ConfigurationManager.AppSettings["strEstadochip1"], Value = ConfigurationManager.AppSettings["strEstadochip1"] });
                estados.Add(new SelectListItem() { Text = ConfigurationManager.AppSettings["strEstadochip2"], Value = ConfigurationManager.AppSettings["strEstadochip2"] });

                ViewBag.EstadoChip = new SelectList(estados, "Value", "Text", 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
