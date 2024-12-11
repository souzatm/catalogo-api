using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "Values('Coca-Cola Diet', 'Refrigerante de Cola 350ml',5.45, 'cocacola.jpg', 50, GETDATE(),1 )");

            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "Values('Lanche de Atum', 'Lanche de atum com maionese',8.50, 'atum.jpg', 10, GETDATE(),2 )");

            mb.Sql("Insert into Produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "Values('Pudim', 'Pudim de leite condensado 100g',6.75, 'pudim.jpg', 20, GETDATE(),3 )");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos");
        }
    }
}
