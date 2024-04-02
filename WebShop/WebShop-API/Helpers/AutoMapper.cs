namespace WebShop_API.Helpers.AutoMapper
{
    public class AutoMapper : Profile
    {
        /// <summary>
        /// Using CreateMap() to make custom maps from an object to another object.
        /// </summary>
        public AutoMapper()
        {
            CreateMap<Account, DirectAccountResponse>();
            CreateMap<Account, StaticAccountResponse>();
            CreateMap<AccountRequest, Account>();

            CreateMap<Country, DirectCountryResponse>();
            CreateMap<Country, StaticCountryResponse>();
            CreateMap<CountryRequest, Country>();

            CreateMap<ICollection<Payment>, List<Payment>>();
            CreateMap<ICollection<Order>, List<Order>>();

            CreateMap<Customer, DirectCustomerResponse>();
            CreateMap<Customer, StaticCustomerResponse>();
            CreateMap<CustomerRequest, Customer>();

            CreateMap<Category, DirectCategoryResponse>();
            CreateMap<Category, StaticCategoryResponse>();
            CreateMap<CategoryRequest, Category>();

            CreateMap<Payment, DirectPaymentResponse>();
            CreateMap<Payment, StaticPaymentResponse>();
            CreateMap<PaymentRequest, Payment>();

            CreateMap<Photo, DirectPhotoResponse>();
            CreateMap<Photo, StaticPhotoResponse>();
            CreateMap<PhotoRequest, Photo>();

            CreateMap<Product, DirectProductResponse>();
            CreateMap<Product, StaticProductResponse>();
            CreateMap<ProductRequest, Product>();

            CreateMap<Manufacturer, DirectManufacturerResponse>();
            CreateMap<Manufacturer, StaticManufacturerResponse>();
            CreateMap<ManufacturerRequest, Manufacturer>();

            CreateMap<ProductType, DirectProductTypeResponse>();
            CreateMap<ProductType, StaticProductTypeResponse>();
            CreateMap<ProductTypeRequest, ProductType>();           
            
            CreateMap<Discount, DirectDiscountResponse>();
            CreateMap<Discount, StaticDiscountResponse>();
            CreateMap<DiscountRequest, Discount>();

			CreateMap<Transaction, DirectTransactionResponse>();
			CreateMap<Transaction, StaticTransactionResponse>();
			CreateMap<TransactionRequest, Transaction>();

			CreateMap<Order, DirectOrderResponse>();
			CreateMap<Order, StaticOrderResponse>();
			CreateMap<OrderRequest, Order>();

			CreateMap<AuthenticationResponse, StaticRefreshTokenResponse>();
        }
    }
}
