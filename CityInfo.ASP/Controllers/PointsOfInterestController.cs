using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.ASP.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            if(foundCity == null)
            {
                return NotFound();
            }

            return Ok(foundCity.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult GetPointOfInterest(int cityId, int poiId)
        {
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            if (foundCity == null)
            {
                return NotFound();
            }

            var foundPoi = foundCity.PointsOfInterest.Find(poi => poi.Id == poiId);

            if (foundPoi == null)
            {
                return NotFound();
            }

            return Ok(foundPoi);
        }
    }
}
