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

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome param)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(param.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(param.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias,param.PageNumber, param.PageSize);

            return categoriasFiltradas;
        }

        public PagedList<Categoria> GetCategoriasParameters(CategoriasParameters parameters)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias,
                parameters.PageNumber, parameters.PageSize);

            return categoriasOrdenadas;
        }
    }
}
