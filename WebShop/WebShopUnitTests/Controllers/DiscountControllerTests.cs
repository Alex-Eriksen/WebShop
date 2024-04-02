using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop_API.Database.Entities;
using WebShop_API.DTOs.Discount;
using WebShop_API.DTOs.Transaction;

namespace WebShopUnitTests.Controllers
{
	public class DiscountControllerTests
	{
		private readonly DiscountController m_discountController;
		private readonly Mock<IDiscountService> m_discountServiceMock = new();

		public DiscountControllerTests()
		{
			m_discountController = new(m_discountServiceMock.Object);
		}

		[Fact]
		public async void GetAll_ShouldReturnStatusCode200_WhenDiscountsExists()
		{
			// Arrange
			List<StaticDiscountResponse> discounts = new()
			{
				new StaticDiscountResponse
				{
					DiscountID = 1,
					Name = "Discount-1",
					Description = "Description-1",
					DiscountPercent = 20,
				},
				new StaticDiscountResponse
				{
					DiscountID = 2,
					Name = "Discount-2",
					Description = "Description-2",
					DiscountPercent = 25,
				}
			};


			m_discountServiceMock
				.Setup(x => x.GetAll())
				.ReturnsAsync(discounts);

			// Act
			var result = (IStatusCodeActionResult) await m_discountController.GetAll();

			// Assert
			Assert.Equal(200, result.StatusCode);
		}

		[Fact]
		public async void GetAll_ShouldReturnStatusCode204_WhenNoDiscountsExist()
		{
			// Arrange
			List<StaticDiscountResponse> discounts = new();

			m_discountServiceMock
				.Setup(x => x.GetAll())
				.ReturnsAsync(discounts);

			// Act
			var result = (IStatusCodeActionResult) await m_discountController.GetAll();

			// Assert
			Assert.Equal(204, result.StatusCode);
		}

		[Fact]
		public async void GetAll_ShouldReturnStatusCode500_WhenServiceReturnsNull()
		{
			// Arrange
			m_discountServiceMock
				.Setup(x => x.GetAll())
				.ReturnsAsync(() => throw new Exception("This is an exception"));

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.GetAll();

			// Assert
			Assert.Equal(500, result.StatusCode);
		}

		[Fact]
		public async void GetById_ShouldReturnStatusCode200_WhenDíscountExists()
		{
			// Arrange
			int discountId = 1;

			DirectDiscountResponse discount = new()
			{
				DiscountID = discountId,
				Name = "Discount-1",
				Description = "Description-1",
				DiscountPercent = 20,
				Transactions = new List<StaticTransactionResponse>()
			};

			m_discountServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.ReturnsAsync(discount);

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.GetById(discountId);

			// Assert
			Assert.Equal(200, result.StatusCode);
		}

		[Fact]
		public async void GetById_ShouldReturnStatusCode404_WhenDiscountDoesNotExist()
		{
			// Arrange
			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.ReturnsAsync(() => null);

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.GetById(discountId);

			// Assert
			Assert.Equal(404, result.StatusCode);
		}

		[Fact]
		public async void GetById_ShouldReturnStatusCode500_WhenExceptionIsRaised()
		{
			// Arrange
			m_discountServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.ReturnsAsync(() => throw new Exception("This is an exception"));

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.GetById(1);
			// Assert
			Assert.Equal(500, result.StatusCode);
		}

		[Fact]
		public async void Create_ShouldReturnStatusCode200_WhenDiscountIsCreatedSucessfully()
		{
			// Arrange
			DiscountRequest request = new()
			{
				Name = "Discount",
				Description = "Description",
				DiscountPercent = 20
			};

			int discountId = 1;

			DirectDiscountResponse response = new()
			{
				DiscountID = discountId,
				Description = "Description",
				DiscountPercent = 20
			};

			m_discountServiceMock
				.Setup(x => x.Create(It.IsAny<DiscountRequest>()))
				.ReturnsAsync(response);

			// Act
			var result = (IStatusCodeActionResult) await m_discountController.Create(request);

			// Assert
			Assert.Equal(200, result.StatusCode);
		}

