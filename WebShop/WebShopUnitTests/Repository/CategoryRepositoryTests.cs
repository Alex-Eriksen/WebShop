using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShopUnitTests.Repository
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> m_options;
        private readonly DatabaseContext m_context;
        private readonly CategoryRepository m_categoryRepository;

        public CategoryRepositoryTests()
        {
            m_options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CategoryRepository")
                .Options;

            m_context = new(m_options);

            m_categoryRepository = new(m_context);
        }

        [Fact] 
        public async void GetAll_ShouldReturnListOfCategories_WhenCategoriesExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            m_context.Category.Add(new()
            {
                CategoryID = 1,
                CategoryName = "Category",
                Products = new List<Product>()
            });

            m_context.Category.Add(new()
            {
                CategoryID = 2,
                CategoryName = "Category2",
                Products = new List<Product>()
            });

            await m_context.SaveChangesAsync();

            // Act
            var result = await m_categoryRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfCategories_WhenNoCategoriesExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            // Act
            var result = await m_categoryRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetById_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            m_context.Category.Add(new()
            {
                CategoryID = categoryId,
                CategoryName = "CategoryName",
                Products = new List<Product>()
            });

            await m_context.SaveChangesAsync();

            // Act
            var result = await m_categoryRepository.GetById(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.CategoryID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            // Act
            var result = await m_categoryRepository.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnNewIdToCategory_WhenSavingToDatabase()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            Category category = new()
            {
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            // Act
            var result = await m_categoryRepository.Create(category);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(expectedNewId, result.CategoryID);
        }

        [Fact]
        public async void Create_ShouldFailtoAddNewCategory_WhenCategoryAlreadyExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            // Act
            var result = await m_categoryRepository.Create(category);

            async Task action() => await m_categoryRepository.Create(category);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void Update_ShouldChangeValuesOnCategory_WhenCategoryExists()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            Category newCategory = new()
            {
                CategoryID = 1,
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            m_context.Category.Add(newCategory);

            await m_context.SaveChangesAsync();

            Category updateCategory = new()
            {
                CategoryID = categoryId,
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            // Act
            var result = await m_categoryRepository.Update(categoryId, updateCategory);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.CategoryID);
            Assert.Equal(updateCategory.CategoryName, result.CategoryName);
            Assert.Equal(updateCategory.Products, result.Products);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            Category updateCategory = new()
            {
                CategoryID = categoryId,
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            // Act
            var result = await m_categoryRepository.Update(categoryId, updateCategory);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDeletedCategory_WhenCategoryIsDeleted()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            Category newCategory = new()
            {
                CategoryID = categoryId,
                CategoryName = "CategoryName",
                Products = new List<Product>()
            };

            m_context.Category.Add(newCategory);
            await m_context.SaveChangesAsync();

            // Act
            var result = await m_categoryRepository.Delete(categoryId);
            var category = await m_categoryRepository.GetById(categoryId);


            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.CategoryID);

            Assert.Null(category);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            await m_context.Database.EnsureDeletedAsync();

            // Act
            var result = await m_categoryRepository.Delete(1);

            // Assert
            Assert.Null(result);
        }
    }
}
