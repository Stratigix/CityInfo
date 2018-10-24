using CityInfo.ASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CityInfo.ASP.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            if (foundCity == null)
            {
                return NotFound();
            }

            return Ok(foundCity.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{poiId}", Name = "GetPointOfInterest")]
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

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null) return BadRequest();

            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            if (foundCity == null) return NotFound();

            // get the new POI ID - not optimal, will improve (todo)
            var currentMaxId = CitiesDataStore.Current.Cities.SelectMany(city => city.PointsOfInterest).Max(poiDto => poiDto.Id);

            var newPoi = new PointOfInterestDto
            {
                Id = ++currentMaxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            foundCity.PointsOfInterest.Add(newPoi);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = newPoi.Id }, newPoi);
        }
    }
}