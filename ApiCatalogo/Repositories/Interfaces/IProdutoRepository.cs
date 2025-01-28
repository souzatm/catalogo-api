using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutosParameters(ProdutosParameters parameters);
        PagedList<Produto> GetProdutosParameters(ProdutosParameters parameters);
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
    }
}
