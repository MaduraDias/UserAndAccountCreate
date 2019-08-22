using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZipPay.Users.BusinessService;
using ZipPay.Users.Domain;

namespace ZipPay.Users.Api
{
    public class Startup
    {
        private readonly ILogger<Startup> logger;

        public Startup(IConfiguration configuration,ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterDependencies(services);
            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                //errorApp.Run(async context => {
                //   var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                //    logger.LogError(exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Error);

                //   await context.Response.Wri
                //};
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }


        private static void RegisterDependencies(IServiceCollection services)
        {
            services.AddDbContext<DefaultDBContext>(ServiceLifetime.Scoped);
            services.AddScoped<IUserService, UserService>();
        }
    }
}
