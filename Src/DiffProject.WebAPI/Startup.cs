using DiffProject.Application.CommandHandlers.Notifications;
using DiffProject.Domain.AggregateModels.ComparisonAggregate.RepositoryInterfaces;
using DiffProject.Infrastructure.DataPersistence;
using DiffProject.WebAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace DiffProject.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiffProject.WebAPI", Version = "v1" });
            });

            // Registering the custom filters
            services.AddScoped<NotificationsFilter>();
            services.AddScoped<ExceptionFilter>();

            // Registering the Notification Context
            services.AddScoped<INotificationContext, NotificationContext>();

            // Registering the repositories
            services.AddScoped<IBinaryDataRepository, BinaryDataRepository>();
            services.AddScoped<IComparisonResultRepository, ComparisonResultRepository>();

            // Registering the Mediatr service
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            // Registering te Database Context woth the UseInMemory option.
            services.AddDbContext<DiffDbContext>(options => options.UseInMemoryDatabase(databaseName: "DiffDbDatabase"));
        }

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
