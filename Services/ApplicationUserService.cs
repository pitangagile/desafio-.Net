using Domains;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ApplicationUserService : BaseService<ApplicationUser>, IApplicationUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public ApplicationUserService(DbContext dbContext, IRedisConnectionFactory connectionFactory, UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager, IValidator<ApplicationUser> validator) : base(dbContext, connectionFactory, validator)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
		}

		public async Task<IdentityResult> CreateAsync(ApplicationUser user)
		{
			user.CreatedAt = DateTime.Now;
			IdentityResult result = await _userManager.CreateAsync(user, user.Password);

			return result;
		}

		public async Task<SignInResult> SignInAsync(string email, string password)
		{
			var result = await _signInManager.PasswordSignInAsync(email, password, true, false);

			return result;
		}

		public async Task<ApplicationUser> ChangePasswordAsync(string email, string currentpassword, string newPassword)
		{
			var user = await this._userManager.FindByEmailAsync(email);
			var result = await this._userManager.ChangePasswordAsync(user, currentpassword, newPassword);

			if (!result.Succeeded) {
				throw new Exception(JsonConvert.SerializeObject(result.Errors));
			}

			return user;
		}

		public async Task<ApplicationUser> ChangeEmailAsync(string currentEmail, string newEmail)
		{
			var user = await this._userManager.FindByEmailAsync(currentEmail);
			var token = await this._userManager.GenerateChangeEmailTokenAsync(user, newEmail);
			var result = await this._userManager.ChangeEmailAsync(user, newEmail, token);

			if (!result.Succeeded)
			{
				throw new Exception(JsonConvert.SerializeObject(result.Errors));
			}

			return user;
		}

	}
}
