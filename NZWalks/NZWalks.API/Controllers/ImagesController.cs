using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO.Image;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository repository;
        private readonly ILogger<ImagesController> logger;

        public ImagesController(IImageRepository repository, ILogger<ImagesController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // -------------------------------------- Upload Image ------------------------------------------------
        // POST : https://localhost:1234/api/Images/Upload

        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto uploadImagesRequestDto)
        {
            ValidateFileUpload(uploadImagesRequestDto);

            if (ModelState.IsValid) 
            {
                // Convert Dto into Domain Models
                var imageDomainModel = new Image
                {
                     File = uploadImagesRequestDto.File,
                     FileName = uploadImagesRequestDto.FileName,
                     FileSizeInBytes = uploadImagesRequestDto.File.Length,
                     FileDescription = uploadImagesRequestDto.FileDescription,
                     FileExtension = Path.GetExtension(uploadImagesRequestDto.File.FileName)
                };

                // Save the image in the DB
                var savedImage = await repository.UploadImageAsync(imageDomainModel);

                logger.LogInformation($"{savedImage.FileName} image successfully saved in the database");

                if (savedImage == null) 
                {
                    return BadRequest();
                }

                return Ok(savedImage);
            }

            // Return a badRequest Error

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto requestDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };

            if (allowedExtensions.Contains(Path.GetExtension(requestDto.File.FileName)) == false)
            {
                ModelState.AddModelError("File", "Unsupported File Extension");
            }

            if (requestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("File", "File Size is more than 10 MB, Please upload a smaller file size");
            }
        }
    }
}
