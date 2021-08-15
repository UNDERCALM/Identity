using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Identity.Server.Controllers;
using Identity.Server.Models;
using FakeItEasy;
using Identity.Server;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
namespace Identity.Tests.Controllers
{
   [TestClass]
   public class UsersControllerTest
   {
      [TestMethod]
      public async Task Get_Users_Returns_All_Users()
      {

         // Arrange
         ApplicationUserManager applicationUserManager = A.Fake<ApplicationUserManager>();


         //var fakeUsers = A.CollectionOfDummy<UsersControllerTest>(n).AsEnumerable();
         var fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";

         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));

         ApplicationRoleManager applicationRoleManager = A.Fake<ApplicationRoleManager>();
         UsersController controller = new UsersController(applicationUserManager, applicationRoleManager);

         // Act
         IHttpActionResult actionResult = await controller.GetUser(fakeUser.Id);


         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<User>;

         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);
         Assert.AreEqual(fakeUser.Id, contentResult.Content.Id);

      }
   }
}
