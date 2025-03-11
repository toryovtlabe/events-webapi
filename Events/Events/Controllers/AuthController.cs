using AutoMapper;
using Business.DTO;
using Business.Services;
using Events.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Events.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController(AuthService authService, IMapper mapper) : ControllerBase
    {
        private readonly AuthService _authService = authService;
        private readonly IMapper _mapper = mapper;

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            RegisterDTO registerDTO = _mapper.Map<RegisterDTO>(registerModel);
            TokensDTO tokensDTO = _authService.Register(registerDTO);
            return Ok(tokensDTO);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            LoginDTO loginDTO = _mapper.Map<LoginDTO>(loginModel);
            TokensDTO tokensDTO = _authService.Login(loginDTO);
            return Ok(tokensDTO);
        }
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] string refreshToken)
        {
            TokensDTO tokensDTO = _authService.Refresh(refreshToken);
            return Ok(tokensDTO);
        }
    }
}