		[Fact]
		public async void Create_ShouldReturnStatusCode400_WhenRequestIsNull()
		{
			// Arrange
			DiscountRequest request = new();

			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.Create(It.IsAny<DiscountRequest>()))
				.ReturnsAsync(() => null);

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.Create(request);

			// Assert
			Assert.Equal(400, result.StatusCode);
		}

		[Fact]
		public async void Create_ShouldReturnStatusCode500_WhenExceptionIsRaised()
		{
			// Arrange
			DiscountRequest request = new()
			{
				Name = "Discount",
				Description = "Description",
				DiscountPercent = 20
			};

			m_discountServiceMock
				.Setup(x => x.Create(It.IsAny<DiscountRequest>()))
				.ReturnsAsync(() => throw new Exception("This is an exception"));

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.Create(request);

			// Assert
			Assert.Equal(500, result.StatusCode);
		}

		[Fact]
		public async void Update_ShouldReturnStatusCode200_WhenDiscountIsUpdatedSuccessfully()
		{
			// Arrange
			DiscountRequest request = new()
			{
				Name = "Discount",
				Description = "Description",
				DiscountPercent = 20
			};

			int discountId = 1;

			DirectDiscountResponse response = new()
			{
				DiscountID = discountId,
				Description = "Description",
				DiscountPercent = 20
			};

			m_discountServiceMock
				.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<DiscountRequest>()))
				.ReturnsAsync(response);

			// Act
			var result = (IStatusCodeActionResult )await m_discountController.Update(discountId, request);

			// Assert
			Assert.Equal(200, result.StatusCode);
		}

		[Fact]
		public async void Update_ShouldReturnStatusCode404_WhenDiscountDoesNotExist()
		{
			// Arrange
			DiscountRequest request = new()
			{
				Name = "Discount",
				Description = "Description",
				DiscountPercent = 20
			};

			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<DiscountRequest>()))
				.ReturnsAsync(() => null);

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.Update(discountId, request);

			// Assert
			Assert.Equal(404, result.StatusCode);
		}

		[Fact]
		public async void Update_ShouldReturnStatusCode500_WhenExceptionIsRaised()
		{
			// Arrange
			DiscountRequest request = new()
			{
				Name = "Discount",
				Description = "Description",
				DiscountPercent = 20
			};

			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<DiscountRequest>()))
				.ReturnsAsync(() => throw new Exception("This is an exception"));

			// Act
			var result = (IStatusCodeActionResult)await m_discountController.Update(discountId, request);

			// Assert
			Assert.Equal(500, result.StatusCode);
		}

		[Fact]
		public async void Delete_ShouldReturnStatuscCode200_WhenDiscountIsDeleted()
		{
			// Arrange
			int discountId = 1;

			DirectDiscountResponse response = new()
			{
				DiscountID = discountId,
				Description = "Description",
				DiscountPercent = 20
			};

			m_discountServiceMock
				.Setup(x => x.Delete(It.IsAny<int>()))
				.ReturnsAsync(response);
			
			// Act
			var result = (IStatusCodeActionResult) await m_discountController.Delete(discountId);

			// Assert
			Assert.Equal(200, result.StatusCode);
		}

		[Fact]
		public async void Delete_ShouldReturnStatusCode404_WhenDiscountDoesNotExist()
		{
			// Arrange
			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.Delete(It.IsAny<int>()))
				.ReturnsAsync(() => null);

			// Act
			var result = (IStatusCodeActionResult) await m_discountController.Delete(discountId);

			// Assert
			Assert.Equal(404, result.StatusCode);
		}

		[Fact]
		public async void Delete_ShouldReturnStatusCode500_WhenExceptionIsRaised()
		{
			// Arrange
			int discountId = 1;

			m_discountServiceMock
				.Setup(x => x.Delete(It.IsAny<int>()))
				.ReturnsAsync(() => throw new Exception("This is an exception"));

			// Act
			var result = (IStatusCodeActionResult) await m_discountController.Delete(discountId);

			// Assert
			Assert.Equal(500, result.StatusCode);
		}
	}
}
