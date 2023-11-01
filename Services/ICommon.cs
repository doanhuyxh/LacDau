namespace LacDau.Services
{
    public interface ICommon
    {
        Task<string> UploadTrademarkAsync(IFormFile file);
        Task<string> UploadIconCategoryAsync(IFormFile file);
        string RandomString (int length);
    }
}
