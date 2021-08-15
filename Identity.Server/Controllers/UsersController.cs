using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Identity.Server.Models;
using Identity.Server.Providers;
using Identity.Server.Results;
using System.Linq;
using Identity.Server.Helpers;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace Identity.Server.Controllers
{
   [Authorize(Roles = "Administrator")]
   [RoutePrefix("api/Users")]
   public class UsersController : ApiController
    {
      private ApplicationUserManager _userManager;
      private ApplicationRoleManager _roleManager;

      public UsersController()
      {

      }
      public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
      {
         UserManager = userManager;
         RoleManager = roleManager;
      }
      public ApplicationUserManager UserManager
      {
         get
         {
            return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
         }
         private set
         {
            _userManager = value;
         }
      }
      public ApplicationRoleManager RoleManager
      {
         get
         {
            return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
         }
         private set
         {
            _roleManager = value;
         }
      }
      // GET: api/Roles/5
      [ResponseType(typeof(IdentityRole))]
      public async Task<IHttpActionResult> GetUser(string id)
      {
         User user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"user with the id: {id} not found"));
         }

         return Ok(user);
      }

      // PUT: api/Users/5
      [ResponseType(typeof(void))]
      public async Task<IHttpActionResult> PutUser(UsersBindingModel model)
      {
         if (string.IsNullOrEmpty(model.Id))
         {
            return BadRequest("User Id is Mandatory");
         }
         User user = await UserManager.FindByIdAsync(model.Id);
         if (user == null)
         {
            throw new HttpResponseException(
           Request.CreateErrorResponse(HttpStatusCode.NotFound, $"user with the id: {model.Id} not found"));
         }

         user.UserName = model.Email ?? user.UserName;
         user.Email = model.Email ?? user.Email;
         user.FirstName = model.FirstName ?? user.FirstName;
         user.LastName = model.LastName ?? user.LastName;
         user.StreetAddress = model.StreetAddress ?? user.StreetAddress;
         user.State = model.State ?? user.State;
         user.City = model.City ?? user.City;
         user.Zip = model.Zip ?? user.Zip;
         user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

         IdentityResult result = await UserManager.UpdateAsync(user);

         if (!result.Succeeded)
         {
            return GetErrorResult(result);
         }

         return Ok();
      }

   

      // DELETE: api/Roles/5
      [ResponseType(typeof(IdentityRole))]
      public async Task<IHttpActionResult> DeleteUser(string id, string provider = "Local", string providerKey = "")
      {
         User user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            return NotFound();
         }
         IdentityResult result;

         if (provider == "Local")
         {
            result = await UserManager.RemovePasswordAsync(user.Id);
         }
         else
         {
            result = await UserManager.RemoveLoginAsync(user.Id,
                new UserLoginInfo(provider, providerKey));
         }

         if (!result.Succeeded)
         {
            return GetErrorResult(result);
         }
         await UserManager.DeleteAsync(user);


         return Ok();
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (_roleManager != null) {
               _roleManager.Dispose();
               _roleManager = null;
            }
            if (_userManager != null)
            {
               _userManager.Dispose();
               _userManager = null;
            }
         }
        
         base.Dispose(disposing);
      }

      private bool UserExists(string id)
      {
         return UserManager.FindById(id) != null;
      }
      private IHttpActionResult GetErrorResult(IdentityResult result)
      {
         if (result == null)
         {
            return InternalServerError();
         }

         if (!result.Succeeded)
         {
            if (result.Errors != null)
            {
               foreach (string error in result.Errors)
               {
                  ModelState.AddModelError("", error);
               }
            }

            if (ModelState.IsValid)
            {
               // No ModelState errors are available to send, so just return an empty BadRequest.
               return BadRequest();
            }

            return BadRequest(ModelState);
         }

         return null;
      }
   }
}