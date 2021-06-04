using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.MemoryStorage;

namespace PostService
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

            var sqlConnectionString = Configuration.GetConnectionString("PostgreSqlConnectionString");
            services.AddDbContext<PostService.Data.PostServiceContext>(options => options.UseNpgsql(sqlConnectionString));
            var storage = new MemoryStorage();
            var options = new BackgroundJobServerOptions { ServerName = "local" };
            JobStorage.Current = storage;
            services.AddHangfire(x => x.UseMemoryStorage());

            RecurringJob.AddOrUpdate(() => System.Diagnostics.Debug.WriteLine("Minute XX"), Cron.Minutely);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PostService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PostService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
