using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using TradeChat.API.Extensions;
using TradeChat.Auth.Extensions;
using TradeChat.Services.Extensions;
using TradeChat.Services.Hubs;

namespace TradeChat.API
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
            services.AddConfigurations(Configuration);

            services.AddSwagger();

            services.AddDocumentDb(Configuration);

            services.AddAppServices();

            services.AddGraphServices(Configuration);

            //services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureAdB2C");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(AzureADDefaults.AuthenticationScheme, options =>
                {
                    options.Bind(Configuration);
                })
                .AddMicrosoftIdentityWebApi(Configuration, "AzureAdB2C");

            services.AddControllers(configure =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                configure.Filters.Add(new AuthorizeFilter(policy));
            });


            services.AddSignalR();

            var origins = Configuration.GetSection("Cors:AllowOrigins").Value.Split(',');
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                .WithOrigins(origins)
                .AllowCredentials();
            }));
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);


            //services.AddScoped<CoinbaseAuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerServices();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GroupHub>("/api/chatsocket");
            });
        }
    }
}
