using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using X.PagedList;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutosParameters(ProdutosParameters parameters);
        Task<IPagedList<Produto>> GetProdutosParametersAsync(ProdutosParameters parameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco param);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    }
}
