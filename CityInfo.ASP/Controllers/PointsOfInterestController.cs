using CityInfo.ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
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
            // if body is null, send bad request
            if (pointOfInterest == null)
            {
                return BadRequest(ModelState);
            }

            // add a validation check to the model state to check that the description is not the same as the name.
            if (pointOfInterest.Description.Equals(pointOfInterest.Name, System.StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("Description", "The description cannot be the same as the name.");
            }

            // if the model is not valid (decorated by attributes), send bad request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            // get the specified city
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            // if the city is not found, send not found
            if (foundCity == null)
            {
                return NotFound();
            }

            // get the new POI ID - not optimal, will improve (todo)
            var currentMaxId = CitiesDataStore.Current.Cities.SelectMany(city => city.PointsOfInterest).Max(poiDto => poiDto.Id);

            var newPoi = new PointOfInterestDto
            {
                Id = ++currentMaxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            // add the new point of interest to the specified city
            foundCity.PointsOfInterest.Add(newPoi);


            // create a result pointing to the new city (with the route specified)
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, poiId = newPoi.Id }, newPoi);
        }

        [HttpPut("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int poiId, [FromBody] PointOfInterestToUpdateDto pointOfInterest)
        {
            // if body is null, send bad request
            if (pointOfInterest == null)
            {
                return BadRequest(ModelState);
            }

            // add a validation check to the model state to check that the description is not the same as the name.
            if (pointOfInterest.Description?.Equals(pointOfInterest.Name, System.StringComparison.InvariantCultureIgnoreCase) == true)
            {
                ModelState.AddModelError("Description", "The description cannot be the same as the name.");
            }

            // if the model is not valid (decorated by attributes), send bad request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            // get the specified city
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            // if the city is not found, send not found
            if (foundCity == null)
            {
                return NotFound();
            }

            var pointOfInterestToUpdate = foundCity.PointsOfInterest.Find(poi => poi.Id == poiId);

            if(pointOfInterestToUpdate == null)
            {
                return NotFound();
            }

            pointOfInterestToUpdate.Name = pointOfInterest.Name;
            pointOfInterestToUpdate.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult PatchPointOfInterest(int cityId, int poiId, [FromBody] JsonPatchDocument<PointOfInterestToUpdateDto> poiPatch)
        {
            if(poiPatch == null)
            {
                return BadRequest();
            }

            // get the specified city
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            // if the city is not found, send not found
            if (foundCity == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = foundCity.PointsOfInterest.Find(poi => poi.Id == poiId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var poiDto = new PointOfInterestToUpdateDto
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            poiPatch.ApplyTo(poiDto, ModelState);

            // add a validation check to the model state to check that the description is not the same as the name.
            if (poiDto.Description?.Equals(poiDto.Name, System.StringComparison.InvariantCultureIgnoreCase) == true)
            {
                ModelState.AddModelError("Description", "The description cannot be the same as the name.");
            }

            TryValidateModel(poiDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = poiDto.Name;
            pointOfInterestFromStore.Description = poiDto.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{poiId}")]
        public IActionResult DeletePointOfInterest(int cityId, int poiId)
        {
            // get the specified city
            var foundCity = CitiesDataStore.Current.Cities.Find(city => city.Id == cityId);

            // if the city is not found, send not found
            if (foundCity == null)
            {
                return NotFound();
            }

            //
            var pointOfInterestToDelete = foundCity.PointsOfInterest.Find(poi => poi.Id == poiId);

            if (pointOfInterestToDelete == null)
            {
                return NotFound();
            }

            foundCity.PointsOfInterest.Remove(pointOfInterestToDelete);

            return NoContent();
        }
    }
}