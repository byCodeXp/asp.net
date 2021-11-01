using Api.Data;
using Api.Data.Entities;
using Api.Models.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FishController : ControllerBase
    {
        private readonly DataContext _context;

        public FishController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var fish = _context.FishSet.Adapt<IEnumerable<FishDto>>();
            return Ok(fish);
        }

        [HttpGet("{id}")]
        public ActionResult<FishDto> GetFish([FromRoute] Guid id)
        {
            var fish = _context.Find<Fish>(id);

            if (fish == null)
            {
                return NotFound($"Fish with id {id} was not found");
            }

            return fish.Adapt<FishDto>();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] FishDto fishVm)
        {
            var fish = fishVm.Adapt<Fish>();

            _context.FishSet.Add(fish);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var fish = _context.Find<Fish>(id);

            if (fish == null)
            {
                return NotFound($"Fish with id {id} was not found");
            }

            _context.Remove(fish);
            _context.SaveChanges();

            return Ok();
        }
    }
}
