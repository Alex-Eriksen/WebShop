namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IProductTypeRepository
    {
        Task<List<ProductType>> GetAll();
        Task<ProductType> GetById( int productTypeId );
        Task<ProductType> Create( ProductType request );
        Task<ProductType> Update( int productTypeId, ProductType request );
        Task<ProductType> Delete( int productTypeId );
    }

    /// <summary>
    /// Using IProductTypeRepository interface.
    /// </summary>
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for ProductTypeRepository.
        /// </summary>
        /// <param name="context"></param>
        public ProductTypeRepository( DatabaseContext context )
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a ProductType.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>request</returns>
        public async Task<ProductType> Create( ProductType request )
        {
            m_context.ProductType.Add( request );
            await m_context.SaveChangesAsync();
            return await GetById( request.ProductTypeID );
        }

        /// <summary>
        /// Deletes a ProductType.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>productType</returns>
        public async Task<ProductType> Delete( int productTypeId )
        {
            ProductType productType = await GetById( productTypeId );
            if(productType != null)
            {
                m_context.ProductType.Remove( productType );
                await m_context.SaveChangesAsync();
            }
            return productType;
        }

        /// <summary>
        /// Gets all ProductTypes.
        /// </summary>
        /// <returns>List of ProductTypes</returns>
        public async Task<List<ProductType>> GetAll()
        {
            return await m_context.ProductType.Include( x => x.Products ).ToListAsync();
        }

        /// <summary>
        /// Gets a ProductType by its Id.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        public async Task<ProductType> GetById( int productTypeId )
        {
            return await m_context.ProductType
                .Include( x => x.Products )
                .FirstOrDefaultAsync( x => x.ProductTypeID == productTypeId );
        }

        /// <summary>
        /// Updates a ProductType.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="request"></param>
        /// <returns>productType</returns>
        public async Task<ProductType> Update( int productTypeId, ProductType request )
        {
            ProductType productType = await GetById( productTypeId );
            if(productType != null)
            {
                productType.ProductTypeName = request.ProductTypeName;

                await m_context.SaveChangesAsync();
            }
            return productType;
        }
    }
}
