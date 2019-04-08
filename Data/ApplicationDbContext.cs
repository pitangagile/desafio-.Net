using Domains;
using Mapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
	public abstract class ApplicationDbContext<TContext>: IdentityDbContext<ApplicationUser, ApplicationRole, long,
		ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken> where TContext : DbContext
	{
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<ApplicationUserPhones> ApplicationUserPhones { get; set; }
		public DbSet<Phone> Phones { get; set; }
		public DbSet<ApplicationRole> ApplicationRoles { get; set; }
		public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
		public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
		public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
		public DbSet<ApplicationRoleClaim> ApplicationRoleClaims { get; set; }
		public DbSet<ApplicationUserToken> ApplicationUserTokens { get; set; }

		public ApplicationDbContext(DbContextOptions<TContext> context): base(context)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
			modelBuilder.Entity<ApplicationUser>(b =>
			{
				b.ToTable("ApplicationUsers");
			});
			modelBuilder.Entity<ApplicationUserClaim>(b =>
			{
				b.ToTable("ApplicationUserClaims");
			});
			modelBuilder.Entity<ApplicationUserLogin>(b =>
			{
				b.ToTable("ApplicationUserLogins");
			});
			modelBuilder.Entity<ApplicationUserToken>(b =>
			{
				b.ToTable("ApplicationUserTokens");
			});
			modelBuilder.Entity<ApplicationUserRole>(b =>
			{
				b.ToTable("ApplicationUserRoles");
			});
			modelBuilder.Entity<ApplicationRole>(b =>
			{
				b.ToTable("ApplicationRoles");
			});
			modelBuilder.Entity<ApplicationRoleClaim>(b =>
			{
				b.ToTable("ApplicationRoleClaims");
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			//https://imasters.com.br/back-end/advanced-repository-pattern-com-entity-framework-core
			//optionsBuilder.UseLazyLoadingProxies();

		}
	}
}
