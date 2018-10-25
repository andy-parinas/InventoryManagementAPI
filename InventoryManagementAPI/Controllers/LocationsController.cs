using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{

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


        [HttpGet("types")]
        public async Task<IActionResult> GetLocationTypes()
        {
            var locationTypes = await _locationRepo.GetLocationTypes();

            var returnedLocationTypes = _mapper.Map<ICollection<LocationTypeDto>>(locationTypes);

            return Ok(returnedLocationTypes);
        }
    }
}