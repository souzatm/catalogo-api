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
        //private readonly IRepository<Produto> _repository;
        private readonly ILogger<ProdutosController> _logger;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(ILogger<ProdutosController> logger, 
            IProdutoRepository produtoRepository) //IRepository<Produto> repository
        {
            //_repository = repository;
            _logger = logger;
            _produtoRepository = produtoRepository;
        }

        [HttpGet("categoria/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);

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
            return _produtoRepository.GetAll().ToList();
        }


        [HttpGet("{id:int}", Name = "ObterProduto")] //Nomeia a rota como ObterProduto
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _produtoRepository.GetById(p => p.ProdutoId == id);

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

            _produtoRepository.Create(produto);
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

            _produtoRepository.Update(produto);
            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _produtoRepository.GetById(p => p.ProdutoId == id);
            // var produto = _context.Produtos.Find(id);

            if (produto is null)
            {
                _logger.LogError($"Produto com id={id} não encontrado");
                return NotFound($"Produto com id={id} não encontrado");
            }

            _produtoRepository.Delete(produto);
            return Ok(produto);
        }
    }
}
