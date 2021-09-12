using CoinJar.Api.Settings;
using CoinJar.DB;
using CoinJar.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CoinJar.Domain.Commands;
using Microsoft.OpenApi.Models;
using CoinJar.Domain.Queries;

namespace CoinJar.Api
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
            services.Configure<HashSet<CoinSettings>>(Configuration.GetSection("Coins"));
            services.AddDbContext<CoinJarContext>
            (options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICommandHandler<DepositCommand>, DepositCommandHandler>();
            services.AddTransient<ICommandHandler<CreateJarCommand>, CreateJarCommandHandler>();
            services.AddTransient<IQueryHandler<BalanceQuery, decimal>, BalanceQueryHandler>();
            services.AddSingleton<IMediator, Mediator>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
