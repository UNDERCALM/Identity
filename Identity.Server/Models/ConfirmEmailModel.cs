using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity.Server.Models
{
   public class ConfirmEmailModel
   {
      public bool Success { get; set; }
      public string Message { get; set; }
   }
}