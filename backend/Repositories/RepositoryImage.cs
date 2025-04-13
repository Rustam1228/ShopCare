using backend.Context;
using backend.Core.Dto.ImegeDto;
using backend.Core.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http.Headers;
using System.Xml;

namespace backend.Repositories
{
    public class RepositoryImage
    {
        private ApplicationDbContext _context { get; }

        public RepositoryImage(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// сохраняет фото в папке imageFile (как пример но не более)
        /// </summary>
        /// <returns> возвращает true при выполнении без ошибок </returns>
        public async Task<bool> Add(CreateImage image, IFormFile file)
        {
            var JpgFile = "image/jpeg";

            if (file == null || file.ContentType != JpgFile)
            {
                return false;
            }
            var fileName = string.Empty;
            do
            {
                fileName = Guid.NewGuid().ToString() + ".JPEG";
            }
            while (_context.Images.Any(x => x.ImageName == fileName));

            var fileNewPath = Path.Combine(Directory.GetCurrentDirectory(), "imageFile", fileName);

            using (var stream = new FileStream(fileNewPath, FileMode.Create))
            {
                file.CopyTo(stream);
            };                 
            
            var imageModel = new ImageModel()
            {
                ImageName = fileName,
                ImageId = image.ImageId
            };
            await _context.Images.AddAsync(imageModel);
            await _context.SaveChangesAsync();
            return true;
        }        

        /// <summary>
        /// Показывает фото по Id
        /// </summary>         
        
        public async Task<ActionResult<GetImage>> GetOneImage(Guid id)
        {
            GetImage getImage = new GetImage();

            if (!_context.Images.Any(x => x.Id == id))
            {
                return getImage;
            }

            var image = await _context.Images.FirstOrDefaultAsync(x => x.Id == id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imageFile", image.ImageName);
            if (File.Exists(filePath))
            {
                getImage.ImageData = File.ReadAllBytes(filePath);
                getImage.ImageName = image.ImageName;
            }
            return getImage;
        }

        /// <summary>
        /// Показывает все фото по Id анкеты машины 
        /// </summary>
       
        public async Task<ActionResult<List<GetImage>>> GetFullImage(Guid id)
        {
            var images = await _context.Images.Where(x => x.ImageId == id).ToListAsync();

            var result = new List<GetImage>();

            if (images != null)
            {
                foreach (var i in images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imageFile", i.ImageName);
                    if (File.Exists(filePath))
                    {                       
                        var fileBytes = File.ReadAllBytes(filePath);
                        var newGetImage = new GetImage()
                        {
                            ImageName = i.ImageName,
                            ImageData = fileBytes
                        };
                        result.Add(newGetImage);
                    }
                }
            }
            return result;
        }
       /// <summary>
       /// Удаляет фото 
       /// </summary>       
       /// <returns></returns>
        public async Task<string> Delete(string imageName)
        {
            if (!_context.Images.Any(x => x.ImageName == imageName))
            {
                return "Нет фото с данным названием в базе данных";
            }

            var image = await _context.Images.FirstOrDefaultAsync(x => x.ImageName == imageName);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imageFile", image.ImageName);

            if (!File.Exists(filePath))
            {
                return "Нет фото на сервере";
            }

            File.Delete(filePath);
            await _context.Images.Where(x => x.ImageName == imageName).ExecuteDeleteAsync();
            return string.Empty;
        }
        public async Task<List<ImageModel>> GetFull()
        {
            return await _context.Images.ToListAsync();
        }

    }


}

