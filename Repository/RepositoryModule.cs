using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Domains;

namespace Repository
{
	public class RepositoryModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
			builder.RegisterType<ApplicationUserRepository>().As<RepositoryBase<ApplicationUser>>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

			base.Load(builder);
		}
	}
}
