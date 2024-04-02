namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById( int productId );
        Task<Product> Create( Product request );
        Task<Product> Update( int productId, Product request );
        Task<Product> Delete( int productId );
        Task<Product> UpdateQuantity(int productId, int quantity );
    }

    /// <summary>
    /// Using IProductRepository interface.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for ProductRepository
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(DatabaseContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a Product
        /// </summary>
        /// <param name="request"></param>
        /// <returnsrequest></returns>
        public async Task<Product> Create( Product request )
        {
            m_context.Product.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.ProductID);
        }

        /// <summary>
        /// Deletes a Product.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>product</returns>
        public async Task<Product> Delete( int productId )
        {
            Product product = await GetById(productId);
            if(product != null)
            {
                m_context.Product.Remove(product);
                await m_context.SaveChangesAsync();
            }

            return product;
        }

        /// <summary>
        /// Gets all Products.
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<List<Product>> GetAll()
        {
            return await m_context.Product
                .Include(x => x.Manufacturer)
                .Include(x => x.Category)
                .Include(x => x.ProductType)
                .Include(x => x.Transactions)
                .Include(x => x.Photos)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a Product by its Id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product</returns>
        public async Task<Product> GetById( int productId )
        {
            return await m_context.Product
                .Include(x => x.Manufacturer)
                .Include(x => x.Category)
                .Include(x => x.ProductType)
                .Include(x => x.Transactions)
                .Include(x => x.Photos)
                .Include(x => x.Discount)
                .FirstOrDefaultAsync(x => x.ProductID == productId);
        }

        /// <summary>
        /// Updates a Product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns>product</returns>
        public async Task<Product> Update( int productId, Product request )
        {
            Product product = await GetById(productId);
            if(product != null)
            {
                product.ProductPrice = request.ProductPrice;
                product.ProductName = request.ProductName;
                product.ProductQuantity = request.ProductQuantity;
                product.ProductDescription = request.ProductDescription;
                product.ManufacturerID = request.ManufacturerID;
                product.CategoryID = request.CategoryID;
                product.DiscountID = request.DiscountID;
                product.ReleaseDate = request.ReleaseDate;
                product.ProductTypeID = request.ProductTypeID;

                if(request.DiscountID != null)
                {
                    product.DiscountID = request.DiscountID;
                }

                await m_context.SaveChangesAsync();
            }

            return product;
        }

        public async Task<Product> UpdateQuantity(int productId, int quantity)
        {
            Product product = await GetById(productId);
            if(product != null)
            {
                product.ProductQuantity = product.ProductQuantity - quantity;
                await m_context.SaveChangesAsync();
            }
            return product;
        }
    }
}
