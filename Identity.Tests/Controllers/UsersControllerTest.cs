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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Tests.Controllers
{
   [TestClass]
   public class UsersControllerTest
   {
      private ApplicationUserManager applicationUserManager;
      private ApplicationRoleManager applicationRoleManager;
      private UsersController controller;
      private void SetupManagers() {
      }
      public UsersControllerTest()
      {
         applicationUserManager = A.Fake<ApplicationUserManager>();
         applicationRoleManager = A.Fake<ApplicationRoleManager>();
         controller = new UsersController(applicationUserManager, applicationRoleManager);
      }
      [TestMethod]
      public async Task Get_User_Returns_A_User()
      {
         // Arrange
         var fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));
         // Act
         IHttpActionResult actionResult = await controller.GetUser(fakeUser.Id);
         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<User>;
         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);
         Assert.AreEqual(fakeUser.Id, contentResult.Content.Id);

      }
      [TestMethod]
      public async Task Put_User_Saves_A_User()
      {

         // Arrange
         var fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         var fakeUserData = new UsersBindingModel();
         fakeUserData.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         fakeUserData.FirstName = "Jhon";
         fakeUserData.LastName = "Smit";
         fakeUserData.Zip = "j.smit @challenge.com";
         fakeUserData.Email = "j.smit@challenge.com";
         fakeUserData.StreetAddress = "55 test St";
         fakeUserData.State = "California";
         fakeUserData.City = "California";
         fakeUserData.Zip = "5555555";
         fakeUserData.PhoneNumber = "1-555-555-5555";
         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));
         A.CallTo(() => applicationUserManager.UpdateAsync(fakeUser)).Returns(Task.FromResult(IdentityResult.Success));
         

         // Act
         IHttpActionResult actionResult = await controller.PutUser(fakeUserData);
         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<User>;
         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);
         Assert.AreEqual(fakeUser.Email, contentResult.Content.Email);

      }
      [TestMethod]
      public async Task Delete_A_User_Returns_Deleted_User()
      {

         // Arrange
         var fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         string provider = "Local";
         string providerKey = "";
         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));
         A.CallTo(() => applicationUserManager.UpdateAsync(fakeUser)).Returns(Task.FromResult(IdentityResult.Success));
         A.CallTo(() => applicationUserManager.RemovePasswordAsync(fakeUser.Id)).Returns(Task.FromResult(IdentityResult.Success));
         A.CallTo(() => applicationUserManager.RemoveLoginAsync(fakeUser.Id, new UserLoginInfo(provider, providerKey))).Returns(Task.FromResult(IdentityResult.Success));
         A.CallTo(() => applicationUserManager.DeleteAsync(fakeUser)).Returns(Task.FromResult(IdentityResult.Success));
         
            
            
         // Act
         IHttpActionResult actionResult = await controller.DeleteUser(fakeUser.Id,"Local","");
         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<User>;
         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);

      }
      [TestMethod]
      public async Task Activate_User_Returns_User()
      {

         // Arrange
         var fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         fakeUser.IsActive = false;
         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));
         A.CallTo(() => applicationUserManager.UpdateAsync(fakeUser)).Returns(Task.FromResult(IdentityResult.Success));


         // Act
         IHttpActionResult actionResult = await controller.ActivateUser(fakeUser.Id);
         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<User>;
         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);
         Assert.AreEqual(fakeUser.IsActive, true);

      }
      public async Task AssignRole_To_User_Returns_User()
      {

         // Arrange
         User fakeUser = new User();
         fakeUser.Id = "ac924b35-f17a-437a-b70c-f1b094b18094";
         fakeUser.IsActive = false;
         IdentityRole fakeRole = new IdentityRole();
         fakeRole.Name = "Admin";
         A.CallTo(() => applicationUserManager.FindByIdAsync(fakeUser.Id)).Returns(Task.FromResult(fakeUser));
         A.CallTo(() => applicationRoleManager.FindByNameAsync(fakeRole.Name)).Returns(Task.FromResult(fakeRole));
         A.CallTo(() => applicationUserManager.AddToRoleAsync(fakeUser.Id, fakeRole.Name)).Returns(Task.FromResult(IdentityResult.Success));
        
         
         // Act
         IHttpActionResult actionResult = await controller.ActivateUser(fakeUser.Id);
         // Assert
         var contentResult = actionResult as OkNegotiatedContentResult<IdentityRole>;
         Assert.IsNotNull(contentResult);
         Assert.IsNotNull(contentResult.Content);
         Assert.AreEqual(fakeRole.Name, contentResult.Content.Name);

      }
   }
}
