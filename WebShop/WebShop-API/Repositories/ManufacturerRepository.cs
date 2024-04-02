namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IManufacturerRepository
    {
        Task<List<Manufacturer>> GetAll();
        Task<Manufacturer> GetById( int manufacturerId );
        Task<Manufacturer> Create( Manufacturer request );
        Task<Manufacturer> Update( int manufacturerId, Manufacturer request );
        Task<Manufacturer> Delete( int manufacturerId );
    }

    /// <summary>
    /// Using IManufacturerRepository interface.
    /// </summary>
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for ManufacturerRepository.
        /// </summary>
        /// <param name="context"></param>
        public ManufacturerRepository( DatabaseContext context )
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a Manufacturer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>ManufacturerID</returns>
        public async Task<Manufacturer> Create( Manufacturer request )
        {
            m_context.Manufacturer.Add( request );
            await m_context.SaveChangesAsync();
            return await GetById( request.ManufacturerID );
        }

        /// <summary>
        /// Deletes a Manufacturer.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns>category</returns>
        public async Task<Manufacturer> Delete( int manufacturerId )
        {
            Manufacturer category = await GetById( manufacturerId );
            if (category != null)
            {
                m_context.Manufacturer.Remove( category );
                await m_context.SaveChangesAsync();
            }

            return category;
        }

        /// <summary>
        /// Gets all Manufacturers.
        /// </summary>
        /// <returns>List of Manufacturers</returns>
        public async Task<List<Manufacturer>> GetAll()
        {
            return await m_context.Manufacturer.Include( x => x.Products ).ToListAsync();
        }

        /// <summary>
        /// Gets a Manufacturers by its id.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns>manufacturersId</returns>
        public async Task<Manufacturer> GetById( int manufacturerId )
        {
            return await m_context.Manufacturer.Include( x => x.Products ).FirstOrDefaultAsync( x => x.ManufacturerID == manufacturerId );
        }

        /// <summary>
        /// Updates a Manufacturers.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="request"></param>
        /// <returns>category</returns>
        public async Task<Manufacturer> Update( int manufacturerId, Manufacturer request )
        {
            Manufacturer category = await GetById( manufacturerId );
            if (category != null)
            {
                category.ManufacturerName = request.ManufacturerName;
                category.Modified_At = DateTime.UtcNow;

                await m_context.SaveChangesAsync();
            }

            return category;
        }
    }
}
