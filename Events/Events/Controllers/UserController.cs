using AutoMapper;
using Business.DTO;
using Business.Services;
using Events.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Events.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UserController(SubService subService, IMapper mapper) : ControllerBase
    {
        private readonly SubService _subService = subService;
        private readonly IMapper _mapper = mapper;
        [HttpPost("/register/{eventId}")]
        [Authorize]
        public IActionResult CreateSubscription(int eventId)
        {
            SubDTO subDTO = _subService.CreateSub(eventId, User?.Identity?.Name);
            return Ok();
        }

        [HttpGet("/subscription/{eventId}")]
        public List<UserViewModel> GetSubsById(int eventId)
        {
            List<UserViewModel> subs = [];
            foreach (var item in _subService.GetSubsById(eventId))
            {
                subs.Add(_mapper.Map<UserViewModel>(item));
            }
            return subs;
        }

        [HttpGet("{id}")]
        public UserDTO GetUser(int id)
        {
            return _subService.GetUserById(id);
        }

        [HttpDelete("/unregister/{eventId}")]
        [Authorize]
        public IActionResult DeleteSubscription(int eventId)
        {
            _subService.DeleteSub(eventId, User?.Identity?.Name);
            return NoContent();
        }
    }
}
