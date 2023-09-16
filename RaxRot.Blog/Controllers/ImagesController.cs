using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaxRot.Blog.Repositories;
using System.Net;

namespace RaxRot.Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
           var imageURl = await _imageRepository.UploadAsync(file);
            if (imageURl == null)
            {
                return Problem("Something wrong", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageURl });
        }
    }
}
