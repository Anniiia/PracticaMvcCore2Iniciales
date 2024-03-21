using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2.Extensions;
using PracticaMvcCore2.Filters;
using PracticaMvcCore2.Models;
using PracticaMvcCore2.Repositories;
using System.Data;

namespace PracticaMvcCore2.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;

        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {

            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);

        }
        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            List<Libro> librosGen = await this.repo.GetLibrosGeneroAsync(idgenero);
            return View(librosGen);

        }

        public async Task<IActionResult> Details(int idlibro)
        {

            Libro libro = await this.repo.GetDetailsLibroAsync(idlibro);

            return View(libro);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Carrito(int? idadded)
        {
            if (idadded != null)
            {
                List<Libro> librosCarrito;
                if (HttpContext.Session.GetObject<List<Libro>>("CARRITO") == null)
                {
                    librosCarrito = new List<Libro>();
                }
                else
                {
                    librosCarrito = HttpContext.Session.GetObject<List<Libro>>("CARRITO");
                }
                Libro libro = await this.repo.GetDetailsLibroAsync(idadded.Value);
                librosCarrito.Add(libro);
                HttpContext.Session.SetObject("CARRITO", librosCarrito);
            }

            return RedirectToAction("Index");
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> VerCarrito(int? ideliminar)
        {
            List<Libro> listaLibros = HttpContext.Session.GetObject<List<Libro>>("CARRITO");

            if (ideliminar != null)
            {
                Libro libro = listaLibros.Find(z => z.IdLibro == ideliminar.Value);

                listaLibros.Remove(libro);

                if (listaLibros.Count == 0)
                {

                    HttpContext.Session.Remove("CARRITO");

                }
                else
                {
                    //almacenamos de nuevo los datos en session
                    HttpContext.Session.SetObject("CARRITO", listaLibros);
                }

            }
            listaLibros = HttpContext.Session.GetObject<List<Libro>>("CARRITO");

            return View(listaLibros);
        }
        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }

        [AuthorizeUsuarios]

        public async Task<IActionResult> Pedidos()
        {
            int idUsuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);

            List<VistaPedidos> pedidos = this.repo.GetVistaPedidoUsuario(idUsuario);

            return View(pedidos);

        }

        [AuthorizeUsuarios]

        public async Task<IActionResult> FinalizarCompra()
        {
            List<Libro> carrito = HttpContext.Session.GetObject<List<Libro>>("CARRITO");
            if (carrito != null)
            {
                int idUser = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
                this.repo.InsertarPedido(carrito,idUser);
                HttpContext.Session.Remove("CARRITO");
                return RedirectToAction("Pedidos");
            }
            else
            {
                ViewData["MENSAJE"] = "No puedes comprar nada";
            }
            return View("Pedidos");

        }
    }
}
