using ApiCatalogo.Data;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome param)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(param.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(param.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(),param.PageNumber, param.PageSize);

            return categoriasFiltradas;
        }

        public async Task<PagedList<Categoria>> GetCategoriasParametersAsync(CategoriasParameters parameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas,
                parameters.PageNumber, parameters.PageSize);

            return resultado;
        }
    }
}
