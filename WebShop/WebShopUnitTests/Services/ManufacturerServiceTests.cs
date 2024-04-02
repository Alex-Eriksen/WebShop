using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;

namespace WebShopUnitTests.Services
{
    public class ManufacturerServiceTests
    {
        private readonly ManufacturerService m_manufacturerService;
        private readonly Mock<IManufacturerRepository> m_manufacturerRepositoryMock = new();
        private readonly IMapper m_mapper;

        public ManufacturerServiceTests()
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
            m_manufacturerService = new(m_manufacturerRepositoryMock.Object, m_mapper);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfStaticManufacturerResponses_WheManufacturerExists()
        {
            // Arrange
            List<Manufacturer> manufacturers = new()
            {
                new Manufacturer
                {
                    ManufacturerID = 1,
                    ManufacturerName = "AMD",
                    Products = new List<Product>()
                },
                new Manufacturer
                {
                    ManufacturerID = 2,
                    ManufacturerName = "NVIDIA",
                    Products = new List<Product>()
                }
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(manufacturers);

            // Act
            var result = await m_manufacturerService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StaticManufacturerResponse>>(result);
            Assert.Equal(2, result.Count);
            Assert.NotNull(result[0]);
            Assert.NotNull(result[1]);
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfStaticManufacturerResponses_WhenNoManufacturerExists()
        {
            // Arrange
            List<Manufacturer> responses = new();

            m_manufacturerRepositoryMock
                .Setup(x => x.GetAll())
                .ReturnsAsync(responses);

            // Act
            var result = await m_manufacturerService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<StaticManufacturerResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetById_ShouldReturnDirectManufacturerResponses_WhenManufacturerExists()
        {
            // Arrange
            int manufacturerId = 1;

            Manufacturer manufacturer = new()
            {
                ManufacturerID = manufacturerId,
                ManufacturerName = "AMD",
                Products = new List<Product>()
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(manufacturer);

            // Act
            var result = await m_manufacturerService.GetById(manufacturerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectManufacturerResponse>(result);
            Assert.Equal(manufacturer.ManufacturerID, result.ManufacturerID);
            Assert.Equal(manufacturer.ManufacturerName, result.ManufacturerName);
            Assert.Equal(manufacturer.Products.Count, result.Products.Count);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenManufacturerDoesNotExis()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_manufacturerService.GetById(manufacturerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnDirectManufacturerResponse_WhenManufacturerIsCreated()
        {
            // Arrange
            ManufacturerRequest manufacturerRequest = new()
            {
                ManufacturerName = "AMD"
            };

            int manufacturerId = 1;

            Manufacturer manufacturer = new()
            {
                ManufacturerID = 1,
                ManufacturerName = "AMD",
                Products = new List<Product>()
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.Create(It.IsAny<Manufacturer>()))
                .ReturnsAsync(manufacturer);

            m_manufacturerRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .ReturnsAsync(manufacturer);

            // Act
            var result = await m_manufacturerService.Create(manufacturerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectManufacturerResponse>(result);
            Assert.Equal(manufacturerId, result.ManufacturerID);
            Assert.Equal(manufacturer.ManufacturerName, result.ManufacturerName);
        }

        [Fact]
        public async void Create_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            ManufacturerRequest manufacturerRequest = new()
            {
                ManufacturerName = "AMD"
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.Create(It.IsAny<Manufacturer>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_manufacturerService.Create(manufacturerRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldReturnDirectManufacturerResponse_WhenUpdateIsSuccessful()
        {
            // Arrange
            ManufacturerRequest manufacturerRequest = new()
            {
                ManufacturerName = "AMD"
            };

            int manufacturerId = 1;

            Manufacturer manufacturer = new()
            {
                ManufacturerID = manufacturerId,
                ManufacturerName = "NVIDIA",
                Products = new List<Product>()
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Manufacturer>()))
                .ReturnsAsync(manufacturer);

            // Act
            var result = await m_manufacturerService.Update(manufacturerId, manufacturerRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectManufacturerResponse>(result);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenManufacturerDoesNotExist()
        {
            // Arrange
            ManufacturerRequest manufacturerRequest = new()
            {
                ManufacturerName = "AMD"
            };

            int manufacturerId = 1;

            m_manufacturerRepositoryMock
                .Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Manufacturer>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_manufacturerService.Update(manufacturerId, manufacturerRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Delete_ShouldReturnDirectManufacturerResponse_WhenDeleteIsSuccesful()
        {
            // Arrange
            int manufacturerId = 1;

            Manufacturer manufacturer = new()
            {
                ManufacturerID = manufacturerId,
                ManufacturerName = "NVIDIA",
                Products = new List<Product>()
            };

            m_manufacturerRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(manufacturer);

            // Act
            var result = await m_manufacturerService.Delete(manufacturerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<DirectManufacturerResponse>(result);
            Assert.Equal(manufacturer.ManufacturerName, result.ManufacturerName);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenManufacturerDoesNotExist()
        {
            // Arrange
            int manufacturerId = 1;

            m_manufacturerRepositoryMock
                .Setup(x => x.Delete(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await m_manufacturerService.Delete(manufacturerId);

            // Assert
            Assert.Null(result);
        }


    }
}
