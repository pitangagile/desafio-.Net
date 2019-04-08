using Microsoft.EntityFrameworkCore;

namespace Data
{
	public class ApplicationPGDbContext : ApplicationDbContext<ApplicationPGDbContext>
	{
		public ApplicationPGDbContext(DbContextOptions<ApplicationPGDbContext> context) : base(context)
		{
		}
	}
}
