using AutoMapper;
using Domains;
using FluentValidation;
using Infrastructure;
using Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioDotNET
{
	public class AuthenticationController : BaseController
	{
		private readonly IMapper _mapper;
		protected readonly IValidator _validator;
		private readonly IApplicationUserService _service;
		private readonly BaseService<ApplicationUser> _redis;

		public AuthenticationController(IApplicationUserService service, IMapper mapper, IValidator<ApplicationUserDto> validator, BaseService<ApplicationUser> redis)
		{
			this._mapper = mapper;
			this._validator = validator;
			this._service = service;
			this._redis = redis;
		}

		[HttpPost("signup")]
		public async Task<IActionResult> CreateUserAsync([FromBody] SignupDto dto, [FromServices] IValidator<SignupDto> validator)
		{
			var validated = await validator.ValidateAsync(dto);
			if (!validated.IsValid)
				return UnprocessableEntity(validated.Errors.Select(e => new { message = e.ErrorMessage, statusCode = e.ErrorCode }).Distinct());

			var user = this._mapper.Map<ApplicationUser>(dto);
			IdentityResult result = await _service.CreateAsync(user);

			if (result.Succeeded)
			{
				return Ok(new { message = "successful operation", statusCode = 200 });
			}
			return UnprocessableEntity(result.Errors.Where(e => e.Code.ToUpper() == "DuplicateEmail".ToUpper()).Select(e => new { message = "E-mail already exists", statusCode = 422 }).Distinct());
		}

		[HttpGet("logout")]
		public async Task<IActionResult> SignOutAsync()
		{
			await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			return Content("done");
		}

		[HttpPost("signin")]
		public async Task<IActionResult> LoginAsync([FromBody] SigninDto dto, [FromServices]SigningConfigurations signingConfigurations, [FromServices]TokenConfigurations tokenConfigurations, [FromServices] IApplicationUserService userService, [FromServices] IValidator<SigninDto> validatorSignin)
		{
			var cache = this._redis.GetCache(dto.Email);

			var validated = await validatorSignin.ValidateAsync(dto);

			if (!validated.IsValid)
				return UnprocessableEntity(validated.Errors.Select(e => new { message = e.ErrorMessage, statusCode = e.ErrorCode }).Distinct());

			if (!cache.IsNull())
			{
				var response = this._mapper.Map<ApplicationUserDto>(cache);
				return GenerateResultToken((ApplicationUser)cache, signingConfigurations, tokenConfigurations, response);
			}

			var result = await _service.SignInAsync(dto.Email, dto.Password);

			if (result.Succeeded)
			{
				var user = (await userService.GetAllIncludingAsync((e => e.Phones))).SingleOrDefault(e => e.Email.ToUpper() == dto.Email.ToUpper());
				if (user == null)
				{
					return NotFound(new { message = "User Not Found", statusCode = 404 });
				}
				user.LastLogin = DateTime.Now;
				await userService.UpdateAsync(user);

				var response = this._mapper.Map<ApplicationUserDto>(user);
				this._redis.SaveCache(user.Email, user);
				return GenerateResultToken(user, signingConfigurations, tokenConfigurations, response);
			}
			return UnprocessableEntity(new { message = "Invalid e-mail or password", statusCode = 422 });
		}

		[UserIdentityValidatorsMiddleware, Authorize("Bearer")]
		[HttpPost("me")]
		public async Task<IActionResult> Me([FromQuery(Name = "email")] string email, [FromServices] IApplicationUserService userService)
		{
			var cache = this._redis.GetCache(email);
			if (!cache.IsNull())
			{
				var response = this._mapper.Map<ApplicationUserDto>(cache);
				return Ok(response);
			}

			var user = (await userService.GetAllIncludingAsync((e => e.Phones))).SingleOrDefault(e => e.Email.ToUpper() == email.ToUpper());
			var result = _mapper.Map<ApplicationUserDto>(user);

			if (result != null)
			{
				return Ok(result);
			}

			return BadRequest();
		}

		[UserIdentityValidatorsMiddleware, Authorize("Bearer")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> ExcludeAccountAsync([FromRoute] long id)
		{
			var account = await this._service.FindByIdAsync(id);
			var result = await this._service.RemoveByIdAsync(id);
			if (result != null)
			{
				this._redis.DeleteCache(account.Email);
				return Ok();
			}
			return BadRequest();
		}

		[UserIdentityValidatorsMiddleware, Authorize("Bearer")]
		[HttpPut("password")]
		public async Task<IActionResult> ChangePassword([FromBody] ApplicationUserChangePassword dto)
		{
			var user = await this._service.ChangePasswordAsync(dto.Email, dto.CurrentPassword, dto.NewPassword);

			this._redis.UpdateCache(user.Email, user);

			return Ok();
		}

		[UserIdentityValidatorsMiddleware, Authorize("Bearer")]
		[HttpPut("email")]
		public async Task<IActionResult> ChangeEmail([FromBody] ApplicationUserChangeEmailDto dto)
		{
			var user = await this._service.ChangeEmailAsync(dto.CurrentEmail, dto.NewEmail);

			this._redis.DeleteCache(dto.CurrentEmail);
			this._redis.SaveCache(dto.CurrentEmail, user);

			return Ok();
		}

		private IActionResult GenerateResultToken(ApplicationUser user, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, ApplicationUserDto dto)
		{
			var objectToken = user.GenerateToken(tokenConfigurations, signingConfigurations);

			return Ok(new { user = dto, token = objectToken });
		}
	}
}