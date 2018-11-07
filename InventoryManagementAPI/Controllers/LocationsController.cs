using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{
    [Authorize]
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepo;
        private readonly IMapper _mapper;

        public LocationsController(ILocationRepository locationRepo, IMapper mapper)
        {
            _locationRepo = locationRepo;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetLocations([FromQuery] LocationParams locationParams)
        {
            var locations = await _locationRepo.GetLocations(locationParams);

            var locationList = _mapper.Map<ICollection<LocationListDto>>(locations);

            Response.AddPagination(locations.CurrentPage, locations.PageSize,
                locations.TotalCount, locations.TotalPages);


            return Ok(locationList);

        }

        [HttpGet("{id}", Name ="GetLocation")]
        public async Task<IActionResult> GetLocation(int id)
        {
            var location = await _locationRepo.GetLocation(id);

            if (location == null)
                return NotFound();

            var locationDetail = _mapper.Map<LocationDetailDto>(location);

            return Ok(locationDetail);

        }

        [HttpGet("types")]
        public async Task<IActionResult> GetLocationTypes()
        {
            var locationTypes = await _locationRepo.GetLocationTypes();

            var returnedLocationTypes = _mapper.Map<ICollection<LocationTypeDto>>(locationTypes);

            return Ok(returnedLocationTypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationFormDto location)
        {
            var locationType = await _locationRepo.GetLocationType(location.LocationTypeId);

            if (locationType == null)
                ModelState.AddModelError("error", "Location Type not found");


            if (string.IsNullOrEmpty(location.Name))
                ModelState.AddModelError("error", "Location name is required");

            if (string.IsNullOrEmpty(location.Address))
                ModelState.AddModelError("error", "Location Address is required");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newLocation = new Location
            {
                Name = location.Name,
                Address = location.Address,
                Phone = location.Phone,
                LocationType = locationType
            };

            _locationRepo.Add(newLocation);

            if(await _locationRepo.Save())
            {
                var locationToReturn = _mapper.Map<LocationDetailDto>(newLocation);

                return CreatedAtRoute("GetLocation", new { controller = "Locations", id = newLocation.Id }, locationToReturn);
            }


            return BadRequest(new {error="Unabble to Create new Location" });


        }


        [HttpPost("types")]
        public async Task<IActionResult> CreateLocationType([FromBody] LocationTypeDto locationType)
        {
            if (string.IsNullOrEmpty(locationType.Name))
                ModelState.AddModelError("error", "LocationType Required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var newLocationType = new LocationType
            {
                Name = locationType.Name
            };

            _locationRepo.Add(newLocationType);

            if (await _locationRepo.Save())
                return Ok(newLocationType);

            return BadRequest(new { error = new string[] { "Error creating Location Type" } });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationFormDto locationUpdate)
        {
            var location = await _locationRepo.GetLocation(id);

            if (location == null)
                return NotFound(new { error = new string[] { "Location Not Found" } });


            if(locationUpdate.LocationTypeId > 0)
            {
                var locationType = await _locationRepo.GetLocationType(locationUpdate.LocationTypeId);

                if(locationType == null)
                    return NotFound(new { error = new string[] { "Location Type Not Found" } });

                location.LocationType = locationType;

            }

            if (!string.IsNullOrEmpty(locationUpdate.Name))
                location.Name = locationUpdate.Name;

            if (!string.IsNullOrEmpty(locationUpdate.Address))
                location.Address = locationUpdate.Address;

            if (!string.IsNullOrEmpty(locationUpdate.Phone))
                location.Phone = locationUpdate.Phone;

            if(await _locationRepo.Save())
            {
                return Ok(location);
            }

            return BadRequest(new { error = new string[] { "Error Saving Location" } });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _locationRepo.GetLocation(id);

            if (location == null)
                return NotFound(new { error = new string[] { "Location Not Found" } });

            if (location.Inventories.Count > 0)
                ModelState.AddModelError("error", "Location have associated inventories");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            location.IsArchived = true;


            if (await _locationRepo.Save())
                return Ok(location);

            return BadRequest(new { error = new string[] { "Error Deleting Location" } });
        }



        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateLocationType(int id, [FromBody] LocationTypeDto locationTypeUpdate)
        {
            var locationType = await _locationRepo.GetLocationType(id);

            if (locationType == null)
                return NotFound(new { error = new string[] { "Inventory Type not found" } });


            if (string.IsNullOrEmpty(locationTypeUpdate.Name))
                ModelState.AddModelError("error", "Location Type name is required");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            locationType.Name = locationTypeUpdate.Name;

            if(await _locationRepo.Save())
            {
                var returnedLocationType = _mapper.Map<LocationTypeDto>(locationType);
                return Ok(returnedLocationType);
            }

            return BadRequest(new { error = new string[] { "Inventory Type not found" } });

        }

        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteLocationType(int id)
        {

            var locationType = await _locationRepo.GetLocationType(id);

            if (locationType == null)
                return NotFound(new { error = new string[] { "Location Type not found" } });

            if (locationType.Locations.Count > 0)
                return BadRequest(new { error = new string[] { "Location Type have associated Location cannot be deleted" } });


            _locationRepo.Delete(locationType);

            if (await _locationRepo.Save())
                return Ok();


            return BadRequest(new { error = new string[] { "Location Type cannot be deleted" } });

        }
       
    }
}