using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2.Models;
using PracticaMvcCore2.Repositories;

namespace PracticaMvcCore2.ViewComponents
{
    public class MenuGeneros: ViewComponent
    {
        private RepositoryLibros repo;

        public MenuGeneros(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();

            return View(generos);
        }
    }
}
