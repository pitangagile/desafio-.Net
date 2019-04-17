using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Infrastructure
{
	public static class ApplicationBuilderExtension
	{
		public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(builder =>
			{
				builder.Run(async context =>
				{
					var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (exceptionHandlerFeature != null)
					{
						context.Response.ContentType = "application/json";

						var json = new
						{
							Message = exceptionHandlerFeature.Error,
							Detailed = (int)HttpStatusCode.InternalServerError
						};

						await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
					}
				});
			});
		}
	}
}
