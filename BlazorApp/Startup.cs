using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorApp.Data;
using BlazorStrap;
using BlazorApp.Models;
using BlazorApp.Interface;
using Microsoft.Extensions.Options;
using BlazorApp.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net.Http;

namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBootstrapCss();
            services.AddSingleton<WeatherForecastService>();

            services.Configure<CustomerDbSettings>(Configuration.GetSection(nameof(CustomerDbSettings)));
            services.AddSingleton<ICustomerDbSettings>(sp => sp.GetRequiredService<IOptions<CustomerDbSettings>>().Value);

            services.AddSingleton<CustomerService>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddServerSideBlazor().AddCircuitOptions(o => o.DetailedErrors = true);
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                                {
                                options.Authority = "https://demo.identityserver.io/";
                            options.ClientId = "interactive.confidential.short"; // 75 seconds
                            options.ClientSecret = "secret";
                            options.ResponseType = "code";
                                    options.CallbackPath = "/signin-oidc";
                            options.SaveTokens = true;
                            options.GetClaimsFromUserInfoEndpoint = true;

                            options.Events = new OpenIdConnectEvents
                            {
                                OnAccessDenied = context =>
                                {
                                    context.HandleResponse();
                                    context.Response.Redirect("/");
                                    return Task.CompletedTask;
                                }
                            };
                                 });
           
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            // Server Side Blazor doesn't register HttpClient by default
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.      
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri)
                    };
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
