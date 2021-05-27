using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using YukikoSite.Models;
using YukikoSite.Models.Account;

namespace YukikoSite.Controllers {
    public class AccountController : Controller {
        private AppDbContext dbContext;

        public AccountController(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("logintoadmin")]
        public IActionResult LoginToAdmin() => View();

        [HttpPost]
        [Route("logintoadmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginToAdmin(LoginModel model) {
            if (ModelState.IsValid) {
                User user = await dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == model.Login && u.Password == model.Password);
                if (user != null) {
                    await Authanticate(user);
                    return RedirectToAction("choosecontent", "admin");
                }
                else
                    ModelState.AddModelError("", "Некорректные данные");
            }
            return View(model);
        }

        private async Task Authanticate(User user) {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Route("logout")]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
    }
}
