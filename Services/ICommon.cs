namespace LacDau.Services
{
    public interface ICommon
    {
        Task<string> UploadTrademarkAsync(IFormFile file);
        Task<string> UploadIconCategoryAsync(IFormFile file);
        string RandomString (int length);
        Task<string> UploadFileImgProductAsync(IFormFile file);
        Task<string> UploadFileVideoProductAsync(IFormFile file);
        Task<string> UploadFileBannerAsync(IFormFile file);
        public string GenerateSlug(string name);
        public string RemoveAccents(string input);
    }
}
