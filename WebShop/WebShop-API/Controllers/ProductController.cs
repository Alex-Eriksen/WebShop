using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WebShop_API.Controllers
{
    /// <summary>
    /// Using ProductController to control products
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Using IProductService interface
        /// </summary>
        private readonly IProductService m_productService;

        /// <summary>
        /// Using ICategoryService interface.
        /// </summary>
        private readonly ICategoryService m_categoryService;

        /// <summary>
        /// IManufacturerService interface.
        /// </summary>
        private readonly IManufacturerService m_manufacturerService;

        /// <summary>
        /// Constructor for ProductController.
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="categoryService"></param>
        /// <param name="manufacturerService"></param>
        public ProductController( IProductService productService, ICategoryService categoryService, IManufacturerService manufacturerService )
        {
            m_productService = productService;
            m_categoryService = categoryService;
            m_manufacturerService = manufacturerService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<StaticProductResponse> products = await m_productService.GetAll();

                if (products == null)
                {
                    return Problem("Nothing was returned from service, this is unexpected");
                }

                if (products.Count == 0)
                {
                    return NoContent();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gets products by their id.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetById( int productId )
        {
            try
            {
                DirectProductResponse directProductResponse = await m_productService.GetById( productId );

                if (directProductResponse == null)
                {
                    return NotFound();
                }
                return Ok(directProductResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Creates products.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> Create( [FromBody] ProductRequest request )
        {
            try
            {
                DirectProductResponse directProductResponse = await m_productService.Create( request );

                if (directProductResponse == null)
                {
                    return Problem("SuperHero was not created, something failed...");
                }
                return Ok(directProductResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Updates products.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{productId}")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> Update( int productId, ProductRequest request )
        {
            try
            {
                DirectProductResponse directProductResponse = await m_productService.Update( productId, request );

                if (directProductResponse == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Deletes products.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{productId}")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> Delete( int productId )
        {
            try
            {
                DirectProductResponse directProductResponse = await m_productService.Delete( productId );

                if (directProductResponse == null)
                {
                    return NotFound();
                }
                return Ok(directProductResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                List<StaticCategoryResponse> products = await m_categoryService.GetAll();

                if (products == null)
                {
                    return Problem("Nothing was returned from service, this is unexpected");
                }

                if (products.Count == 0)
                {
                    return NoContent();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets categories by thier id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetCategoryById( int categoryId )
        {
            try
            {
                DirectCategoryResponse response = await m_categoryService.GetById( categoryId );

                if (response == null)
                {
                    return NotFound();
                }
                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Creates categories.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("category")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> CreateCategory( CategoryRequest request )
        {
            try
            {
                DirectCategoryResponse response = await m_categoryService.Create( request );

                if (response == null)
                {
                    return Problem( "SuperHero was not created, something failed..." );
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Updates categories.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("category")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> UpdateCategory( int categoryId, CategoryRequest request )
        {
            try
            {
                DirectCategoryResponse response = await m_categoryService.Update( categoryId, request );
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Deletes categories.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("category/{categoryId}")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> DeleteCategory( int categoryId )
        {
            try
            {
                DirectCategoryResponse response = await m_categoryService.Delete( categoryId );

                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets all types.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("type")]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                List<StaticProductTypeResponse> responses = await m_categoryService.GetAllTypes();
                if(responses == null)
                {
                    return Problem( "Category Service returned null." );
                }


                if(responses.Count == 0)
                {
                    return NoContent();
                }

                return Ok( responses );

            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gets types by their id.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("type/{productTypeId}")]
        public async Task<IActionResult> GetType( int productTypeId )
        {
            try
            {
                DirectProductTypeResponse response = await m_categoryService.GetType(productTypeId);
                if(response == null)
                {
                    return NotFound();
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Creates types.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("type")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> CreateType(ProductTypeRequest request)
        {
            try
            {
                DirectProductTypeResponse response = await m_categoryService.CreateType(request);
                if(response == null)
                {
                    return Problem( "Couldnt create new type." );
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Updates types.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("type/{productTypeId}")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> UpdateType(int productTypeId, ProductTypeRequest request)
        {
            try
            {
                DirectProductTypeResponse response = await m_categoryService.UpdateType(productTypeId, request);
                if(response == null)
                {
                    return NotFound( $"Couldnt find a product type with the id: {productTypeId}" );
                }

                return Ok( response );
            }
            catch(Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Deletes types.
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("type/{productTypeId}")]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> DeleteType(int productTypeId)
        {
            try
            {
                DirectProductTypeResponse response = await m_categoryService.DeleteType(productTypeId);
                if(response == null)
                {
                    return NotFound( $"Couldnt find a product type with the id: {productTypeId}" );
                }

                return Ok(response);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Gets all manufacturers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route( "manufacturer" )]
        public async Task<IActionResult> GetAllManufacturers()
        {
            try
            {
                List<StaticManufacturerResponse> responses = await m_manufacturerService.GetAll();
                if (responses == null)
                {
                    return Problem( "Manufacturer Service returned null." );
                }

                if(responses.Count == 0)
                {
                    return NoContent();
                }

                return Ok( responses );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Gets manufacturers by their id.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route( "manufacturer/{manufacturerId}" )]
        public async Task<IActionResult> GetManufacturer(int manufacturerId)
        {
            try
            {
                DirectManufacturerResponse response = await m_manufacturerService.GetById( manufacturerId );
                if (response == null)
                {
                    return NotFound();
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Creates manufacturers.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route( "manufacturer" )]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> CreateManufacturer(ManufacturerRequest request)
        {
            try
            {
                DirectManufacturerResponse response = await m_manufacturerService.Create(request);
                if (response == null)
                {
                    return Problem( "Couldnt create new manufacturer." );
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        /// <summary>
        /// Uppdates manufacturers.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route( "manufacturer/{manufacturerId}" )]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> UpdateManufacturer( int manufacturerId, ManufacturerRequest request )
        {
            try
            {
                DirectManufacturerResponse response = await m_manufacturerService.Update( manufacturerId, request );
                if (response == null)
                {
                    return NotFound( $"Couldnt find a product type with the id: {manufacturerId}" );
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        /// <summary>
        /// Deletes manufacturers.
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route( "manufacturer/{manufacturerId}" )]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> DeleteManufacturer( int manufacturerId )
        {
            try
            {
                DirectManufacturerResponse response = await m_manufacturerService.Delete( manufacturerId );
                if (response == null)
                {
                    return NotFound( $"Couldnt find a product type with the id: {manufacturerId}" );
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        [HttpPost]
        [Route( "photo/{productId}" )]
        [Authorize( Roles = "Admin" )]
        public async Task<IActionResult> CreatePhoto( int productId )
        {
            try
            {
                foreach(IFormFile imageFile in Request.Form.Files)
                {
                    PhotoRequest request = new()
                    {
                        PhotoID = 0,
                        ProductID = productId
                    };

                    if (imageFile.Length == 0)
                    {
                        return BadRequest( "File not selected" );
                    }

                    string path = Path.Combine( Directory.GetCurrentDirectory(), "FileFolder", imageFile.FileName );
                    using (FileStream fileStream = new FileStream( path, FileMode.Create ))
                    {
                        await imageFile.CopyToAsync( fileStream );
                        fileStream.Close();
                    }

                    request.ImageName = imageFile.FileName;
                    request.ImageLocation = path;

                    DirectPhotoResponse response = await m_productService.CreatePhoto( request );

                    if (response == null)
                    {
                        return Problem( "Something went wrong!" );
                    }
                }

                return Ok( new { message = "SUCCESS!" } );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        [HttpDelete]
        [Route("photo/{photoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePhoto( int photoId )
        {
            try
            {
                DirectPhotoResponse response = await m_productService.DeletePhoto( photoId );

                FileInfo file = new FileInfo( response.ImageLocation );

                if (file.Exists)
                {
                    file.Delete();
                }

                if(response == null)
                {
                    return NotFound();
                }

                return Ok( response );
            }
            catch (Exception ex)
            {
                return Problem( ex.Message );
            }
        }

        [HttpGet]
        [Route("photo/{fileName}")]
        public async Task<IActionResult> GetPhoto( string fileName )
        {
            byte[] b = await System.IO.File.ReadAllBytesAsync(Path.Combine(Directory.GetCurrentDirectory(), "FileFolder", fileName));
            return File( b, "image/jpeg" );
        }
    }
}
