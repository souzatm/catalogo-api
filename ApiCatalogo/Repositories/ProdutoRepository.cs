using ApiCatalogo.Data;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> GetProdutos()
        {
            return _context.Produtos.ToList();
        }

        public Produto GetProdutoById(int id)
        {
            return _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
        }

        public Produto Create(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto Update(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return produto;
        }

        public Produto Delete(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }
            _context.Remove(id);
            _context.SaveChanges();
            return produto;
        }
    }
}
