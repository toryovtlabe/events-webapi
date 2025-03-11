using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using DataAccess.Entity;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Service;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class AuthService(UserRepository userRepository, IMapper mapper, IConfiguration config, HashService hashService, IValidator<RegisterDTO> registerValidator, IValidator<LoginDTO> loginValidator)
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _config = config;
        private readonly HashService _hashService = hashService;
        private readonly IValidator<RegisterDTO> _registerValidator = registerValidator;
        private readonly IValidator<LoginDTO> _loginValidator = loginValidator;

        public string GenerateJwtToken(User user, bool isRefreshToken = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

            var expiration = isRefreshToken
                ? DateTime.Now.AddDays(double.Parse(_config["Jwt:RefreshTokenExpireDays"]))
                : DateTime.Now.AddMinutes(double.Parse(_config["Jwt:AccessTokenExpireMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokensDTO Register(RegisterDTO registerDTO)
        {
            FluentValidation.Results.ValidationResult result = _registerValidator.Validate(registerDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            User user = _mapper.Map<User>(registerDTO);
            user.Password = _hashService.HashPassword(user.Password);
            string accessToken = GenerateJwtToken(user);
            string refreshToken = GenerateJwtToken(user, true);
            user.RefreshToken = refreshToken;
            _userRepository.Create(user);
            try
            {
                _userRepository.Save();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Введённые почта или логин уже заняты");
            }
            return new TokensDTO(accessToken, refreshToken);
        }

        public TokensDTO Login(LoginDTO loginDTO)
        {
            FluentValidation.Results.ValidationResult result = _loginValidator.Validate(loginDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            User? user = _userRepository.GetByLogin(loginDTO.Login);

            if (user == null || !_hashService.VerifyPassword(user.Password, loginDTO.Password))
                throw new DirectoryNotFoundException("Логин или пароль неверны");

            string accessToken = GenerateJwtToken(user);
            string refreshToken = GenerateJwtToken(user, true);
            user.RefreshToken = refreshToken;
            _userRepository.Update(user);
            _userRepository.Save();

            return new TokensDTO(accessToken, refreshToken);
        }

        public TokensDTO Refresh(string refreshToken)
        {
            var principal = GetPrincipalFromExiredToken(refreshToken);
            if(principal == null)
            {
                throw new SecurityTokenException("Неверный refresh токен");
            }

            var userLogin = principal.Identity.Name;
            var user = _userRepository.GetByLogin(userLogin);

            if(user == null || !ValidateRefreshToken(user, refreshToken))
            {
                throw new SecurityTokenException("Неверный refresh токен");
            }

            string newAccessToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateJwtToken(user, true);

            return new TokensDTO(newAccessToken, newRefreshToken);

        }

        private ClaimsPrincipal GetPrincipalFromExiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtToken = securityToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Неверный токен");
            }

            return principal;
        }

        private bool ValidateRefreshToken(User user, string refreshToken)
        {
            return user.RefreshToken == refreshToken;
        }
    }
}
