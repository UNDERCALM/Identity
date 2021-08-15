using Identity.Server;
using Identity.Server.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Identity.Tests.Controllers
{
   [TestClass]
   public class HomeControllerTest
   {
      [TestMethod]
      public void Index()
      {
         // Arrange
         HomeController controller = new HomeController();

         // Act
         ViewResult result = controller.Index() as ViewResult;

         // Assert
         Assert.IsNotNull(result);
         Assert.AreEqual("Identity Server", result.ViewBag.Title);
      }
   }
}
