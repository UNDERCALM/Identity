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
      // GET: api/Users/5
      [ResponseType(typeof(User))]
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
      // GET: api/Users
      [ResponseType(typeof(IQueryable<UsersBindingModel>))]
      public async Task<IHttpActionResult> GetUsers()
      {
        
         if (UserManager.Users == null || UserManager.Users.Count() <= 0)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"no users found"));
         }
       
         IQueryable<UsersBindingModel> users = UserManager.Users.Select(t => new UsersBindingModel
         {
            Id = t.Id,
            IsActive = t.IsActive,
            Email = t.Email,
            Username = t.UserName,
            PhoneNumber = t.PhoneNumber,
            FirstName = t.FirstName,
            LastName = t.LastName,
            State = t.State,
            StreetAddress = t.StreetAddress,
            City = t.City,
            EmailConfirmed = t.EmailConfirmed, 
            PhoneNumberConfirmed = t.PhoneNumberConfirmed, 
            Zip = t.Zip
            
         });
         if (users == null)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"no users found"));
         }
         IQueryable<UsersBindingModel> result = await Task.FromResult(users);
         return Ok(result);
      }

      // PUT: api/Users/5
      [ResponseType(typeof(User))]
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

         return Ok(user);
      }

      // DELETE: api/Users/5
      [ResponseType(typeof(User))]
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


         return Ok(user);
      }
      // POST api/Users/ActivateUser
      [Route("ActivateUser")]
      [HttpPost]
      public async Task<IHttpActionResult> ActivateUser(string id) 
      {
         User user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"user with the id: {id} not found"));
         }
         user.IsActive = true;
         IdentityResult result = await UserManager.UpdateAsync(user);

         if (!result.Succeeded)
         {
            return GetErrorResult(result);
         }

         return Ok(user);
      }

      // POST api/Users/AssignRole
      [Route("AssignRole")]
      [HttpPost]
      public async Task<IHttpActionResult> AssignRole(string id,string roleName)
      {
         User user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"user with the id: {id} not found"));
         }
         IdentityRole role = await RoleManager.FindByNameAsync(roleName);
         if (role == null)
         {
            throw new HttpResponseException(
            Request.CreateErrorResponse(HttpStatusCode.NotFound, $"the role {roleName} was not found"));
         }

         IdentityResult result = await UserManager.AddToRoleAsync(user.Id,roleName);

         if (!result.Succeeded)
         {
            return GetErrorResult(result);
         }
         
         return Ok(role);
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