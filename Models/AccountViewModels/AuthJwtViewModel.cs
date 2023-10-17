namespace LacDau.Models.AccountViewModels
{
    public class AuthJwtViewModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
    }
}
