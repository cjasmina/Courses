using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Courses.Contexts;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;
using Microsoft.AspNetCore.Diagnostics;
using Courses.Helpers;
using System.Threading.Tasks;
using Rotativa.AspNetCore;

namespace Courses
{
    public class Startup
    {
        private const string ConnectionString = "Courses";

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var connectionString = _configuration.GetConnectionString(ConnectionString);
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
            services.AddFlashMessage();
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

            app.UseExceptionHandler(options => options.Run(context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                if (exception != null)
                {
                    MailSender.Send("fit.rs1.seminarski.rad@gmail.com", "Greška", $"Dogodila se nova greška u aplikaciji!<br/><br/>Message: {exception.Message}<br/>Stack Trace: {exception.StackTrace}");
                }

                return Task.CompletedTask;
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
