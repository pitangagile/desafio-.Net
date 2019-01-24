using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using System;
using AutoMapper;
using Mapping;
using Services;

namespace DesafioDotNET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<ApplicationDbContext>(options => { options.UseInMemoryDatabase("DesafioDotNet"); });
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddIdentityConfiguration();
            services.AddTokenConfiguration(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy("FrontEnd", builderPolicy => { builderPolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials(); });
            });
            services.AddValidators();
            services.AddGlobalExceptionHandlerMiddleware();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var configMapper = new MapperConfiguration(c =>
            {
                c.AddProfile(new ApplicationMapping());
            });
            services.AddSingleton(configMapper.CreateMapper());

            var builder = new ContainerBuilder();
            builder.RegisterModule<ServiceModule>();
            builder.Populate(services);

            return new AutofacServiceProvider(builder.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseCors("FrontEnd");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseGlobalExceptionHandler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{moreInfo?}");

                routes.MapRoute(
                    name: "aboutPage",
                    template: "more",
                    defaults: new { controller = "About", action = "TellMeMore" });
            });
        }
    }
}
