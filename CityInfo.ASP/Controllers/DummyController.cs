using CityInfo.ASP.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.ASP.Controllers
{
    public class DummyController : Controller
    {
        private CityInfoContext _cityInfoContext;

        public DummyController(CityInfoContext ctx)
        {
            _cityInfoContext = ctx;
        }

        [HttpGet]
        [Route("api/testdb")]
        public IActionResult TestDb()
        {
            return Ok();
        }
    }
}
