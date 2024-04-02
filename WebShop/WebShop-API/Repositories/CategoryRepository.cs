namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update, Delete.
    /// </summary>
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById( int categoryId );
        Task<Category> Create( Category request );
        Task<Category> Update( int categoryId, Category request );
        Task<Category> Delete( int categoryId );
    }

    /// <summary>
    /// Using ICategoryRepository interface. 
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for CategoryRepository.
        /// </summary>
        /// <param name="context"></param>
        public CategoryRepository(DatabaseContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Creates an Category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>CategoryID</returns>
        public async Task<Category> Create( Category request )
        {
            m_context.Category.Add(request);
            await m_context.SaveChangesAsync();
            return await GetById(request.CategoryID);
        }

        /// <summary>
        /// Deletes an Category.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>category</returns>
        public async Task<Category> Delete( int categoryId )
        {
            Category category = await GetById(categoryId);
            if(category != null)
            {
                m_context.Category.Remove(category);
                await m_context.SaveChangesAsync();
            }

            return category;
        }

        /// <summary>
        /// Gets all Categories.
        /// </summary>
        /// <returns>Category</returns>
        public async Task<List<Category>> GetAll()
        {
            return await m_context.Category.Include(x => x.Products).ToListAsync();
        }

        /// <summary>
        /// Gets Category by its Id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>categoryId</returns>
        public async Task<Category> GetById( int categoryId )
        {
            return await m_context.Category
                .Include(x => x.Products)
                .ThenInclude(x => x.Photos)
                .FirstOrDefaultAsync(x => x.CategoryID == categoryId);
        }

        /// <summary>
        /// Updates Category.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="request"></param>
        /// <returns>category</returns>
        public async Task<Category> Update( int categoryId, Category request )
        {
            Category category = await GetById(categoryId);
            if (category != null)
            {
                category.CategoryName = request.CategoryName;
                category.Modified_At = DateTime.UtcNow;
                
                await m_context.SaveChangesAsync();
            }

            return category;
        }
    }
}
