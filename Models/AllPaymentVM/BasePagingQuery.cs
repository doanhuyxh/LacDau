using Microsoft.AspNetCore.Mvc;

namespace LacDau.Models.AllPaymentVM
{
    [BindProperties]
    public class BasePagingQuery
    {
        public string? Criteria { get; set; } = string.Empty;
        public int? PageSize { get; set; } = 20;
        public int? PageIndex { get; set; } = 1;
    }
}
