using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Search()
        {

            string term = HttpContext.Request.Query["term"].ToString();
            List<string> szukanaNazwa = await _db.Products.Where(x=> x.Ukryty==false).Where(p => p.Name.Contains(term))
                                            .Select(p=> ( p.Name + " - [" + p.Symbol+"]")  ).Take(10).ToListAsync();
            
            return Ok(szukanaNazwa);
        }

        class PInfo
        {
            public string Name { get; set; }
            public string Symbol { get; set; }

        }
    }
}
