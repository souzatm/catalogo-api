using ApiCatalogo.Data;
using ApiCatalogo.Filters;
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
        private readonly ILogger _logger;

        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            _logger.LogInformation("============== GET api/categorias/produtos ==============");
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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            _logger.LogInformation("============== GET api/categorias ==============");

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
            _logger.LogInformation($"============== GET api/categorias/id = {id} ==============");
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
            _logger.LogInformation("============== POST api/categoria ==============");

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Categoria categoria)
        {
            _logger.LogInformation("============== PUT api/categorias/ ==============");
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
            _logger.LogInformation($"============== GET api/categorias/id = {id} ==============");
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