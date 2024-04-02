using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using WebShop_API.Controllers;

namespace WebShopUnitTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductController m_productController;
        private readonly Mock<IProductService> m_productServiceMock = new Mock<IProductService>();
        private readonly Mock<ICategoryService> m_categoryServiceMock = new Mock<ICategoryService>();
        private readonly Mock<IManufacturerService> m_manufacturerServiceMock = new Mock<IManufacturerService>();

        public ProductControllerTests()
        {
            m_productController = new( m_productServiceMock.Object, m_categoryServiceMock.Object, m_manufacturerServiceMock.Object );
        }

        #region Product Tests

        [Fact]
        public async void GetAll_ShouldReturnStatusCode200_WhenProductsExists()
        {
            // Arrange
            List<StaticProductResponse> responses = new()
            {
                new()
                {
                    ProductID = 1,
                    CategoryID = 1,
                    ManufacturerID = 1,
                    ProductDescription = "Description-1",
                    ProductName = "Name-1",
                    ProductPrice = 20.3,
                    ProductQuantity = 7,
                    ProductTypeID = 1,
                    ReleaseDate = DateTime.UtcNow
                },
                new()
                {
                    ProductID = 2,
                    CategoryID = 1,
                    ManufacturerID = 1,
                    ProductDescription = "Description-2",
                    ProductName = "Name-2",
                    ProductPrice = 15.3,
                    ProductQuantity = 45,
                    ProductTypeID = 1,
                    ReleaseDate = DateTime.UtcNow
                }
            };

            m_productServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAll();

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode204_WhenProductsDoesNotExist()
        {
            // Arrange
            List<StaticProductResponse> responses = new();

            m_productServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAll();

            // Assert
            Assert.Equal( 204, result.StatusCode );
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_productServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAll();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetAll_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_productServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAll();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode200_WhenProductExists()
        {
            // Arrange
            int productId = 1;

            DirectProductResponse response = new();

            m_productServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetById( productId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;

            m_productServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetById( productId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productId = 1;

            m_productServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetById( productId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode200_WhenSuccessfullyCreated()
        {
            // Arrange
            ProductRequest request = new();

            DirectProductResponse response = new();

            m_productServiceMock
                .Setup( x => x.Create( It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Create( request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            ProductRequest request = new();

            m_productServiceMock
                .Setup( x => x.Create( It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Create( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            ProductRequest request = new();

            m_productServiceMock
                .Setup( x => x.Create( It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Create( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode200_WhenSuccessfullyUpdated()
        {
            // Arrange
            int productId = 1;
            ProductRequest request = new();
            DirectProductResponse response = new();

            m_productServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Update( productId, request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;
            ProductRequest request = new();

            m_productServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Update( productId, request );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void Update_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productId = 1;
            ProductRequest request = new();

            m_productServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ProductRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Update( productId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode200_WhenSuccessfullyDeleted()
        {
            // Arrange
            int productId = 1;

            DirectProductResponse response = new();

            m_productServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Delete( productId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 1;

            m_productServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Delete( productId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productId = 1;

            m_productServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.Delete( productId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        #endregion

        #region Category Tests

        [Fact]
        public async void GetAllCategories_ShouldReturnStatusCode200_WhenCategoriesExists()
        {
            // Arrange
            List<StaticCategoryResponse> responses = new()
            {
                new()
                {
                },
                new()
                {
                }
            };

            m_categoryServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllCategories();

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnStatusCode204_WhenCategoriesDoesNotExist()
        {
            // Arrange
            List<StaticCategoryResponse> responses = new();

            m_categoryServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllCategories();

            // Assert
            Assert.Equal( 204, result.StatusCode );
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_categoryServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllCategories();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetAllCategories_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_categoryServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllCategories();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetCategoryById_ShouldReturnStatusCode200_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;

            DirectCategoryResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetCategoryById( categoryId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetCategoryById_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;

            m_categoryServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetCategoryById( categoryId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetCategoryById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int categoryId = 1;

            m_categoryServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetCategoryById( categoryId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateCategory_ShouldReturnStatusCode200_WhenSuccessfullyCreated()
        {
            // Arrange
            CategoryRequest request = new();

            DirectCategoryResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.Create( It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateCategory( request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void CreateCategory_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            CategoryRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.Create( It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateCategory( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateCategory_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            CategoryRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.Create( It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateCategory( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void UpdateCategory_ShouldReturnStatusCode200_WhenSuccessfullyUpdated()
        {
            // Arrange
            int categoryId = 1;
            CategoryRequest request = new();
            DirectCategoryResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateCategory( categoryId, request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void UpdateCategory_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;
            CategoryRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateCategory( categoryId, request );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void UpdateCategory_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int categoryId = 1;
            CategoryRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<CategoryRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateCategory( categoryId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void DeleteCategory_ShouldReturnStatusCode200_WhenSuccessfullyDeleted()
        {
            // Arrange
            int categoryId = 1;

            DirectCategoryResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteCategory( categoryId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void DeleteCategory_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;

            m_categoryServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteCategory( categoryId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void DeleteCategory_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int categoryId = 1;

            m_categoryServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteCategory( categoryId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        #endregion

        #region Product Type Tests

        [Fact]
        public async void GetAllTypes_ShouldReturnStatusCode200_WhenProductTypesExists()
        {
            // Arrange
            List<StaticProductTypeResponse> responses = new()
            {
                new()
                {
                },
                new()
                {
                }
            };

            m_categoryServiceMock
                .Setup( x => x.GetAllTypes() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllTypes();

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetAllTypes_ShouldReturnStatusCode204_WhenProductTypesDoesNotExist()
        {
            // Arrange
            List<StaticProductTypeResponse> responses = new();

            m_categoryServiceMock
                .Setup( x => x.GetAllTypes() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllTypes();

            // Assert
            Assert.Equal( 204, result.StatusCode );
        }

        [Fact]
        public async void GetAllTypes_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_categoryServiceMock
                .Setup( x => x.GetAllTypes() )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllTypes();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetAllTypes_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_categoryServiceMock
                .Setup( x => x.GetAllTypes() )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllTypes();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetType_ShouldReturnStatusCode200_WhenProductTypeExists()
        {
            // Arrange
            int productTypeId = 1;

            DirectProductTypeResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.GetType( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetType( productTypeId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetType_ShouldReturnStatusCode404_WhenProductTypeDoesNotExist()
        {
            // Arrange
            int productTypeId = 1;

            m_categoryServiceMock
                .Setup( x => x.GetType( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetType( productTypeId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetType_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productTypeId = 1;

            m_categoryServiceMock
                .Setup( x => x.GetType( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetType( productTypeId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateType_ShouldReturnStatusCode200_WhenSuccessfullyCreated()
        {
            // Arrange
            ProductTypeRequest request = new();

            DirectProductTypeResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.CreateType( It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateType( request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void CreateType_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            ProductTypeRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.CreateType( It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateType( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateType_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            ProductTypeRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.CreateType( It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateType( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void UpdateType_ShouldReturnStatusCode200_WhenSuccessfullyUpdated()
        {
            // Arrange
            int productTypeId = 1;
            ProductTypeRequest request = new();
            DirectProductTypeResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.UpdateType( It.IsAny<int>(), It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateType( productTypeId, request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void UpdateType_ShouldReturnStatusCode404_WhenProductTypeDoesNotExist()
        {
            // Arrange
            int productTypeId = 1;
            ProductTypeRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.UpdateType( It.IsAny<int>(), It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateType( productTypeId, request );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void UpdateType_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productTypeId = 1;
            ProductTypeRequest request = new();

            m_categoryServiceMock
                .Setup( x => x.UpdateType( It.IsAny<int>(), It.IsAny<ProductTypeRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateType( productTypeId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void DeleteType_ShouldReturnStatusCode200_WhenSuccessfullyDeleted()
        {
            // Arrange
            int productTypeId = 1;

            DirectProductTypeResponse response = new();

            m_categoryServiceMock
                .Setup( x => x.DeleteType( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteType( productTypeId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void DeleteType_ShouldReturnStatusCode404_WhenProductTypeDoesNotExist()
        {
            // Arrange
            int productTypeId = 1;

            m_categoryServiceMock
                .Setup( x => x.DeleteType( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteType( productTypeId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void DeleteType_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int productTypeId = 1;

            m_categoryServiceMock
                .Setup( x => x.DeleteType( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteType( productTypeId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        #endregion

        #region Manufacturer Tests

        [Fact]
        public async void GetAllManufacturers_ShouldReturnStatusCode200_WhenManufacturersExists()
        {
            // Arrange
            List<StaticManufacturerResponse> responses = new()
            {
                new()
                {
                },
                new()
                {
                }
            };

            m_manufacturerServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllManufacturers();

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetAllManufacturers_ShouldReturnStatusCode204_WhenManufacturersDoesNotExist()
        {
            // Arrange
            List<StaticManufacturerResponse> responses = new();

            m_manufacturerServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( responses );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllManufacturers();

            // Assert
            Assert.Equal( 204, result.StatusCode );
        }

        [Fact]
        public async void GetAllManufacturers_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            m_manufacturerServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllManufacturers();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetAllManufacturers_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            m_manufacturerServiceMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetAllManufacturers();

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void GetManufacturer_ShouldReturnStatusCode200_WhenManufacturerExists()
        {
            // Arrange
            int manufacturerId = 1;

            DirectManufacturerResponse response = new();

            m_manufacturerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void GetManufacturer_ShouldReturnStatusCode404_WhenManufacturerDoesNotExist()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void GetManufacturer_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerServiceMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.GetManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateManufacturer_ShouldReturnStatusCode200_WhenSuccessfullyCreated()
        {
            // Arrange
            ManufacturerRequest request = new();

            DirectManufacturerResponse response = new();

            m_manufacturerServiceMock
                .Setup( x => x.Create( It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateManufacturer( request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void CreateManufacturer_ShouldReturnStatusCode500_WhenServiceReturnsNull()
        {
            // Arrange
            ManufacturerRequest request = new();

            m_manufacturerServiceMock
                .Setup( x => x.Create( It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateManufacturer( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void CreateManufacturer_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            ManufacturerRequest request = new();

            m_manufacturerServiceMock
                .Setup( x => x.Create( It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.CreateManufacturer( request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void UpdateManufacturer_ShouldReturnStatusCode200_WhenSuccessfullyUpdated()
        {
            // Arrange
            int manufacturerId = 1;
            ManufacturerRequest request = new();
            DirectManufacturerResponse response = new();

            m_manufacturerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateManufacturer( manufacturerId, request );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void UpdateManufacturer_ShouldReturnStatusCode404_WhenManufacturerDoesNotExist()
        {
            // Arrange
            int manufacturerId = 1;
            ManufacturerRequest request = new();

            m_manufacturerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateManufacturer( manufacturerId, request );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void UpdateManufacturer_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int manufacturerId = 1;
            ManufacturerRequest request = new();

            m_manufacturerServiceMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ManufacturerRequest>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.UpdateManufacturer( manufacturerId, request );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        [Fact]
        public async void DeleteManufacturer_ShouldReturnStatusCode200_WhenSuccessfullyDeleted()
        {
            // Arrange
            int manufacturerId = 1;

            DirectManufacturerResponse response = new();

            m_manufacturerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( response );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 200, result.StatusCode );
        }

        [Fact]
        public async void DeleteManufacturer_ShouldReturnStatusCode404_WhenManufacturerDoesNotExist()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 404, result.StatusCode );
        }

        [Fact]
        public async void DeleteManufacturer_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerServiceMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => throw new Exception() );

            // Act
            var result = (IStatusCodeActionResult) await m_productController.DeleteManufacturer( manufacturerId );

            // Assert
            Assert.Equal( 500, result.StatusCode );
        }

        #endregion
    }
}
