using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepository, IMapper mapper)
        {
            _npRepository = npRepository;
            _mapper = mapper;
        }

        [HttpGet]
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

        [HttpGet("{id:int}", Name = "GetNationalPark")]
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

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            var success = _npRepository.CreateNationalPark(nationalPark);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {nationalPark.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new {id = nationalPark.Id}, nationalPark);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
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