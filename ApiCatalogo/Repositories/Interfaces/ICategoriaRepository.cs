using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriasParameters(CategoriasParameters parameters);
        PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome param);
    }
}
