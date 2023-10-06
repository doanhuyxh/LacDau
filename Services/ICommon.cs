namespace LacDau.Services
{
    public interface ICommon
    {
        Task<string> UploadTrademarkAsync(IFormFile file);
    }
}
