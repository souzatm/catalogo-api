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

        public async Task<PagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco param)
        {
            var produtos = await GetAllAsync();

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

            var produtosFiltrados = PagedList<Produto>
                .ToPagedList(produtos.AsQueryable(), param.PageNumber, param.PageSize);

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

        public async Task<PagedList<Produto>> GetProdutosParametersAsync(ProdutosParameters parameters)
        {
            var produtos = await GetAllAsync();
               
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

            var resultado = PagedList<Produto>
                .ToPagedList(produtosOrdenados, parameters.PageNumber, parameters.PageSize);

            return resultado;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();

            var resultado = produtos.Where(c => c.CategoriaId == id);

            return resultado;
        }
    }
}
