using LacDau.Data;
using LacDau.Models.AllPaymentVM;
using LacDau.Models.MerchantVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchantsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MerchantsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get(string criteria)
        {
            return Ok();
        }

        public IActionResult GetPaging([FromQuery] BasePagingQuery query)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateMerchantVM request)
        {
            return Ok(true);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult UpDate([FromBody] UpdateMerchantVM request)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{id}/set-active")]
        public IActionResult SetActive(string id, [FromBody] SetActiveMerchantVM request)
        {
            var response = new BaseResult();
            return Ok(response);
        }
    }


}
