using WebShop_API.Database.Entities;

namespace WebShop_API.Services
{
    public interface ICategoryService
    {
        Task<List<StaticCategoryResponse>> GetAll();
        Task<DirectCategoryResponse> GetById( int categoryId );
        Task<DirectCategoryResponse> Create( CategoryRequest request );
        Task<DirectCategoryResponse> Update( int categoryId, CategoryRequest request );
        Task<DirectCategoryResponse> Delete( int categoryId );
        Task<List<StaticProductTypeResponse>> GetAllTypes();
        Task<DirectProductTypeResponse> GetType( int productTypeId );
        Task<DirectProductTypeResponse> CreateType( ProductTypeRequest request );
        Task<DirectProductTypeResponse> UpdateType( int productTypeId, ProductTypeRequest request );
        Task<DirectProductTypeResponse> DeleteType( int productTypeId );
    }

    /// <summary>
    /// CategoryService is used to transfere data to and from CategoryRepository and CategoryController.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository m_categoryRepository;
        private readonly IProductTypeRepository m_productTypeRepository;
        private readonly IMapper m_mapper;

        /// <summary>
        /// Constructor of CategoryService.
        /// </summary>
        /// <param name="categoryRepository"></param>
        /// <param name="productTypeRepository"></param>
        /// <param name="mapper"></param>
        public CategoryService( ICategoryRepository categoryRepository, IProductTypeRepository productTypeRepository, IMapper mapper )
        {
            m_categoryRepository = categoryRepository;
            m_productTypeRepository = productTypeRepository;
            m_mapper = mapper;
        }

        /// <summary>
        /// Creates a request of CategoryRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>category or null</returns>
        public async Task<DirectCategoryResponse> Create( CategoryRequest request )
        {
            Category category = await m_categoryRepository.Create( m_mapper.Map<Category>( request ) );

            if(category != null)
            {
                return m_mapper.Map<DirectCategoryResponse>( category );
            }
            return null;
        }

        /// <summary>
        /// Creates a type of DirectProductTypeResponse.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>pruductType or null</returns>
        public async Task<DirectProductTypeResponse> CreateType( ProductTypeRequest request )
        {
            ProductType productType = await m_productTypeRepository.Create( m_mapper.Map<ProductType>( request ) );
            if( productType != null)
            {
                return m_mapper.Map<DirectProductTypeResponse>( productType );
            }
            return null;
        }

        /// <summary>
        /// Deletes a category by Id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>category or null</returns>
        public async Task<DirectCategoryResponse> Delete( int categoryId )
        {
            Category category = await m_categoryRepository.Delete( categoryId );

            if(category != null)
            {
                return m_mapper.Map<DirectCategoryResponse>( category );
            }
            return null;
        }

        /// <summary>
        /// Deletes a product by Id.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>productType</returns>
        public async Task<DirectProductTypeResponse> DeleteType( int productTypeId )
        {
            ProductType productType = await m_productTypeRepository.Delete( productTypeId );
            if(productType != null)
            {
                return m_mapper.Map<DirectProductTypeResponse>( productType );
            }
            return null;
        }

        /// <summary>
        /// Gets all categorys.
        /// </summary>
        /// <returns>List of categories or null</returns>
        public async Task<List<StaticCategoryResponse>> GetAll()
        {
            List<Category> categories = await m_categoryRepository.GetAll();

            if(categories != null)
            {
                List<StaticCategoryResponse> categoryResponses = categories.Select( category => m_mapper.Map<StaticCategoryResponse>(category) ).ToList();
                for(int i = 0; i < categoryResponses.Count; i++)
                {
                    categoryResponses[i].ProductCount = categories[i].Products.Count;
                }
                return categoryResponses;
            }
            return null;
        }

        /// <summary>
        /// Gets all types of product types.
        /// </summary>
        /// <returns>productType or null</returns>
        public async Task<List<StaticProductTypeResponse>> GetAllTypes()
        {
            List<ProductType> productTypes = await m_productTypeRepository.GetAll();
            if(productTypes != null)
            {
                return productTypes.Select( productType => m_mapper.Map<StaticProductTypeResponse>( productType ) ).ToList();
            }
            return null;
        }

        /// <summary>
        /// Gets a categotry by its id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>category or null</returns>
        public async Task<DirectCategoryResponse> GetById( int categoryId )
        {
            Category category = await m_categoryRepository.GetById( categoryId );

            if(category != null)
            {
                DirectCategoryResponse mappedCategory = m_mapper.Map<DirectCategoryResponse>( category );
                List<StaticProductResponse> mappedProducts = mappedCategory.Products;
                foreach (var product in category.Products)
                {
                    if (product.Photos.Count == 0)
                    {
                        continue;
                    }

                    var x = mappedProducts.Find( x => x.ProductID == product.ProductID );
                    if (x != null)
                    {
                        x.ImageName = product.Photos.First().ImageName;
                    }
                }
                return mappedCategory;
            }
            return null;
        }

        /// <summary>
        /// Gets prodoctType by its id.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns>productType or null</returns>
        public async Task<DirectProductTypeResponse> GetType( int productTypeId )
        {
            ProductType productType = await m_productTypeRepository.GetById( productTypeId );
            if(productType != null)
            {
                return m_mapper.Map<DirectProductTypeResponse>( productType );
            }
            return null;
        }

        /// <summary>
        /// Updates category by its id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="request"></param>
        /// <returns>category or null</returns>
        public async Task<DirectCategoryResponse> Update( int categoryId, CategoryRequest request )
        {
            Category category = await m_categoryRepository.Update( categoryId, m_mapper.Map<Category>( request ) );

            if(category != null)
            {
                return m_mapper.Map<DirectCategoryResponse>( category );
            }
            return null;
        }

        /// <summary>
        /// Updates productType by its id.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="request"></param>
        /// <returns>productType or null</returns>
        public async Task<DirectProductTypeResponse> UpdateType( int productTypeId, ProductTypeRequest request )
        {
            ProductType productType = await m_productTypeRepository.Update( productTypeId, m_mapper.Map<ProductType>( request ) );
            if(productType != null)
            {
                return m_mapper.Map<DirectProductTypeResponse>( productType );
            }
            return null;
        }
    }
}
