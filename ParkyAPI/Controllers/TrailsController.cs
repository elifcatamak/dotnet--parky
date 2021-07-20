using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var trails = _trailRepository.GetTrails();

            var trailDtos = new List<TrailDto>();

            foreach (var item in trails)
            {
                var trailDto = _mapper.Map<TrailDto>(item);
                trailDtos.Add(trailDto);
            }

            return Ok(trailDtos);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="id">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var trail = _trailRepository.GetTrail(id);

            if (trail == null)
            {
                return NotFound();
            }

            var trailDto = _mapper.Map<TrailDto>(trail);

            return Ok(trailDto);
        }

        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var trails = _trailRepository.GetTrailsInNationalPark(nationalParkId);

            if (trails == null)
            {
                return NotFound();
            }

            var trailDtos = new List<TrailDto>();

            foreach (var item in trails)
            {
                var trailDto = _mapper.Map<TrailDto>(item);
                trailDtos.Add(trailDto);
            }

            return Ok(trailDtos);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailCreateDto)
        {
            if (trailCreateDto == null)
            {
                return BadRequest();
            }

            var exists = _trailRepository.TrailExists(trailCreateDto.Name);

            if (exists)
            {
                ModelState.AddModelError("", "Trail exists");

                return StatusCode(404, ModelState);
            }

            var trail = _mapper.Map<Trail>(trailCreateDto);
            var success = _trailRepository.CreateTrail(trail);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {trail.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new {id = trail.Id}, trail);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailUpdateDto)
        {
            if (trailUpdateDto == null || id != trailUpdateDto.Id)
            {
                return BadRequest();
            }

            var trail = _mapper.Map<Trail>(trailUpdateDto);
            var success = _trailRepository.UpdateTrail(trail);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record {trail.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            var exists = _trailRepository.TrailExists(id);

            if (!exists)
            {
                return NotFound();
            }

            var trail = _trailRepository.GetTrail(id);
            var success = _trailRepository.DeleteTrail(trail);

            if (!success)
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the record {trail.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}