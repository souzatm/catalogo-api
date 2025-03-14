using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Update_Return_OkResult()
        {
            //Arrange
            var prodId = 1;

            var produtoAlteraro = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "nome",
                Descricao = "descricao",
                Preco = 10.99m,
                ImagemUrl = "imagem.jpg",
                CategoriaId = 1
            };

            //Act
            var result = await _controller.Put(prodId, produtoAlteraro) as ActionResult<ProdutoDTO>;

            //Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task PutProduto_Update_Return_BadRequest()
        {
            var prodId = 1000;

            var produtoAlterado = new ProdutoDTO
            {
                ProdutoId = 20,
                Nome = "nomea",
                Descricao = "descricaoa",
                ImagemUrl = "imagema.jpg",
                CategoriaId = 2
            };

            //Act
            var data = await _controller.Put(prodId, produtoAlterado);

            //Assert
            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
        }
    }
}
