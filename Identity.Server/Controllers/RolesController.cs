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
using System.Web.Http.Description;
using Identity.Server.Helpers;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace Identity.Server.Controllers
{
   [Authorize(Roles = "Administrator")]
   [RoutePrefix("api/Roles")]
   public class RolesController : ApiController
   {
      private ApplicationRoleManager _roleManager ;
      public RolesController()
      {

      }
      public RolesController(ApplicationRoleManager roleManager)
      {
         RoleManager = roleManager;
      }
      public ApplicationRoleManager RoleManager
      {
         get
         {
            return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>(); ;
         }
         private set
         {
            _roleManager = value;
         }
      }
      // GET: api/Roles
      [ResponseType(typeof(IQueryable<RoleViewModel>))]
      public IHttpActionResult GetRoles()
      {

       
        if(RoleManager.Roles == null || RoleManager.Roles.Count() <= 0)
         {
            return NotFound();
         }
         var result = RoleManager.Roles.Select(role => new RoleViewModel() { Id = role.Id, Name = role.Name });
         return Ok(result);

      }

      // GET: api/Roles/5
      [ResponseType(typeof(IdentityRole))]
      public async Task<IHttpActionResult> GetRole(string id)
      {
         IdentityRole role = await RoleManager.FindByIdAsync(id);
         if (role == null)
         {
            return NotFound();
         }

         return Ok(role);
      }

      // PUT: api/Roles/5
      [ResponseType(typeof(void))]
      public async Task<IHttpActionResult> PutRole(string id, IdentityRole role)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }

         if (id != role.Id)
         {
            return BadRequest();
         }

         

         try
         {
            await RoleManager.UpdateAsync(role);
         }
         catch (DbUpdateConcurrencyException)
         {
            if (!RoleExists(id))
            {
               return NotFound();
            }
            else
            {
               throw;
            }
         }

         return StatusCode(HttpStatusCode.NoContent);
      }

      // POST: api/Roles
      [ResponseType(typeof(IdentityRole))]
      public async Task<IHttpActionResult> PostRole(IdentityRole role)
      {
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }



         try
         {
            await RoleManager.CreateAsync(role);
         }
         catch (DbUpdateException)
         {
            if (RoleExists(role.Id))
            {
               return Conflict();
            }
            else
            {
               throw;
            }
         }

         return CreatedAtRoute("DefaultApi", new { id = role.Id }, role);
      }

      // DELETE: api/Roles/5
      [ResponseType(typeof(IdentityRole))]
      public async Task<IHttpActionResult> DeleteRole(string id)
      {
         IdentityRole role = await RoleManager.FindByIdAsync(id);
         if (role == null)
         {
            return NotFound();
         }

         await RoleManager.DeleteAsync(role);
       

         return Ok(role);
      }

      protected override void Dispose(bool disposing)
      {
         if (disposing && _roleManager != null)
         {
            _roleManager.Dispose();
            _roleManager = null;
         }

         base.Dispose(disposing);
      }

      private bool RoleExists(string id)
      {
         return RoleManager.FindById(id) != null;
      }
   }
}