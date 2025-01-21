using ApiCatalogo.Data;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        /*
        [HttpGet("produtos")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            //return _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToList();
            return await _context.Categorias.Include(p => p.Produtos)
            .Where(c => c.CategoriaId <= 5)
            .AsNoTracking().ToListAsync();
        }
        */

        [HttpGet]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetCategoria(int id)
        {

            var categoria = _uof.CategoriaRepository.GetById(c=> c.CategoriaId == id);

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

            _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterCategoria",
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
            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogError($"Categoria com id={id} não encontrada");
                return NotFound($"Categoria com id={id} não encontrada");
            }

            _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();
            return Ok(categoria);
        }
    }
}