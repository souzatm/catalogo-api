using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasParametersAsync(CategoriasParameters parameters);
        Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome param);
    }
}
