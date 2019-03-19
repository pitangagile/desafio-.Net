using Domains;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IApplicationUserService: IServiceCrud<ApplicationUser>
	{
		Task<IdentityResult> CreateAsync(ApplicationUser user);

		Task<SignInResult> SignInAsync(string email, string password);

		Task<ApplicationUser> ChangePasswordAsync(string email, string currentpassword, string newPassword);

		Task<ApplicationUser> ChangeEmailAsync(string currentEmail, string newEmail);

	}
}
