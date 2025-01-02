using ApiCatalogo.Data;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(AppDbContext context, ILogger<ProdutosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }


        [HttpGet("{id:int}", Name = "ObterProduto")] //Nomeia a rota como ObterProduto
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _context.Produtos
            .FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
            {
                _logger.LogError($"Produto com id={id} não encontrado.");
                return NotFound($"Produto com id={id} não encontrado.");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogError("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            _context.Produtos.Add(produto);
            _context.SaveChangesAsync();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogError($"Produto com id={id} não encontrado");
                return BadRequest($"Produto com id={id} não encontrado");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos
                .FirstOrDefault(p => p.ProdutoId == id);
            // var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                _logger.LogError($"Produto com id={id} não encontrado");
                return NotFound($"Produto com id={id} não encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
