using ApiCatalogo.Data;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                //return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
                return await _context.Categorias.Include(p => p.Produtos)
                .Where(c => c.CategoriaId <= 5)
                .AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro na operação.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            try
            {
                var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

                if (categorias is null)
                {
                    return NotFound("Categorias não encontradas.");
                }

                return categorias;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro na operação.");
            }
            
        }
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> GetByIdAsync(int id)
        { 

            try
            {
                var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Dados inválidos.");
                }

                return categoria;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro na operação.");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> PostAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }
                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(categoria);

            }
            catch(Exception)
            {
                throw new Exception("Ocorreu um erro na operação.");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não localizada.");
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch(Exception)
            {
                throw new Exception("Ocorreu um erro na operação.");
            }
        }
    }
}