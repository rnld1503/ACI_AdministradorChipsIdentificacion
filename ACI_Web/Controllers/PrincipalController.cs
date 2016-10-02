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
        //
        // GET: /Principal/

        public ActionResult Index()
        {

            return View();
        }

    }
}
