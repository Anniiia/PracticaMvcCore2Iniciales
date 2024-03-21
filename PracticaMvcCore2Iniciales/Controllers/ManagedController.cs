using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2.Repositories;
using System.Numerics;
using System.Security.Claims;
using PracticaMvcCore2.Models;

namespace PracticaMvcCore2.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(string email, string password)
        {

            Usuario user = await this.repo.LogInUsuarioAsync(email, password);

            if (user != null)
            {
                //SEGURIDAD
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                //CREAMOS EL CLAIM PARA EL NOMBRE (APELLIDO)
                Claim claimName =
                    new Claim(ClaimTypes.Name, user.Nombre);
                identity.AddClaim(claimName);
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString());
                identity.AddClaim(claimId);
                Claim claimIdUsuario =
                    new Claim("IdUsuario", user.IdUsuario.ToString());
                identity.AddClaim(claimIdUsuario);
                Claim claimEmail =
                    new Claim("Email", user.Email);
                identity.AddClaim(claimEmail);
                Claim claimApellidos =
                    new Claim("Apellidos", user.Apellidos);
                identity.AddClaim(claimApellidos);
                Claim claimPass =
                    new Claim("Pass", user.Password);
                identity.AddClaim(claimPass);
                Claim claimFoto =
                    new Claim("Foto", user.Foto);
                identity.AddClaim(claimFoto);



                //COMO POR AHORA NO VOY A UTILIZAR NI SE UTILIZAR ROLES
                //NO LO INCLUIMOS
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);
                //LO VAMOS A LLEVAR A UNA VISTA QUE TODAVIA NO TENEMOS
                //QUE SERA EL PERFIL DEL EMPLEADO
                return RedirectToAction("PerfilUsuario", "Libros");

            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Libros");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }

    }
}
