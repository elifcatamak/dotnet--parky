using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
        {
            _npRepository = npRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _npRepository.GetNationalParks();

            var nationalParkDtos = new List<NationalParkDto>();

            foreach (var item in nationalParks)
            {
                nationalParkDtos.Add(_mapper.Map<NationalParkDto>(item));
            }

            return Ok(nationalParkDtos);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="id">The Id of the national park</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var nationalPark = _npRepository.GetNationalPark(id);

            if (nationalPark == null)
            {
                return NotFound();
            }

            var nationalParkDto = _mapper.Map<NationalParkDto>(nationalPark);

            return Ok(nationalParkDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            var exists = _npRepository.NationalParkExists(nationalParkDto.Name);

            if (exists)
            {
                ModelState.AddModelError("", "National Park exists");

                return StatusCode(404, ModelState);
            }

            nationalParkDto.CreateDate = DateTime.UtcNow;
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);

            var success = _npRepository.CreateNationalPark(nationalPark);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark",
                                  new
                                  {
                                      version = HttpContext.GetRequestedApiVersion()?.ToString(), id = nationalPark.Id
                                  },
                                  nationalPark);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            var success = _npRepository.UpdateNationalPark(nationalPark);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int id)
        {
            var exists = _npRepository.NationalParkExists(id);

            if (!exists)
            {
                return NotFound();
            }

            var nationalPark = _npRepository.GetNationalPark(id);
            var success = _npRepository.DeleteNationalPark(nationalPark);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}