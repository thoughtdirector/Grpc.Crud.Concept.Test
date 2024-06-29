using Contracts.DTO;
using CustomValidations;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using Services;
using Services.Services.Contract;

namespace UsersApi
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
            RegisterServices(services);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersApi", Version = "v1" });
            });

            // Configurar conexión a MongoDB
            var mongoConnectionString = Configuration.GetConnectionString("MongoDatabase");
            var databaseName = Configuration["ConnectionStrings:DatabaseName"];
            services.AddScoped<RepositoryDbContext>(provider => new RepositoryDbContext(mongoConnectionString, databaseName));
            RegisterAutoServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        private void RegisterAutoServices(IServiceCollection services)
        {
            services.AddScoped<IService<User, UserForCreationDto, UserForUpdateDto>, UserService>();
            services.AddScoped<UserService>();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddAutoServices(typeof(IRepository<>), typeof(RepositoryManager));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersApi v1"));
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
