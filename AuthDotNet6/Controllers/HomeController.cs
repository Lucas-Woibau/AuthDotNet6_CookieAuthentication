using AuthDotNet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthDotNet6.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index(bool erroLogin)
        {
            if (erroLogin)
            {
                ViewBag.Erro = "UserName or Password is incorrect!";
            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var userDB = new User()
            {
                UserName = "Lucas",
                Password = "123",
                UserOffice = ""
            };

            if (!userDB.UserName.Equals(user.UserName) ||
                !userDB.Password.Equals(user.Password)
                )
            {
                return RedirectToAction("Index", new { erroLogin = true });
            }

            //Creating user's cookie
            await new Services().Login(HttpContext, user);
            return RedirectToAction("Profile");

        }

        //To protect from the other peoples not authorized
        [Authorize]
        public async Task<IActionResult> Out()
        {
            await new Services().Logoff(HttpContext);
            return RedirectToAction("Index");
        }

        //Needs authorization
        [Authorize]
        public IActionResult Profile()
        {
            ViewBag.Permissions = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
            return View();
        }
    }
}