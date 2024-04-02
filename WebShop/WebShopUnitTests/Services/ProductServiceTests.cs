namespace WebShopUnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService m_productService;
        private readonly Mock<IProductRepository> m_productRepositoryMock = new();
        private readonly Mock<IPhotoRepository> m_photoRepositoryMock = new();
        private readonly IMapper m_mapper;

        public ProductServiceTests()
        {
            if (m_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new WebShop_API.Helpers.AutoMapper.AutoMapper());
                });
                IMapper mapper = mapperConfig.CreateMapper();
                m_mapper = mapper;
            }
            m_productService = new(m_productRepositoryMock.Object, m_photoRepositoryMock.Object, m_mapper);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfStaticProductResponses_WhenProductsExists()
        {
            // Arrange
            List<Product> products = new()
            {
                new Product
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
                },
                new Product
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
                }
            };

            m_productRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(products);

            // Act
            var result = await m_productService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<StaticProductResponse>>(result);
            Assert.NotNull(result[0]);
            Assert.NotNull(result[1]);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfDirectProductResponses_WhenNoProductsExists()
        {
            // Arrange
            List<Product> responses = new();

            m_productRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(responses);

            // Act
            var result = await m_productService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<StaticProductResponse>>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnDirectProductResponses_WhenProductExists()
        {
            // Arrange
            int productId = 1;

            Product product = new()
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

            m_productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = await m_productService.GetById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectProductResponse>(result);
            Assert.Equal(productId, result.ProductID);
            Assert.Equal(product.ProductName, result.ProductName);
            Assert.Equal(product.ProductPrice, result.ProductPrice);
            Assert.Equal(product.ProductQuantity, result.ProductQuantity);
            Assert.Equal(product.ProductDescription, result.ProductDescription);
            Assert.Equal(product.Manufacturer.ManufacturerID, result.Manufacturer.ManufacturerID);
            Assert.Equal(product.Category.CategoryID, result.Category.CategoryID);
            Assert.Equal(product.ProductType.ProductTypeID, result.ProductType.ProductTypeID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;

            m_productRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            // Act
            var result = await m_productService.GetById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnDirectProductResponse_WhenProductIsCreated()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                CategoryID = 1,
                ProductTypeID = 1
            };

            int productId = 1;

            Product product = new()
            {
                ProductID = productId,
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            m_productRepositoryMock
                .Setup(x => x.Create(It.IsAny<Product>()))
                .ReturnsAsync(product);

            m_productRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            var result = await m_productService.Create(productRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectProductResponse>(result);
            Assert.Equal(product.ProductID, result.ProductID);
            Assert.Equal(product.ProductName, result.ProductName);
            Assert.Equal(product.ProductPrice, result.ProductPrice);
            Assert.Equal(product.ProductQuantity, result.ProductQuantity);
            Assert.Equal(product.ProductDescription, result.ProductDescription);
            Assert.Equal(product.Category.CategoryID, result.Category.CategoryID);
            Assert.Equal(product.ProductType.ProductTypeID, result.ProductType.ProductTypeID);
        }

        [Fact]
        public async void Create_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                CategoryID = 1,
                ProductTypeID = 1
            };

            m_productRepositoryMock
                .Setup(x => x.Create(It.IsAny<Product>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_productService.Create(productRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldReturnDirectProductResponse_WhenUpdateIsSuccessful()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                CategoryID = 1,
                ProductTypeID = 1,
                ManufacturerID = 1
            };

            int productId = 1;

            Product product = new()
            {
                ProductID = productId,
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            m_productRepositoryMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<Product>() ) )
                .ReturnsAsync( product );

            m_productRepositoryMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( product );

            // Act
            var result = await m_productService.Update( productId, productRequest );

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectProductResponse>(result);
            Assert.Equal( productRequest.ProductPrice, result.ProductPrice );
            Assert.Equal( productRequest.ProductName, result.ProductName );
            Assert.Equal( productRequest.ProductQuantity, result.ProductQuantity );
            Assert.Equal( productRequest.ProductDescription, result.ProductDescription );
            Assert.Equal( productRequest.ManufacturerID, result.Manufacturer.ManufacturerID );
            Assert.Equal( productRequest.CategoryID, result.Category.CategoryID );
            Assert.Equal( productRequest.ProductTypeID, result.ProductType.ProductTypeID );
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            ProductRequest productRequest = new()
            {
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                CategoryID = 1,
                ProductTypeID = 1
            };

            int productId = 1;

            m_productRepositoryMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_productService.Update(productId, productRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDirectProductResponse_WhenDeleteIsSuccesful()
        {
            // Arrange
            int productId = 1;

            Product product = new()
            {
                ProductID = productId,
                ProductName = "Product1",
                ProductPrice = 1,
                ProductQuantity = 1,
                ProductDescription = "Description",
                Manufacturer = new Manufacturer { ManufacturerID = 1, ManufacturerName = "Manufacturer-1", Products = new List<Product>() },
                Category = new Category { CategoryID = 1, CategoryName = "Category-1", Products = new List<Product>() },
                ProductType = new ProductType { ProductTypeID = 1, ProductTypeName = "ProductType-1", Products = new List<Product>() },
                Transactions = new List<Transaction>(),
                Photos = new List<Photo>()
            };

            m_productRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(product);

            // Act
            var result = await m_productService.Delete(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectProductResponse>(result);
            Assert.Equal(product.ProductID, result.ProductID);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;

            m_productRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_productService.Delete(productId);

            // Assert
            Assert.Null(result);
        }
    }
}
