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
                ModelState.AddModelError("LocationType", "Location Type not found");

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


    }
}