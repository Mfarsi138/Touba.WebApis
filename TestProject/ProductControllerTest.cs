using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Touba.WebApis.Api.Controllers;
using Touba.WebApis.DataLayer.Models.Product;
using Touba.WebApis.IdentityServer.DataLayer;
using Xunit;

namespace TestProject
{
    public class ProductControllerTest
    {
        ProductController _controller;
        private readonly DataContext _context;

        public ProductControllerTest()
        {

            _controller = new ProductController(_context);

        }



        [Fact]
        public async Task GetAllTest()
        {
            //Arrange
            //Act
            var result = await _controller.GetProductTest();
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<ProductTest>>(list.Value);



            var listProduct = list.Value as List<ProductTest>;

            Assert.Equal(5, listProduct.Count);
        }

        [Theory]
        [InlineData("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200", "ab2bd817-98cd-4cf3-a80a-53ea0cd9c111")]
        public async Task GetProductTestByIdTest(string guid1, string guid2)
        {
            //Arrange
            var validGuid = new Guid(guid1).ToString();
            var invalidGuid = new Guid(guid2).ToString();

            //Act
            var notFoundResult = await _controller.GetProductTest(invalidGuid);
            var okResult = await _controller.GetProductTest(validGuid);

            //Assert
            Assert.IsType<NotFoundResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);


            //Now we need to check the value of the result for the ok object result.
            var item = okResult.Result as OkObjectResult;

            //We Expect to return a single Product
            Assert.IsType<ProductTest>(item.Value);

            //Now, let us check the value itself.
            var productItem = item.Value as ProductTest;
            Assert.Equal(validGuid, productItem.Id);
            Assert.Equal("Managing Oneself", productItem.Description);
        }


        [Fact]
        public async Task Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = await _controller.GetProductTest();
            
            // Assert
            var items = Assert.IsType<List<ProductTest>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }
    }
}
