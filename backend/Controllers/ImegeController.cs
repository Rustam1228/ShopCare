using backend.Core.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using backend.Context;
using backend.Core.Dto.ImegeDto;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly RepositoryImage _image;
        public ImageController(ApplicationDbContext context)
        {
            _image = new RepositoryImage(context);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateImageCare([FromForm] CreateImage imageCreate, IFormFile file)
        {
            var result = await _image.Add(imageCreate, file);
            if (!result)
            {
                return NotFound("Ошибка в сохранении");
            }
            return Ok("Файл сохранен");
        }

        [HttpGet]
        [Route("GetFull")]
        public async Task<ActionResult<List<ImageModel>>> GetFulImages()
        {   
            var result= await _image.GetFull();

            return Ok(result);
        }

        [HttpGet]
        [Route("OniImage{id}")]
        public async Task<ActionResult<GetImage>> GetOniImage(Guid id)
        {
            var result = await _image.GetOneImage(id);            
            if (result==null)
            {
                return NotFound("Не нашлось фото по данному Id");
            }           
            return Ok(result);
        }

        [HttpGet]
        [Route("FullImage{id}")]
        public async Task <ActionResult<List<byte[]>>> GetFullImage(Guid id)
        {
            var result = await _image.GetFullImage(id);
            if (result == null)
            {
                return NotFound("Не нашлось фото по данному Id");
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            var result = await _image.Delete(name);
            if (result!=string.Empty)
            {
                return NotFound(result);
            }
            return Ok("Удаление прошло успешно");
        }


    }
}
