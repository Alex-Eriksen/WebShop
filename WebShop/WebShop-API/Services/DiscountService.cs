using WebShop_API.Database.Entities;

namespace WebShop_API.Services
{
    public interface IDiscountService
    {
        Task<List<StaticDiscountResponse>> GetAll();
        Task<DirectDiscountResponse> GetById( int discountId );
        Task<DirectDiscountResponse> Create( DiscountRequest request );
        Task<DirectDiscountResponse> Update( int discountId, DiscountRequest request );
        Task<DirectDiscountResponse> Delete( int discountId );
    }

    /// <summary>
    /// DiscountService is used to transfere data to and from DiscountRepository and DiscountController.
    /// </summary>
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository m_discountRepository;
        private readonly IMapper m_mapper;

        /// <summary>
        /// Constructor of DiscountService
        /// </summary>
        /// <param name="discountRepository"></param>
        /// <param name="mapper"></param>
        public DiscountService( IDiscountRepository discountRepository, IMapper mapper )
        {
            m_discountRepository = discountRepository;
            m_mapper = mapper;
        }

        /// <summary>
        /// Creates a request of DiscountRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>discount or null</returns>
        public async Task<DirectDiscountResponse> Create( DiscountRequest request )
        {
            Discount discount = await m_discountRepository.Create( m_mapper.Map<Discount>( request ) );
            if(discount != null)
            {
                return m_mapper.Map<DirectDiscountResponse>( discount );
            }

            return null;
        }

        /// <summary>
        /// Deletes a disocunt by its id.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns>discount or null</returns>
        public async Task<DirectDiscountResponse> Delete( int discountId )
        {
            Discount discount = await m_discountRepository.Delete( discountId );
            if(discount != null)
            {
                return m_mapper.Map<DirectDiscountResponse>( discount );
            }

            return null;
        }

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns>discount or null</returns>
        public async Task<List<StaticDiscountResponse>> GetAll()
        {
            List<Discount> discounts = await m_discountRepository.GetAll();
            if(discounts != null)
            {
                return discounts.Select( discount => m_mapper.Map<StaticDiscountResponse>( discount ) ).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets a disocunt by its id.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns>discount or null</returns>
        public async Task<DirectDiscountResponse> GetById( int discountId )
        {
            Discount discount = await m_discountRepository.GetById( discountId );
            if(discount != null)
            {
                DirectDiscountResponse mappedDiscount = m_mapper.Map<DirectDiscountResponse>( discount );
                List<StaticProductResponse> mappedProducts = mappedDiscount.Products;
                foreach (var product in discount.Products)
                {
                    if(product.Photos.Count == 0)
                    {
                        continue;
                    }

                    var x = mappedProducts.Find( x => x.ProductID == product.ProductID );
                    if(x != null)
                    {
                        x.ImageName = product.Photos.First().ImageName;
                    }
                }
                return mappedDiscount;
            }

            return null;
        }

        /// <summary>
        /// Updates a discount.
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="request"></param>
        /// <returns>discount or null</returns>
        public async Task<DirectDiscountResponse> Update( int discountId, DiscountRequest request )
        {
            Discount discount = await m_discountRepository.Update( discountId, m_mapper.Map<Discount>( request ) );
            if(discount != null)
            {
                return m_mapper.Map<DirectDiscountResponse>( discount );
            }

            return null;
        }
    }
}
