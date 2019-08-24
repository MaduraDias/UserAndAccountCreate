using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using ZipPay.Users.BusinessService;
using ZipPay.Users.DataService;
using ZipPay.Users.DataServices.Repositories;
using ZipPay.Users.BusinessService.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,DefaultDBContext dbContext, IHostingEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    logger.Log(LogLevel.Error, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(CreateProblemDetails(env, context, exceptionHandlerFeature.Error)));
                });
            });


            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to migrate the database");
                throw;
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }


        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddDbContext<DefaultDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultDBContext")), ServiceLifetime.Scoped);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        }

        private ProblemDetails CreateProblemDetails(IHostingEnvironment env, HttpContext context, Exception ex)
        {
            ProblemDetails result = new ProblemDetails();

            switch (ex)
            {
                case BusinessValidationException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result.Title = "Validation Error";
                    result.Detail = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    result.Title = "Unexpected error occured";
                    result.Detail = !env.IsProduction() ? ex.ToString() : null;
                    break;
            }

            result.Status = context.Response.StatusCode;

            return result;
        }
    }
}

