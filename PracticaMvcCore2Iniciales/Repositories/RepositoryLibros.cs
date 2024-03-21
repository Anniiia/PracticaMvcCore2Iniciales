using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2.Data;
using PracticaMvcCore2.Models;
using System.Numerics;

namespace PracticaMvcCore2.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosGeneroAsync(int idgenero)
        {
            return await this.context.Libros.Where(z => z.IdGenero == idgenero).ToListAsync();
        }

        public async Task<Libro> GetDetailsLibroAsync(int idLibro)
        {
            Libro libro = await this.context.Libros.Where(z => z.IdLibro == idLibro).FirstOrDefaultAsync();

            return libro;
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<List<VistaPedidos>> GetPedidosAsync(int idUsuario)
        {
            return await this.context.VistaPedidos.Where(z => z.IdUsuario == idUsuario).ToListAsync();
        }

        public async Task<Usuario> Login(string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.Where(z => z.Email == email && z.Password == password).FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<Usuario> DetallesUsuarioAsync(int idUsuario)
        {
            Usuario usuario = await this.context.Usuarios.Where(z => z.IdUsuario == idUsuario).FirstOrDefaultAsync();

            return usuario;
        }

        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        {
            Usuario user = await this.context.Usuarios.Where(z => z.Email == email && z.Password == password).FirstOrDefaultAsync();

            return user;
        }

        public List<VistaPedidos> GetVistaPedidoUsuario(int idUsuario)
        {
            var consulta = from datos in context.VistaPedidos where datos.IdUsuario == idUsuario select datos;

            return consulta.ToList();
        }

        public void InsertarPedido( List<Libro> libros, int idusuario)
        {
            int maxPedido = this.context.Pedidos.Max(x => x.IdPedido) + 1;
            int maxFactura = this.context.Pedidos.Max(x => x.IdFactura) + 1;
            foreach (Libro libro in libros)
            {
                Pedido pedido = new Pedido
                {
                    IdPedido = maxPedido,
                    IdFactura = maxFactura,
                    Fecha = DateTime.Now,
                    IdLibro = libro.IdLibro,
                    IdUsuario = idusuario,
                    Cantidad = 1
                };
                maxPedido++;
                this.context.Pedidos.Add(pedido);
            }
             this.context.SaveChangesAsync();
        }

        
    }
}
