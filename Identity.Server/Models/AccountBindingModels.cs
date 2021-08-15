using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Identity.Server.Models
{
   // Models used as parameters to AccountController actions.

   public class AddExternalLoginBindingModel
   {
      [Required]
      [Display(Name = "External access token")]
      public string ExternalAccessToken { get; set; }
   }

   public class ChangePasswordBindingModel
   {
      [Required]
      [DataType(DataType.Password)]
      [Display(Name = "Current password")]
      public string OldPassword { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm new password")]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }

   public class UsersBindingModel
   {

      [Display(Name = "Id")]
      public string Id { get; set; }


      [Display(Name = "Username")]
      public string Username { get; set; }

      [Display(Name = "Email")]
      public string Email { get; set; }

      [Display(Name = "Email Confirmed")]
      public bool EmailConfirmed { get; set; }

      [Display(Name = "Is Active")]
      public bool IsActive { get; set; }

      [Display(Name = "First Name")]
      public string FirstName { get; set; }

      [Display(Name = "Last Name")]
      public string LastName { get; set; }


      [Display(Name = "Street Address")]
      public string StreetAddress { get; set; }

      [Display(Name = "State")]
      public string State { get; set; }

      [Display(Name = "City")]
      public string City { get; set; }

      [Display(Name = "Zip Code")]
      public string Zip { get; set; }

      [Display(Name = "Phone Number")]
      public string PhoneNumber { get; set; }
      [Display(Name = "Phone Number Confirmed")]
      public bool PhoneNumberConfirmed { get; set; }

   }
   public class RegisterBindingModel
   {
      [Display(Name = "Username")]
      public string Username { get; set; }

      [Display(Name = "Email")]
      public string Email { get; set; }

      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "Password")]
      public string Password { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm password")]
      [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }

      [Required]
      [Display(Name = "First Name")]
      public string FirstName { get; set; }

      [Required]
      [Display(Name = "Last Name")]
      public string LastName { get; set; }

      [Required]
      [Display(Name = "Street Address")]
      public string StreetAddress { get; set; }

      [Required]
      [Display(Name = "State")]
      public string State { get; set; }
      [Required]
      [Display(Name = "City")]
      public string City { get; set; }

      [Required]
      [Display(Name = "Zip Code")]
      public string Zip { get; set; }

      [Required]
      [Display(Name = "Phone Number")]
      public string PhoneNumber { get; set; }

   }

   public class RegisterExternalBindingModel
   {
      [Required]
      [Display(Name = "Email")]
      public string Email { get; set; }
   }

   public class RemoveLoginBindingModel
   {
      [Required]
      [Display(Name = "Login provider")]
      public string LoginProvider { get; set; }

      [Required]
      [Display(Name = "Provider key")]
      public string ProviderKey { get; set; }
   }

   public class SetPasswordBindingModel
   {
      [Required]
      [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm new password")]
      [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
      public string ConfirmPassword { get; set; }
   }
}
