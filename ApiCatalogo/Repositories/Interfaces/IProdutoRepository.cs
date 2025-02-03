using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutosParameters(ProdutosParameters parameters);
        Task<PagedList<Produto>> GetProdutosParametersAsync(ProdutosParameters parameters);
        Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco param);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
