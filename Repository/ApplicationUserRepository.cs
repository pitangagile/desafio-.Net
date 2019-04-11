using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class ApplicationUserRepository: RepositoryBase<ApplicationUser, ApplicationMemoryDbContext>, IApplicationUserService
	{
		public ApplicationUserRepository(ApplicationMemoryDbContext context) : base(context)
		{
		}
	}
}
