using LacDau.Data;

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

    }
}

