namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IDiscountRepository 
    {
        Task<List<Discount>> GetAll();
        Task<Discount> GetById( int discountId );
        Task<Discount> Create( Discount request );
        Task<Discount> Update( int discountId, Discount request );
        Task<Discount> Delete( int discountId );
    }

    /// <summary>
    /// Using IDiscountRepository interface.
    /// </summary>
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for DiscountRepository.
        /// </summary>
        /// <param name="context"></param>
        public DiscountRepository( DatabaseContext context )
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a discount.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>DiscountID</returns>
        public async Task<Discount> Create( Discount request )
        {
            m_context.Discount.Add( request );
            await m_context.SaveChangesAsync();
            return await GetById( request.DiscountID );
        }

        /// <summary>
        /// Deletes a discount.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns>discount</returns>
        public async Task<Discount> Delete( int discountId )
        {
            Discount discount = await GetById( discountId );
            if(discount != null)
            {
                m_context.Discount.Remove( discount );
                await m_context.SaveChangesAsync();
            }

            return discount;
        }

        /// <summary>
        /// Gets all discounts.
        /// </summary>
        /// <returns>List of Discount</returns>
        public async Task<List<Discount>> GetAll()
        {
            return await m_context.Discount
                .Include( x => x.Transactions )
                .Include( x => x.Products )
                .ToListAsync();
        }

        /// <summary>
        /// Gets a discount by its Id.
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns>discountId</returns>
        public async Task<Discount> GetById( int discountId )
        {
            return await m_context.Discount
                .Include( x => x.Transactions )
                .Include( x => x.Products )
                .ThenInclude( x => x.Photos )
                .FirstOrDefaultAsync( x => x.DiscountID == discountId );
        }

        /// <summary>
        /// Updates a discount.
        /// </summary>
        /// <param name="discountId"></param>
        /// <param name="request"></param>
        /// <returns>discount</returns>
        public async Task<Discount> Update( int discountId, Discount request )
        {
            Discount discount = await GetById( discountId );
            if (discount != null)
            {
                discount.Name = request.Name;
                discount.Description = request.Description;
                discount.DiscountPercent = request.DiscountPercent;
                discount.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }

            return discount;
        }
    }
}
