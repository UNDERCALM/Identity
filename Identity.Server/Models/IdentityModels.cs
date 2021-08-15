using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;

namespace Identity.Server.Models
{
   public class ApplicationDbContext : IdentityDbContext<User>
    {
     
      
      public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
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

     
   }
}