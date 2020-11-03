using Health.API.Infra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Health.API
{
    public class Startup
    {
        readonly string XAuthOrigins = "_XAuthOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //CORS settings
            services.AddCors(options =>
            {
                options.AddPolicy(name: XAuthOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://healthui")  //During dev you may change it to localhost:{portnumber}
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials();
                                  });
            });
            //Enable authorization with cookie base authentication
            services.AddAuthorization(AZ =>
            {
                AZ.DefaultPolicy =
                    new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build();
            }).AddAuthentication(A =>
            {
                A.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                A.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(c =>
            {
                c.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                c.LoginPath = new PathString("/signin");
                c.LogoutPath = new PathString("/signout");
                c.EventsType = typeof(UserAuthenticationEvent);
                c.Cookie.HttpOnly = true;
                c.AccessDeniedPath = new PathString("/unauthorized");
                c.SlidingExpiration = true;
            }); ;
            services.AddScoped<UserAuthenticationEvent>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(XAuthOrigins);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseRouting();

            //HP:2
            app.UseAuthorization();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
