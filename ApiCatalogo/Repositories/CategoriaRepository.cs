using ApiCatalogo.Data;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ApiCatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome param)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(param.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(param.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(),param.PageNumber, param.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(param.PageNumber, param.PageSize);

            return categoriasFiltradas;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasParametersAsync(CategoriasParameters parameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas,parameters.PageNumber, parameters.PageSize);

            var resultado = await categoriasOrdenadas.ToPagedListAsync(parameters.PageNumber, parameters.PageSize);

            return resultado;
        }
    }
}
