using ApiCatalogo.Data;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco param)
        {
            var produtos = GetAll().AsQueryable();

            if (param.Preco.HasValue && !string.IsNullOrEmpty(param.PrecoCriterio))
            {
                if (param.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > param.Preco.Value)
                        .OrderBy(p => p.Preco);
                }
                else if (param.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < param.Preco.Value)
                        .OrderBy(p => p.Preco); 
                }
                else if (param.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == param.Preco.Value)
                        .OrderBy(param => param.Preco);
                }
            }

            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, param.PageNumber, param.PageSize);

            return produtosFiltrados;
            
        }

        /*public IEnumerable<Produto> GetProdutosParameters(ProdutosParameters parameters)
        {
            return GetAll()
                .OrderBy(p => p.Nome)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }*/

        public PagedList<Produto> GetProdutosParameters(ProdutosParameters parameters)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produto>
                .ToPagedList(produtos, parameters.PageNumber, parameters.PageSize);

            return produtosOrdenados;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
