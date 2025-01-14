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
        private readonly IProdutoRepository _repository;
        private readonly ILogger<ProdutosController> _logger;

        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            return _repository.GetProdutos().ToList();
        }


        [HttpGet("{id:int}", Name = "ObterProduto")] //Nomeia a rota como ObterProduto
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _repository.GetProdutoById(id);

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

            _repository.Create(produto);
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

            _repository.Update(produto);
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _repository.GetProdutoById(id);
            // var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                _logger.LogError($"Produto com id={id} não encontrado");
                return NotFound($"Produto com id={id} não encontrado");
            }

            _repository.Delete(id);
            return Ok(produto);
        }
    }
}
