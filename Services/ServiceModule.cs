using Autofac;
using System;

namespace Services
{
    public class ServiceModule: Module 
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            base.Load(builder);
        }
    }
}
