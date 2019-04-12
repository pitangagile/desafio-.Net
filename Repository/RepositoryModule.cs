using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Data;
using Domains;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class RepositoryModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
			builder.RegisterType<ApplicationUserRepository>().As<RepositoryBase<ApplicationUser, ApplicationDbContext<DbContext>>>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

			base.Load(builder);
		}
	}
}
