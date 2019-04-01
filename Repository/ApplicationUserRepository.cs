using System;
using System.Collections.Generic;
using System.Text;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class ApplicationUserRepository: RepositoryBase<ApplicationUser>, IApplicationUserService
	{
		public ApplicationUserRepository(DbContext context) : base(context)
		{
		}
	}
}
