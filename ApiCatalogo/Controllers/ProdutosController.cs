﻿using ApiCatalogo.Data;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.AsNoTracking().ToList();

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
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos
                .FirstOrDefault(p => p.ProdutoId == id);

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
        public ActionResult Post(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            //Devolve a rota ObterProduto
            return new CreatedAtRouteResult("ObterProduto",
                new {id = produto.ProdutoId}, produto);
        }
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest($"Dados inválidos");
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

            if(produto is null)
            {
                return NotFound("Produto não localizado.");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
