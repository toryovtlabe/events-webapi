using AutoMapper;
using Business.DTO;
using Business.Services;
using Events.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventsController(IMapper mapper, EventService eventService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly EventService _eventService = eventService;

        [HttpGet]
        public IEnumerable<EventDTO> GetAll([FromQuery] FiltersEventModel eventModel)
        {
            FiltersEventDTO eventDTO = _mapper.Map<FiltersEventDTO>(eventModel);
            IEnumerable<EventDTO> events = _eventService.GetEventsWithFilters(eventDTO);
            Response.Headers["x-total-pages"] = _eventService.GetPages(eventDTO).ToString();
            return events;
        }
        [HttpGet("{id}")]
        public EventDTO GetById(int id)
        {
            return _eventService.GetById(id);
        }

        [HttpPost("/create")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Create([FromBody] EventModel eventModel)
        {
            EventDTO eventDTO = _mapper.Map<EventDTO>(eventModel);
            EventDTO @event = _eventService.CreateEvent(eventDTO);
            return CreatedAtAction(nameof(GetById), new { Id = @event.Id }, @event);
        }

        [HttpPut("/update/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Update([FromBody] EventModel eventModel, int id)
        {
            EventDTO eventDTO = _mapper.Map<EventDTO>(eventModel);
            _eventService.UpdateEvent(id, eventDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Delete(int id)
        {
            _eventService.DeleteEvent(id);
            return NoContent();
        }
        [HttpPost("{id}/upload-image")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UploadImage(int id,[FromBody] string imageBase64)
        {
            await _eventService.UploadImage(id, imageBase64);
            return Ok();
        }
    }
}
