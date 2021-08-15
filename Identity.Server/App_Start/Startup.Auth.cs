using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Identity.Server.Providers;
using Identity.Server.Models;
using Identity.Server.Helpers;

namespace Identity.Server
{
   public partial class Startup
   {
      public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

      public static string PublicClientId { get; private set; }

      // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
      public void ConfigureAuth(IAppBuilder app)
      {
         // Configure the db context and user manager to use a single instance per request
         app.CreatePerOwinContext(ApplicationDbContext.Create);
         app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
         app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
         app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

         //Configure the application for OAuth based flow
         ConfigureOAuthForJWT(app);
         ConfigureJWTConsumption(app);
        
      }

      public void ConfigureOAuthForJWT(IAppBuilder app)
      {
         var expireTime =
         PublicClientId = "self";
         OAuthOptions = new OAuthAuthorizationServerOptions
         {
            TokenEndpointPath = new PathString("/api/Account/Login"),
            Provider = new ApplicationOAuthProvider(PublicClientId),
            AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Utils.Configuration.TokenExpireTimeMinutes),
            // In production mode set AllowInsecureHttp = false
            AllowInsecureHttp = true,
            AccessTokenFormat = new JWTFormat(Utils.Configuration.TokenIssuer),
         };
         app.UseOAuthAuthorizationServer(OAuthOptions);
         app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
      }

      private void ConfigureJWTConsumption(IAppBuilder app)
      {
         var issuer = Utils.Configuration.TokenIssuer;
         string audienceId = Utils.Configuration.TokenAudienceId;
         byte[] audienceSecret = TextEncodings.Base64Url.Decode(Utils.Configuration.TokenAudienceSecret);

         // Api controllers with an [Authorize] attribute will be validated with JWT
         app.UseJwtBearerAuthentication(
             new JwtBearerAuthenticationOptions
             {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audienceId },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                 {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                 }
             });
      }
   }
}

