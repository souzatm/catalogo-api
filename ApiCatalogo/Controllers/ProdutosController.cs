using ApiCatalogo.Data;
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

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produtos = await _context.Produtos.AsNoTracking().ToListAsync();

                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados.");
                }

                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpGet("{id:int}", Name="ObterProduto")] //Nomeia a rota como ObterProduto
        public async Task<ActionResult<Produto>> GetByIdAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado.");
                }
                return produto;
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação.");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new {id = produto.ProdutoId}, produto);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest($"Dados inválidos");
            }

            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var produto = await _context.Produtos
                .FirstOrDefaultAsync(p => p.ProdutoId == id);
            // var produto = _context.Produtos.Find(id);

            if(produto is null)
            {
                return NotFound("Produto não localizado.");
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);
        }
    }
}
