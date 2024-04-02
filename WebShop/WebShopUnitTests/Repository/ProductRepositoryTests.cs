using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;

namespace WebShopUnitTests.Repository
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly ProductRepository _productRepository;

        public ProductRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ProductRepository")
                .Options;

            _context = new(_options);

            _productRepository = new(_context);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfProducts_WhenProductsExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Product.Add(new()
            {
                ProductID = 1,
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>()},
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            });

            _context.Product.Add(new()
            {
                ProductID = 2,
                ProductName = "Product-2",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 2, ManufacturerName = "Manufacturer-2", Products = new List<Product>() },
                Category = new Category { CategoryID = 2, CategoryName = "Category-2", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 2, ProductTypeName = "ProductType-2", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfProducts_WhenNoProductsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetbyId_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            _context.Product.Add(new()
            {
                ProductID = productId,
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.GetById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.ProductID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenProductDoesNotExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldAddNewIdToProduct_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Product product = new()
            {
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            // Act
            var result = await _productRepository.Create(product);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(expectedId, result.ProductID);
        }

        [Fact]
        public async void Create_ShouldFailToAddNewProduct_WhenProductAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            // Act
            var result = await _productRepository.Create(product);

            async Task action() => await _productRepository.Create(product);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void Update_ShouldChangeValuesOnProduct_WhenProductExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            Product newProduct = new()
            {
                ProductID = 1,
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            Product updateProduct = new()
            {
                ProductID = productId,
                ProductName = "Product-2",
                ProductPrice = 2,
                ProductQuantity = 2,
                ProductDescription = "Description-2",
                Manufacturer = new Manufacturer { ManufacturerID = 2, ManufacturerName = "Manufacturer-2", Products = new List<Product>() },
                Category = new Category { CategoryID = 2, CategoryName = "Category-2", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 2, ProductTypeName = "ProductType-2" },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            // Act
            var result = await _productRepository.Update(productId, updateProduct);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.ProductID);
            Assert.Equal(updateProduct.ProductName, result.ProductName);
            Assert.Equal(updateProduct.ProductPrice, result.ProductPrice);
            Assert.Equal(updateProduct.ProductQuantity, result.ProductQuantity);
            Assert.Equal(updateProduct.ProductDescription, result.ProductDescription);
            Assert.Equal(updateProduct.ManufacturerID, result.ManufacturerID);
            Assert.Equal(updateProduct.CategoryID, result.CategoryID);
            Assert.Equal(updateProduct.ProductTypeID, result.ProductTypeID);
            Assert.Equal(updateProduct.Transactions, result.Transactions);
            Assert.Equal(updateProduct.Photos, result.Photos);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            Product updateProduct = new()
            {
                ProductID = 1,
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            // Act
            var result = await _productRepository.Update(productId, updateProduct);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public async void Delete_ShouldReturnDeletedProduct_WhenProductIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            Product newProduct = new()
            {
                ProductID = 1,
                ProductName = "Product-1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            // Act
            var result = await _productRepository.Delete(productId);
            var product = await _productRepository.GetById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.ProductID);

            Assert.Null(product);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _productRepository.Delete(1);

            // Assert
            Assert.Null(result);
        }
    }
}
