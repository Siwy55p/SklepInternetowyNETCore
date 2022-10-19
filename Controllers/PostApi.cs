using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
//szukanaNazwa PostApi
namespace partner_aluro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostApi : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public PostApi(ApplicationDbContext db)
        {
            _db = db;       
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<IActionResult> Serach()
        {
            string term = HttpContext.Request.Query["term"].ToString();
            var szukanaNazwa = _db.Products.Where(p => p.Name.Contains(term))
                                            .Select(p => p.Name).ToList();
            return Ok(szukanaNazwa);
        }
    }
}
