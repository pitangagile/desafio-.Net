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
			builder.RegisterType<ApplicationUserRepository>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

			base.Load(builder);
		}
	}
}
