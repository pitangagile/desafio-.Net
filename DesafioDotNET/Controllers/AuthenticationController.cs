using AutoMapper;
using Domains;
using FluentValidation;
using Infrastructure;
using Mapping;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioDotNET
{
	public class AuthenticationController : BaseController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;
		protected readonly IValidator _validator;
		private readonly IApplicationUserService _service;

		public AuthenticationController(IApplicationUserService service, UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager, IMapper mapper, IValidator<ApplicationUserDto> validator)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._mapper = mapper;
			this._validator = validator;
			this._service = service;
		}

		[HttpPost("signup")]
		public async Task<IActionResult> CreateUserAsync([FromBody] SignupDto dto, [FromServices] IValidator<SignupDto> validator)
		{
			var validated = await validator.ValidateAsync(dto);
			if (!validated.IsValid)
				return UnprocessableEntity(validated.Errors.Select(e => new { message = e.ErrorMessage, statusCode = e.ErrorCode }).Distinct());

			var user = this._mapper.Map<ApplicationUser>(dto);
			user.CreatedAt = DateTime.Now;
			IdentityResult result = await _userManager.CreateAsync(user, user.Password);

			if (result.Succeeded)
			{
				var response = await _userManager.FindByEmailAsync(user.Email);
				if (response != null)
					return Ok(new { message = "successful operation", statusCode = 200 });
				return BadRequest();
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
			//await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
			var cache = this._service.GetCache(dto.Email);

			var validated = await validatorSignin.ValidateAsync(dto);

			if (!validated.IsValid)
				return UnprocessableEntity(validated.Errors.Select(e => new { message = e.ErrorMessage, statusCode = e.ErrorCode }).Distinct());

			if (!cache.IsNull())
			{
				var response = this._mapper.Map<ApplicationUserDto>(cache);
				return GenerateResultToken((ApplicationUser)cache, signingConfigurations, tokenConfigurations, response);
			}

			var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, true, false);

			if (result.Succeeded)
			{
				var user = (await userService.GetAllIncludingAsync((e => e.Phones))).SingleOrDefault(e => e.Email.ToUpper() == dto.Email.ToUpper());
				if (user == null)
				{
					return NotFound(new { message = "User Not Found", statusCode = 404 });
				}
				user.LastLogin = DateTime.Now;
				await userService.UpdateAsync(user, user.Id);

				var response = this._mapper.Map<ApplicationUserDto>(user);
				this._service.SaveCache(user.Email, user);
				return GenerateResultToken(user, signingConfigurations, tokenConfigurations, response);
			}
			return UnprocessableEntity(new { message = "Invalid e-mail or password", statusCode = 422 });
		}

		[UserIdentityValidatorsMiddleware, Authorize("Bearer")]
		[HttpPost("me")]
		public async Task<IActionResult> Me([FromQuery(Name = "email")] string email, [FromServices] IApplicationUserService userService)
		{
			var cache = this._service.GetCache(email);
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

		private IActionResult GenerateResultToken(ApplicationUser user, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, ApplicationUserDto dto)
		{
			var objectToken = user.GenerateToken(tokenConfigurations, signingConfigurations);

			return Ok(new { user = dto, token = objectToken });
		}
	}
}