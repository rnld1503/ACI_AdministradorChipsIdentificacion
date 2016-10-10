using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACI_Web.Models;
using ACI_Web.Models.DataAccess;

namespace ACI_Web.Controllers
{
    public class PrincipalController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Principal/

        public ActionResult Index()
        {
            logger.Info("Inicia la aplicacion");
            return View();
        }

    }
}
