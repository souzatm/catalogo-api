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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            //return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
            return await _context.Categorias.Include(p => p.Produtos)
            .Where(c => c.CategoriaId <= 5)
            .AsNoTracking().ToListAsync();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Categoria> GetById(int id)
        {

            var categoria = _context.Categorias
            .FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada.");
                return NotFound("Dados inválidos.");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult<Categoria> Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);


        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias
            .FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogError($"Categoria com id={id} não encontrada");
                return NotFound($"Categoria com id={id} não encontrada");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}