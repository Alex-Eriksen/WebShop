namespace WebShop_API.Services
{
    public interface IManufacturerService
    {
        Task<List<StaticManufacturerResponse>> GetAll();
        Task<DirectManufacturerResponse> GetById( int manufacturerId );
        Task<DirectManufacturerResponse> Create( ManufacturerRequest request );
        Task<DirectManufacturerResponse> Update( int manufacturerId, ManufacturerRequest request );
        Task<DirectManufacturerResponse> Delete( int manufacturerId );
    }

    /// <summary>
    /// ManufacturerService is used to transfere data to and from ManufacturerRepository and ProductController.
    /// </summary>
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository m_manufacturerRepository;
        private readonly IMapper m_mapper;

        /// <summary>
        /// Constructor of ManufacturerService.
        /// </summary>
        /// <param name="manufacturerRepository"></param>
        /// <param name="mapper"></param>
        public ManufacturerService( IManufacturerRepository manufacturerRepository, IMapper mapper )
        {
            m_manufacturerRepository = manufacturerRepository;
            m_mapper = mapper;
        }

        /// <summary>
        /// Creates a manufacturer.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>manufacturer or null</returns>
        public async Task<DirectManufacturerResponse> Create( ManufacturerRequest request )
        {
            Manufacturer manufacturer = await m_manufacturerRepository.Create( m_mapper.Map<Manufacturer>( request ) );
            if (manufacturer != null)
            {
                return m_mapper.Map<DirectManufacturerResponse>( manufacturer );
            }

            return null;
        }

        /// <summary>
        /// Deletes a manufacturer bu its id.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        public async Task<DirectManufacturerResponse> Delete( int manufacturerId )
        {
            Manufacturer manufacturer = await m_manufacturerRepository.Delete( manufacturerId );
            if (manufacturer != null)
            {
                return m_mapper.Map<DirectManufacturerResponse>( manufacturer );
            }

            return null;
        }

        /// <summary>
        /// Gets all manufacturers.
        /// </summary>
        /// <returns>manufacturer or null</returns>
        public async Task<List<StaticManufacturerResponse>> GetAll()
        {
            List<Manufacturer> products = await m_manufacturerRepository.GetAll();
            if (products != null)
            {
                return products.Select( manufacturer => m_mapper.Map<StaticManufacturerResponse>( manufacturer ) ).ToList();
            }

            return null;
        }

        /// <summary>
        /// Gets a manufacturer by its id.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns>manufacturer or null</returns>
        public async Task<DirectManufacturerResponse> GetById( int manufacturerId )
        {
            Manufacturer manufacturer = await m_manufacturerRepository.GetById( manufacturerId );
            if (manufacturer != null)
            {
                return m_mapper.Map<DirectManufacturerResponse>( manufacturer );
            }

            return null;
        }

        /// <summary>
        /// Updates a manufacturer.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="request"></param>
        /// <returns>manufacturer of null</returns>
        public async Task<DirectManufacturerResponse> Update( int manufacturerId, ManufacturerRequest request )
        {
            Manufacturer manufacturer = await m_manufacturerRepository.Update( manufacturerId, m_mapper.Map<Manufacturer>( request ) );
            if (manufacturer != null)
            {
                return m_mapper.Map<DirectManufacturerResponse>( manufacturer );
            }

            return null;
        }
    }
}
