using ApiCatalogo.Models;

namespace ApiCatalogo.Repository
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategorias(); //IEnumerable fornece abstração mais genérica do que o "List(impõe coleção)"
        Categoria GetCategoriaById(int id);
        Categoria Create(Categoria categoria);
        Categoria Update(Categoria categoria);
        Categoria Delete(int id);
    }
}
