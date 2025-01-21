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
    public class ProdutosController : ControllerBase
    {
        private readonly ILogger<ProdutosController> _logger;
        private readonly IUnitOfWork _uof;

        public ProdutosController(ILogger<ProdutosController> logger, IUnitOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("categoria/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null)
            {
                return NotFound();
            }
            return Ok(produtos);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            return _uof.ProdutoRepository.GetAll().ToList();
        }


        [HttpGet("{id:int}", Name = "ObterProduto")] //Nomeia a rota como ObterProduto
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

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

            _uof.ProdutoRepository.Create(produto);
            _uof.Commit();
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

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            // var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                _logger.LogError($"Produto com id={id} não encontrado");
                return NotFound($"Produto com id={id} não encontrado");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok(produto);
        }
    }
}
