using LacDau.Data;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace LacDau.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            string CharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(CharSet.Length);
                result.Append(CharSet[index]);
            }
            return result.ToString();
        }

        public async Task<string> UploadIconCategoryAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/media/category");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/media/category/{path}";
        }

        public async Task<string> UploadTrademarkAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/trademark");

                if (file.FileName == null)
                    path = "logo.png";
                else
                    path = DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/trademark/{path}";
        }

        public async Task<string> UploadFileImgProductAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/productPicture");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + ".png";
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/productPicture/{path}";
        }

        public async Task<string> UploadFileVideoProductAsync(IFormFile file)
        {
            string path = string.Empty;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/productVideo");

                if (file.FileName == null)
                    path = "icon.png";
                else
                    path = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, path);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return $"/upload/productVideo/{path}";
        }

    }

}