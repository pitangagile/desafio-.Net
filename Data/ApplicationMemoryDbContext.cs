using Microsoft.EntityFrameworkCore;

namespace Data
{
	public class ApplicationMemoryDbContext : ApplicationDbContext<ApplicationMemoryDbContext>
	{
		public ApplicationMemoryDbContext(DbContextOptions<ApplicationMemoryDbContext> context) : base(context)
		{
		}
	}
}
