using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;
using WebShop_API.DTOs.Category;
using WebShop_API.DTOs.Product;

namespace WebShopUnitTests.Services
{
    public class CategoryServiceTests
    {
        private readonly CategoryService m_categoryService;
        private readonly Mock<ICategoryRepository> m_categoryRepositoryMock = new();
        private readonly Mock<IProductTypeRepository> m_productTypeRepositoryMock = new();
        private readonly IMapper m_mapper;

        public CategoryServiceTests()
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

            m_categoryService = new(m_categoryRepositoryMock.Object, m_productTypeRepositoryMock.Object, m_mapper);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfStaticCategoryResponses_WhenCategoryExists()
        {
            // Arrange
            List<Category> categories = new()
            {
                new Category
                {
                    CategoryID = 1,
                    CategoryName = "Category-1",
                    Products = new List<Product>()
                },
                new Category
                {
                    CategoryID = 2,
                    CategoryName = "Category-2",
                    Products = new List<Product>()
                }
            };

            m_categoryRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(categories);

            // Act
            var result = await m_categoryService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StaticCategoryResponse>>(result);
            Assert.Equal(2, result.Count);
            Assert.NotNull(result[0]);
            Assert.NotNull(result[1]);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfStaticCategoryResponses_WhenNoCategoryExists()
        {
            // Arrange
            List<Category> responses = new();

            m_categoryRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(responses);

            // Act
            var result = await m_categoryService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StaticCategoryResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetById_ShouldReturnDirectCategoryResponses_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;

            Category category = new()
            {
                CategoryID = categoryId,
                CategoryName = "Category-1",
                Products = new List<Product>()
            };

            m_categoryRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await m_categoryService.GetById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectCategoryResponse>(result);
            Assert.Equal(category.CategoryID, result.CategoryID);
            Assert.Equal(category.CategoryName, result.CategoryName);
            Assert.Equal(category.Products.Count, result.Products.Count);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryDoesNotExis()
        {
            // Arrange
            int categoryId = 1;

            m_categoryRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_categoryService.GetById(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnDirectCategoryResponse_WhenCategoryIsCreated()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Category-1"
            };

            int categoryId = 1;

            Category category = new()
            {
                CategoryID = categoryId,
                CategoryName = "Category-1",
                Products = new List<Product>()
            };

            m_categoryRepositoryMock
                .Setup(x => x.Create(It.IsAny<Category>()))
                .ReturnsAsync(category);

            m_categoryRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await m_categoryService.Create(categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectCategoryResponse>(result);
            Assert.Equal(categoryId, result.CategoryID);
            Assert.Equal(category.CategoryName, result.CategoryName);
        }

        [Fact]
        public async void Create_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Category-1"
            };

            m_categoryRepositoryMock
                .Setup(x => x.Create(It.IsAny<Category>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_categoryService.Create(categoryRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldReturnDirectCategoryResponse_WhenUpdateIsSuccessful()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Category-2"
            };

            int categoryId = 1;

            Category category = new()
            {
                CategoryID = categoryId,
                CategoryName = "Category-1",
                Products = new List<Product>()
            };


            m_categoryRepositoryMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(() => category);

            // Act
            var result = await m_categoryService.Update(categoryId, categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectCategoryResponse>(result);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            CategoryRequest categoryRequest = new()
            {
                CategoryName = "Category-1"
            };

            int productId = 1;

            m_categoryRepositoryMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_categoryService.Update(productId, categoryRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDirectCategoryResponse_WhenDeleteIsSuccesful()
        {
            // Arrange
            int categoryId = 1;

            Category category = new()
            {
                CategoryID = categoryId,
                CategoryName = "Category-1",
                Products = new List<Product>()
            };

            m_categoryRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(category);

            // Act
            var result = await m_categoryService.Delete(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectCategoryResponse>(result);
            Assert.Equal(category.CategoryID, result.CategoryID);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 1;

            m_categoryRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_categoryService.Delete(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GetAllTypes_ShouldReturnListOfStaticProductTypeResponses_WhenProductTypesExists()
        {
            // Arrange
            List<ProductType> productTypes = new()
            {
                new()
                {
                    ProductTypeID = 1,
                    ProductTypeName = "CPU",
                    Products = new List<Product>()
                },
                new()
                {
                    ProductTypeID = 2,
                    ProductTypeName = "GPU",
                    Products = new List<Product>()
                }
            };

            m_productTypeRepositoryMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( productTypes );

            // Act
            var result = await m_categoryService.GetAllTypes();

            // Assert
            Assert.NotNull( result );
            Assert.IsType<List<StaticProductTypeResponse>>( result );
            Assert.Equal( 2, result.Count );
            Assert.NotNull( result[ 0 ] );
            Assert.NotNull( result[ 1 ] );
        }

        [Fact]
        public async void GetAllTypes_ShouldReturnEmptyListOfStaticProductTypeResponses_WhenNoProductTypesExists()
        {
            // Arrange
            List<ProductType> productTypes = new();

            m_productTypeRepositoryMock
                .Setup( x => x.GetAll() )
                .ReturnsAsync( productTypes );

            // Act
            var result = await m_categoryService.GetAllTypes();

            // Assert
            Assert.NotNull( result );
            Assert.IsType<List<StaticProductTypeResponse>>( result );
            Assert.Empty( result );
        }

        [Fact]
        public async void GetType_ShouldReturnDirectProductTypeResponse_WhenProductTypeExist()
        {
            // Arrange
            int productTypeId = 1;

            ProductType productType = new()
            {
                ProductTypeID = productTypeId,
                ProductTypeName = "CPU",
                Products= new List<Product>()
            };

            m_productTypeRepositoryMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( productType );

            // Act
            var result = await m_categoryService.GetType( productTypeId );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<DirectProductTypeResponse>( result );
            Assert.Equal( productTypeId, result.ProductTypeID );
        }

        [Fact]
        public async void GetType_ShouldReturnNull_WhenProductTypeDoesNotExist()
        {
            // Arrange
            int productTypeId = 1;

            m_productTypeRepositoryMock
                .Setup( x => x.GetById( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = await m_categoryService.GetType( productTypeId );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void CreateType_ShouldReturnDirectProductTypeResponse_WhenCreatedSuccessfully()
        {
            // Arrange
            ProductTypeRequest request = new()
            {
                ProductTypeName = "CPU"
            };

            int productTypeId = 1;

            ProductType productType = new()
            {
                ProductTypeID = productTypeId,
                ProductTypeName = "CPU",
                Products = new List<Product>()
            };

            m_productTypeRepositoryMock
                .Setup( x => x.Create( It.IsAny<ProductType>() ) )
                .ReturnsAsync( productType );

            // Act
            var result = await m_categoryService.CreateType( request );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<DirectProductTypeResponse>( result );
            Assert.Equal( productTypeId, result.ProductTypeID );
        }

        [Fact]
        public async void CreateType_ShouldReturnNull_WhenProductTypeAlreadyExists()
        {
            // Arrange
            ProductTypeRequest request = new()
            {
                ProductTypeName = "CPU"
            };

            m_productTypeRepositoryMock
                .Setup( x => x.Create( It.IsAny<ProductType>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = await m_categoryService.CreateType( request );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void UpdateType_ShouldReturnDirectProductTypeResponse_WhenUpdatedSuccessfully()
        {
            // Arrange
            ProductTypeRequest request = new()
            {
                ProductTypeName = "CPU"
            };

            int productTypeId = 1;

            ProductType productType = new()
            {
                ProductTypeID = productTypeId,
                ProductTypeName = "CPU",
                Products = new List<Product>()
            };

            m_productTypeRepositoryMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ProductType>() ) )
                .ReturnsAsync( productType );

            // Act
            var result = await m_categoryService.UpdateType( productTypeId, request );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<DirectProductTypeResponse>( result );
            Assert.Equal( request.ProductTypeName, result.ProductTypeName );
        }

        [Fact]
        public async void UpdateType_ShouldReturnNull_WhenUpdateFailed()
        {
            // Arrange
            ProductTypeRequest request = new()
            {
                ProductTypeName = "CPU"
            };

            int productTypeId = 1;

            m_productTypeRepositoryMock
                .Setup( x => x.Update( It.IsAny<int>(), It.IsAny<ProductType>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = await m_categoryService.UpdateType( productTypeId, request );

            // Assert
            Assert.Null( result );
        }

        [Fact]
        public async void DeleteType_ShouldReturnDirectProductTypeResponse_WhenDeletedSuccessfully()
        {
            // Arrange
            int productTypeId = 1;

            ProductType productType = new()
            {
                ProductTypeID = productTypeId,
                ProductTypeName = "CPU",
                Products = new List<Product>()
            };

            m_productTypeRepositoryMock
                .Setup( x => x.Delete( productTypeId ) )
                .ReturnsAsync( productType );

            // Act
            var result = await m_categoryService.DeleteType( productTypeId );

            // Assert
            Assert.NotNull( result );
            Assert.IsType<DirectProductTypeResponse>( result );
            Assert.Equal( productTypeId, result.ProductTypeID );
        }

        [Fact]
        public async void DeleteType_ShouldReturnNull_WhenDeletionFailed()
        {
            // Arrange
            int productTypeId = 1;

            m_productTypeRepositoryMock
                .Setup( x => x.Delete( It.IsAny<int>() ) )
                .ReturnsAsync( () => null );

            // Act
            var result = await m_categoryService.DeleteType( productTypeId );

            // Assert
            Assert.Null( result );
        }
    }
}
