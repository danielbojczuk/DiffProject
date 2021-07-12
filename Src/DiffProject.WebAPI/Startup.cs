using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.WebAPI.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using System;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Infrastructure.DataPersistence;
using Microsoft.EntityFrameworkCore;

namespace DiffProject.WebAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiffProject.WebAPI", Version = "v1" });
            });

            services.AddScoped<NotificationsFilter>();
            services.AddScoped<INotificationContext,NotificationContext>();
            services.AddTransient<IBinaryDataRepository, BinaryDataRepository>();
            services.AddTransient<IComparisonResultRepository, ComparisonResultRepository>();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<DiffDbContext>(options => options.UseInMemoryDatabase(databaseName: "DiffDbDatabase"));
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DiffProject.WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
