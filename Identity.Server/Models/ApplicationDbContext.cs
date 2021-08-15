using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;

namespace Identity.Server.Models
{
   public class ApplicationDbContext : IdentityDbContext<User>
   {


      public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
      {
      }
      static ApplicationDbContext()
      {

         Database.SetInitializer<ApplicationDbContext>(new IdentityDbInit());

      }
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<User>().ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("UserId");


         modelBuilder.Entity<IdentityRole>().ToTable("Roles", "dbo").Property(p => p.Name).HasColumnName("Name");

         modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "dbo");


         modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "dbo");


         modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "dbo");
      }
      public static ApplicationDbContext Create()
      {
         return new ApplicationDbContext();
      }
      public class IdentityDbInit : DropCreateDatabaseAlways<ApplicationDbContext>
      {
         protected override void Seed(ApplicationDbContext context)
         {
            PerformInitialSetup(context);
            base.Seed(context);
         }
         public void PerformInitialSetup(ApplicationDbContext context)
         {
            using (var userManager = new UserManager<User>(new UserStore<User>(context)))
            {
               using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
               {
                  //system Role
                  if (!roleManager.RoleExists("Administrator"))
                  {
                     roleManager.Create(new IdentityRole("Administrator"));
                  }
                  if (!roleManager.RoleExists("User"))
                  {
                     roleManager.Create(new IdentityRole("User"));
                  }

                  var admin = new User() { UserName = "admin", IsActive = true, FirstName = "admin", Email = "admin@localhost", PhoneNumber = "00000000000" };
                  if (userManager.Create(admin, "Password@123") != IdentityResult.Success)
                  {
                     throw new Exception("failed");
                  }
                  var user = new User() { UserName = "user", FirstName = "user", IsActive = true, Email = "user@localhost", PhoneNumber = "000000005555" };
                  if (userManager.Create(user, "Password@123") != IdentityResult.Success)
                  {
                     throw new Exception("Failed");
                  }


                  // role
                  userManager.AddToRole(admin.Id, "Administrator");
                  userManager.AddToRole(user.Id, "User");

                  context.SaveChanges();
               }
            }

         }

      }
   }
}