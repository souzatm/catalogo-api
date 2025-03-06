using ApiCatalogo.Data;
using ApiCatalogo.DTOs;
using ApiCatalogo.Filters;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixedwindow")]
    [Produces("application/json")] // Padroniza o retorno para application/json
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CategoriasController(ILogger<CategoriasController> logger, IUnitOfWork uof, IMapper mapper)
        {
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
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

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetParameters([FromQuery]  CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasParametersAsync(categoriasParameters);
            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetProdutosFilterNome([FromQuery] CategoriasFiltroNome param)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(param);
            return ObterCategorias(categorias);
        }

        /// <summary>
        /// Obtem uma lista de objetos Categoria
        /// </summary>
        /// <returns>Uma lista de objetos Categoria</returns>
        [HttpGet]
        [Authorize]
        //[ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }

        /// <summary>
        /// Obtem uma categoria por seu Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Objeto Categoria</returns>
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        //[ProducesResponseType(StatusCodes.Status404NotFound)] NotFound no Swagger caso o recurso não seja encontrado
        public async Task<ActionResult<CategoriaDTO>> GetCategoriaAsync(int id)
        {

            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c=> c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada.");
                return NotFound("Dados inválidos.");
            }

            var categoriasDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoria);
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/categorias
        ///     {
        ///         "categoriaId": 1,
        ///         "nome": "categoria",
        ///         "imagemUrl": "http://teste.com/imagem.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaDto"></param>
        /// <returns>O objeto Categoria incluído</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            var novaCategoria =  _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(novaCategoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAtualizadaDto = _mapper.Map<CategoriaDTO>(categoriaAtualizada);

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetByIdAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogError($"Categoria com id={id} não encontrada");
                return NotFound($"Categoria com id={id} não encontrada");
            };

            var produtoDeletado = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var produtoDeletadoDto = _mapper.Map<CategoriaDTO>(produtoDeletado);

            return Ok(produtoDeletadoDto);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDto);
        }
    }
}