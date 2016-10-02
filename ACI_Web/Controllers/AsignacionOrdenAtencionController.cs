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
                    estado = model.ACI_OrdenAtencion.Where(f => f.idOrdenAtencion == codOrden).FirstOrDefault().estadoAtencion;
                }
                return Json(new { success = true, responseText = estado }, JsonRequestBehavior.AllowGet);
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
                    estado = model.ACI_Chip.Where(f => f.idChip == codChip).FirstOrDefault().estado;
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
            return View();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AsignarOrdenAtencion(string codChip)
        {
            return View();
        }


        private void PopulateEstadosChip()
        {
            try
            {
                List<SelectListItem> estados = new List<SelectListItem>();
                estados.Add(new SelectListItem() { Text = "Activo", Value = "Activo" });
                estados.Add(new SelectListItem() { Text = "Defectuoso", Value = "Defectuoso" });

                ViewBag.EstadoChip = new SelectList(estados, "Value", "Text", 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
