using Microsoft.AspNetCore.Http;

namespace WebShop_API.Repositories
{
    /// <summary>
    /// Interface with methods: GetAll, GetById, Create, Update and Delete.
    /// </summary>
    public interface IPhotoRepository
    {
        Task<Photo> GetById( int photoId );
        Task<Photo> Create( Photo request );
        Task<Photo> Delete( int photoId );
    }

    /// <summary>
    /// Using IPhotoRepository interface.
    /// </summary>
    public class PhotoRepository : IPhotoRepository
    {
        private readonly DatabaseContext m_context;

        /// <summary>
        /// Constructor for PhotoRepository.
        /// </summary>
        /// <param name="context"></param>
        public PhotoRepository(DatabaseContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Creates a Photo.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>request</returns>
        public async Task<Photo> Create( Photo request )
        {
            m_context.Photo.Add( request );
            await m_context.SaveChangesAsync();
            return await GetById( request.PhotoID );
        }

        /// <summary>
        /// Deletes a Photo.
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns>photo</returns>
        public async Task<Photo> Delete( int photoId )
        {
            Photo photo = await GetById( photoId );
            m_context.Photo.Remove( photo );
            await m_context.SaveChangesAsync();
            return photo;
        }

        /// <summary>
        /// Gets a Photo by its Id.
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns>Photo</returns>
        public async Task<Photo> GetById( int photoId )
        {
            return await m_context.Photo.Include(x => x.Product).FirstOrDefaultAsync( x => x.PhotoID == photoId );
        }
    }
}