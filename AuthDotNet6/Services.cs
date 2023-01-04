using AuthDotNet6.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace AuthDotNet6
{
    public class Services 
    {
        //Add user login function
        public async Task Login(HttpContext context, User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Role, user.UserOffice));

            var claimsIdentity =
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme)//Authentication with cookies with the claims than created up there
                    ); 

            //Cookie configurations
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(8), //Cookie expiration time
                IssuedUtc = DateTime.Now   //The date when the cookies has ben created
            };

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsIdentity, authProperties);
        }

        //Add cookie remotion
        public async Task Logoff(HttpContext contexto)
        {
            await contexto.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

    }
}
