using Identity.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Identity.Server.Controllers
{
   public class HomeController : Controller
   {
      public ActionResult Index()
      {
         ViewBag.Title = "Identity Server";

         return View();
      }
      public ActionResult ConfirmEmail(string id, string token) {
         var model = new ConfirmEmailModel();
         model.Success = true;
         model.Message = "Email Confirmed";
         return View(model);
      }
   }
}
