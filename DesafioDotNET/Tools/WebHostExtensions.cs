using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DesafioDotNET
{
	public static class WebHostExtensions
	{
		//public static IWebHost UpgradeRegisterDb(this IWebHost webHost)
		//{
		//	var connectionString = new ConfigurationBuilder()
		//		  .SetBasePath(Directory.GetCurrentDirectory())
		//		  .AddJsonFile("appsettings.json")
		//		  .Build().GetConnectionString("RegisterDb");

		//	var options = new DbContextOptionsBuilder<RegisterDbContext>()
		//			.UseSqlServer(connectionString)
		//			.Options;

		//	var dbContext = new RegisterDbContext(options);
		//	dbContext.Database.Migrate();

		//	return webHost;
		//}
	}
}
