using Autofac;
using Domains;
using System;

namespace Services
{
    public class ServiceModule: Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
			builder.RegisterType<ApplicationUserService>().As<BaseService<ApplicationUser>>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
			
			base.Load(builder);
        }
    }
}
