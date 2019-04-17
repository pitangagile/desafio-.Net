using Autofac;

namespace Repository
{
	public class RepositoryModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<ApplicationUserRepository>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
		}
	}
}
