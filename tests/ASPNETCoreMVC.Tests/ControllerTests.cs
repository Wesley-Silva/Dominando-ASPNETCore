using ASPNETCoreMVC.Controllers;
using ASPNETCoreMVC.Data;
using ASPNETCoreMVC.Models;
using ASPNETCoreMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace ASPNETCoreMVC.Tests
{
    public class ControllerTests
    {
        [Fact]
        public void TestController_Index_Sucesso()
        {
            // Arrange
            var controller = new TesteController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_Index_Sucesso()
        {
            // Arrange
            // DbContext Options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new AppDbContext(options);

            ctx.Produtos.Add(new Produto { Id = 1, Nome = "Produto 1", Valor = 10m });
            ctx.Produtos.Add(new Produto { Id = 2, Nome = "Produto 2", Valor = 10m });
            ctx.Produtos.Add(new Produto { Id = 3, Nome = "Produto 3", Valor = 10m });
            ctx.SaveChanges();

            // Identity
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(m => m.Name).Returns("teste@teste.com");

            var principal = new ClaimsPrincipal(mockClaimsIdentity.Object);

            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.User).Returns(principal);

            var imgService = new Mock<IImageUploadService>();
            var controller = new ProdutosController(ctx, imgService.Object)
            {
                ControllerContext = new ControllerContext
                { 
                    HttpContext = mockContext.Object
                }
            };

            // Act
            var result = controller.Index().Result;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ProdutoController_CriarNovoProduto_Sucesso()
        {
            // Arrange
            // DbContext Options
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Contexto
            var ctx = new AppDbContext(options);

            // IFormfile
            var fileMock = new Mock<IFormFile>();            
            var fileName = "test.txt";            
            fileMock.Setup(_ => _.FileName).Returns(fileName);

            // Img Service
            var imgService = new Mock<IImageUploadService>();
            imgService.Setup(s => s.UploadArquivo(
                new ModelStateDictionary(),
                fileMock.Object, 
                It.IsAny<string>()
                )).ReturnsAsync(true);
            
            // Controller
            var controller = new ProdutosController(ctx, imgService.Object);
            
            var produto = new Produto
            {
                Id = 1,
                ImagemUpload = fileMock.Object,
                Nome = "Produto 1",
                Valor = 10m
            };

            // Act
            var result = controller.CriarNovoProduto(produto).Result;

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }
    }
}