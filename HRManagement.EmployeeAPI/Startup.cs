using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using HRManagement.EmployeeAPI.DataAccess;
using HRManagement.EmployeeAPI.MiddleWare;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace HRManagement.EmployeeAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<EmployeeDBContext>(opt => opt.UseInMemoryDatabase("EmployeePayroll").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
                .AddEntityFrameworkSqlServer();

            services.AddControllers();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "EmployeeAPI",
                    Version = "v1",
                    Description = "EmployeeAPI"
                }); 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                var Response = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotFound };
                //app.UseExceptionHandler("/error");
            }

            app.UseStatusCodePagesWithReExecute("{statuscode}");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            //app.UseExceptionHandler(a => a.Run(async context =>
            //{
            //    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            //    var exception = feature.Error;

            //    var result = JsonConvert.SerializeObject(new { error = exception.Message });
            //    context.Response.ContentType = "application/json";
            //    await context.Response.WriteAsync(result);
            //}));

            app.UseHttpsRedirection();
            //app.UseHttpCacheHeaders();
            app.UseResponseCaching();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee API V"); });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
