namespace WebShopUnitTests.Services
{
    public class DiscountServiceTests
    {
        private readonly DiscountService m_discountService;
        private readonly Mock<IDiscountRepository> m_discountRepositoryMock = new();
        private readonly IMapper m_mapper;

        public DiscountServiceTests()
        {
            if (m_mapper == null)
            {
                var mapperConfig = new MapperConfiguration(mc => {
                    mc.AddProfile(new WebShop_API.Helpers.AutoMapper.AutoMapper());
                });
                IMapper mapper = mapperConfig.CreateMapper();
                m_mapper = mapper;
            }
            m_discountService = new(m_discountRepositoryMock.Object, m_mapper);
        }

        [Fact]
        public async void GetAll_ShouldReturnListOfDirectDiscountResponses_WhenDiscountExsits()
        {
            List<Discount> discounts = new();
            List<StaticDiscountResponse> responses = new();

            discounts.Add(new()
            {
                DiscountID = 1,
                Name = "Navn",
                Description = "Hej med dig",
                DiscountPercent = 20,
                Transactions = new List<Transaction>()
            });

            discounts.Add(new()
            {
                DiscountID = 2,
                Name = "Navn2",
                Description = "Hej med dig din mor",
                DiscountPercent = 23,
                Transactions = new List<Transaction>()
            });

            m_discountRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(discounts);

            var result = await m_discountService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.IsType<List<StaticDiscountResponse>>(result);
            Assert.NotNull(result [ 0 ] );
            Assert.NotNull(result [ 1 ] );
        }

        [Fact]
        public async void GetAll_ShouldReturnEmptyListOfDirectDiscountResponses_WhenNoDiscountExists()
        {
            List<Discount> responses = new();

            m_discountRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(responses);

            var result = await m_discountService.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.IsType<List<StaticDiscountResponse>>(result);
        }

        [Fact]
        public async void GetById_ShouldReturnDirectDiscountResponse_WhenCustomerExists()
        {
            int discountId = 1;

            Discount discount = new()
            {
                DiscountID = discountId,
                Name = "Discount",
                Description = "Hej med dig",
                DiscountPercent = 20,
                Transactions = new List<Transaction>()
            };

            m_discountRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(discount);

            var result = await m_discountService.GetById(discountId);

            Assert.NotNull(result);
            Assert.IsType<DirectDiscountResponse>(result);
            Assert.Equal(discount.DiscountID, result.DiscountID);
        }

        [Fact]
        public async void GetById_ShouldReturnNull_WhenDiscountDoseNotExists()
        {
            int discountId = 1;

            m_discountRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await m_discountService.GetById(discountId);

            Assert.Null(result);
        }

        [Fact]
        public async void Create_ShouldReturnDirectDiscountResponse_WhenCreateIsSuccess()
        {
            DiscountRequest newDiscount = new()
            {
                Name = "Hej med dig",
                Description = "Din Mor",
                DiscountPercent = 99
            };

            int discountId = 1;

            Discount discount = new()
            {
                DiscountID = discountId,
                Name = "Navn2",
                Description = "Hej med dig din mor",
                DiscountPercent = 23,
                Transactions = new List<Transaction>()
            };

            m_discountRepositoryMock.Setup(x => x.Create(It.IsAny<Discount>())).ReturnsAsync(discount);

            var result = await m_discountService.Create(newDiscount);

            Assert.NotNull(result);
            Assert.IsType<DirectDiscountResponse>(result);
            Assert.Equal(discount.DiscountID, result.DiscountID);
            Assert.Equal(discount.Name, result.Name);
            Assert.Equal(discount.DiscountPercent, result.DiscountPercent);
            Assert.Equal(discount.DiscountPercent, result.DiscountPercent);
        }

        [Fact]
        public async void Create_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            DiscountRequest newDiscount = new()
            {
                Name = "Hej med dig",
                Description = "Din Mor",
                DiscountPercent = 99
            };

            m_discountRepositoryMock.Setup(x => x.Create(It.IsAny<Discount>())).ReturnsAsync(() => null);

            var result = await m_discountService.Create(newDiscount);

            Assert.Null(result);
        }

        [Fact]
        public async void Update_ShouldReturnDirectdiscountResponse_WhenUpdateIsSuccess()
        {
            DiscountRequest discountRequest = new()
            {
                Name = "Hej med dig",
                Description = "Din Mor",
                DiscountPercent = 99
            };

            int discountId = 1;

            Discount discount = new()
            {
                DiscountID = discountId,
                Name = "Navn2",
                Description = "Hej med dig din mor",
                DiscountPercent = 23,
                Transactions = new List<Transaction>()
            };

            m_discountRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Discount>())).ReturnsAsync(discount);

            var result = await m_discountService.Update(discountId, discountRequest);

            Assert.NotNull(result);
            Assert.IsType<DirectDiscountResponse>(result);
            Assert.Equal(discount.DiscountID, result.DiscountID);
            Assert.Equal(discount.Name, result.Name);
            Assert.Equal(discount.Description, result.Description);
            Assert.Equal(discount.DiscountPercent, result.DiscountPercent);
        }

        [Fact]
        public async void Update_ShouldReturnNull_WhenDiscountDoseNotExists()
        {
            DiscountRequest discountRequest = new()
            {
                Name = "Hej med dig",
                Description = "Din Mor",
                DiscountPercent = 99
            };

            int discountId = 1;

            m_discountRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Discount>())).ReturnsAsync(() => null);

            var result = await m_discountService.Update(discountId, discountRequest);

            Assert.Null(result);
        }

        [Fact]
        public async void Delete_shouldReturnDirectDiscountResponse_WhenDeleteIsSuccess()
        {
            int discountId = 1;

            Discount discount = new()
            {
                DiscountID = discountId,
                Name = "Navn2",
                Description = "Hej med dig din mor",
                DiscountPercent = 23,
                Transactions = new List<Transaction>()
            };

            m_discountRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(discount);

            var result = await m_discountService.Delete(discountId);

            Assert.NotNull(result);
            Assert.IsType<DirectDiscountResponse>(result);
            Assert.Equal(discountId, result.DiscountID);
        }

        [Fact]
        public async void Delete_ShouldReturnNull_WhenDiscountDoesNotExists()
        {
            int discountId = 1;

            m_discountRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await m_discountService.Delete(discountId);

            Assert.Null(result);
        }
    }
}
