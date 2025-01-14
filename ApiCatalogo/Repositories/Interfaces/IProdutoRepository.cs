using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        IEnumerable<Produto> GetProdutos();
        Produto GetProdutoById(int id);
        Produto Create(Produto produto);
        Produto Update(Produto produto);
        Produto Delete(int id);
    }
}
