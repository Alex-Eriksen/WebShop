//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebShop_API.Database.Entities;

//namespace WebShopUnitTests.Repository
//{
//    public class PhotoRepositoryTests
//    {
//        private readonly DbContextOptions<DatabaseContext> _options;
//        private readonly DatabaseContext _context;
//        private readonly PhotoRepository _photoRepository;

//        public PhotoRepositoryTests()
//        {
//            _options = new DbContextOptionsBuilder<DatabaseContext>()
//                .UseInMemoryDatabase(databaseName: "PhotoRepository")
//                .Options;

//            _context = new(_options);

//            _photoRepository = new(_context);
//        }

//        [Fact]
//        public async void GetById_ShouldReturnPhoto_WhenPhotoExists()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            int photoId = 1;

//            _context.Photo.Add(new()
//            {
//                PhotoID = photoId,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            });

//            await _context.SaveChangesAsync();

//            // Act
//            var result = await _photoRepository.GetById(photoId);

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<Photo>(result);
//            Assert.Equal(photoId, result.PhotoID);
//        }

//        [Fact]
//        public async void GetById_ShouldReturnNull_WhenPhotoDoesNotExist()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            // Act
//            var result = await _photoRepository.GetById(1);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async void Create_ShouldAddNewIdToPhoto_WhenSavingToDatabase()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            int expectedPhotoId = 1;

//            Photo photo = new()
//            {
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            };

//            // Act
//            var result = await _photoRepository.Create(photo);

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<Photo>(result);
//            Assert.Equal(expectedPhotoId, result.PhotoID);
//        }

//        [Fact]
//        public async void Create_ShouldFailToAddNewIdToPhoto_WhenPhotoIdAlreadyExists()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            Photo photo = new()
//            {
//                PhotoID = 1,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            };

//            // Act
//            var result = await _photoRepository.Create(photo);

//            async Task action() => await _photoRepository.Create(photo);

//            // Assert
//            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
//            Assert.Contains("An item with the same key has already been added", ex.Message);
//        }

//        [Fact]
//        public async void Update_ShouldChangeValuesOnPhoto_WhenPhotoExists()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            int photoId = 1;

//            Photo newPhoto = new()
//            {
//                PhotoID = 1,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            };

//            _context.Photo.Add(newPhoto);
//            await _context.SaveChangesAsync();

//            Photo updatePhoto = new()
//            {
//                PhotoID = photoId,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 8,
//            };

//            // Act
//            var result = await _photoRepository.Update(photoId, updatePhoto);

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<Photo>(result);
//            Assert.Equal(photoId, result.PhotoID);
//            Assert.Equal(updatePhoto.ProductID, result.ProductID);
//            Assert.Equal(updatePhoto.Bytes, result.Bytes);
//            Assert.Equal(updatePhoto.Size, result.Size);
//        }

//        [Fact]
//        public async void Update_ShouldReturnNull_WhenPhotoDoesNotExist()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();
//            int photoId = 1;

//            Photo updatePhoto = new()
//            {
//                PhotoID = 1,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            };

//            // Act
//            var result = await _photoRepository.Update(photoId, updatePhoto);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async void Delete_ShouldReturnDeletedPhoto_WhenPhotoIsDeleted()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            int photoId = 1;

//            Photo newPhoto = new()
//            {
//                PhotoID = photoId,
//                Product = new Product { },
//                Bytes = new byte[4],
//                Size = 10,
//            };

//            _context.Photo.Add(newPhoto);
//            await _context.SaveChangesAsync();

//            // Act
//            var result = await _photoRepository.Delete(photoId);
//            var photo = await _photoRepository.GetById(photoId);

//            // Assert
//            Assert.NotNull(result);
//            Assert.IsType<Photo>(result);
//            Assert.Equal(photoId, result.PhotoID);

//            Assert.Null(photo);
//        }

//        [Fact]
//        public async void Delete_ShouldReturnNull_WhenPhotoDoesNotExist()
//        {
//            // Arrange
//            await _context.Database.EnsureDeletedAsync();

//            // Act
//            var result = await _photoRepository.Delete(1);

//            // Assert
//            Assert.Null(result);
//        }
//    }
//}
