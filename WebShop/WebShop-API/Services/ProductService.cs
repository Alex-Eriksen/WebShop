namespace WebShop_API.Services
{
    public interface IProductService
    {
        Task<List<StaticProductResponse>> GetAll();
        Task<DirectProductResponse> GetById(int productId);
        Task<DirectProductResponse> Create(ProductRequest productRequest);
        Task<DirectProductResponse> Update(int productId, ProductRequest productRequest);
        Task<DirectProductResponse> Delete(int productId);
        Task<DirectPhotoResponse> GetPhoto( int photoId );
        Task<DirectPhotoResponse> DeletePhoto( int photoId );
        Task<DirectPhotoResponse> CreatePhoto( PhotoRequest request );
    }

    /// <summary>
    ///  ProductService is used to transfere data to and from ProductRepository and ProductController.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository m_productRepository;
        private readonly IPhotoRepository m_photoRepository;
        private readonly IMapper m_mapper;

        /// <summary>
        /// Constructor of ProductService.
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="photoRepository"></param>
        /// <param name="mapper"></param>
        public ProductService(IProductRepository productRepository, IPhotoRepository photoRepository, IMapper mapper)
        {
            m_productRepository = productRepository;
            m_photoRepository = photoRepository;
            m_mapper = mapper;
        }

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns>product or null</returns>
        public async Task<DirectProductResponse> Create( ProductRequest productRequest )
        {
            Product product = await m_productRepository.Create( m_mapper.Map<Product>( productRequest ) );
            
            if(product != null)
            {
                return m_mapper.Map<DirectProductResponse>(product);
            }

            return null;
        }

        public async Task<DirectPhotoResponse> CreatePhoto( PhotoRequest request )
        {
            Photo photo = await m_photoRepository.Create( m_mapper.Map<Photo>( request ) );
            if(photo != null)
            {
                return m_mapper.Map<DirectPhotoResponse>( photo );
            }

            return null;
        }

        /// <summary>
        /// Deletes a product by its id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>product or null</returns>
        public async Task<DirectProductResponse> Delete( int productId )
        {
            Product product = await m_productRepository.Delete(productId);
            if (product != null)
            {
                return m_mapper.Map<DirectProductResponse>(product);
            }

            return null;
        }

        public async Task<DirectPhotoResponse> DeletePhoto( int photoId )
        {
            Photo photo = await m_photoRepository.Delete( photoId );
            if(photo != null)
            {
                return m_mapper.Map<DirectPhotoResponse>( photo );
            }

            return null;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>product or null</returns>
        public async Task<List<StaticProductResponse>> GetAll()
        {
            List<Product> products = await m_productRepository.GetAll();
            if(products != null)
            {
                List<StaticProductResponse> mappedProducts = products.Select( product => m_mapper.Map<StaticProductResponse>( product ) ).ToList();
                foreach (var product in products)
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
                return mappedProducts;
            }

            return null;
        }

        /// <summary>
        /// Gets a product by its id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>product or null</returns>
        public async Task<DirectProductResponse> GetById( int productId )
        {
            Product product = await m_productRepository.GetById(productId);
            if(product != null)
            {
                return m_mapper.Map<DirectProductResponse>(product);
            }

            return null;
        }

        public async Task<DirectPhotoResponse> GetPhoto( int photoId )
        {
            Photo photo = await m_photoRepository.GetById( photoId );
            if(photo != null)
            {
                return m_mapper.Map<DirectPhotoResponse>( photo );
            }

            return null;
        }

        /// <summary>
        /// Updates a product by its id.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productRequest"></param>
        /// <returns>product or null</returns>
        public async Task<DirectProductResponse> Update( int productId, ProductRequest productRequest )
        {
            Product product = await m_productRepository.Update( productId, m_mapper.Map<Product>( productRequest ) );

            if ( product != null )
            {
                return m_mapper.Map<DirectProductResponse>( product );
            }

            return null;
        }
    }
}
