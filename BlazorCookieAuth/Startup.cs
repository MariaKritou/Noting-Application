using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorCookieAuth.Data;
// ******
// BLAZOR COOKIE Auth Code (begin)
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using SQW;
using SQW.Interfaces;
// BLAZOR COOKIE Auth Code (end)
// ****** 

namespace BlazorCookieAuth
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to 
    // add services to the container.
    // For more information on how to configure your application, 
    // visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // Connect to oracle
      var databaseOptionsSection = Configuration.GetSection("DatabaseOptions");
      services.Configure<DatabaseOptions>(databaseOptionsSection);
      var databaseOptions = databaseOptionsSection.Get<DatabaseOptions>();

      var oraConfig = new SQWOraConfig
      {
        host = databaseOptions.dbServer,
        instance = databaseOptions.dbInstance,
        userName = databaseOptions.userName,
        password = databaseOptions.password
      };

      var sequenceGenerator = new SQWSequenceGenerator();

      SQWOraWorker worker = new SQWOraWorker(oraConfig, sequenceGenerator);
      services.AddSingleton<ISQWWorker>(worker);

      // ******
      // BLAZOR COOKIE Auth Code (begin)
      services.Configure<CookiePolicyOptions>(options =>
            {
              options.CheckConsentNeeded = context => true;
              options.MinimumSameSitePolicy = SameSiteMode.None;
            });
      services.AddAuthentication(
          CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie();
      // BLAZOR COOKIE Auth Code (end)
      // ******

      services.AddRazorPages();
      services.AddServerSideBlazor();
      services.AddSingleton<UserService>();
      services.AddSingleton<NoteService>();

      // ******
      // BLAZOR COOKIE Auth Code (begin)
      // From: https://github.com/aspnet/Blazor/issues/1554
      // HttpContextAccessor
      services.AddHttpContextAccessor();
      services.AddScoped<HttpContextAccessor>();
      services.AddHttpClient();
      services.AddScoped<HttpClient>();
      services.AddAuthorizationCore();
      // BLAZOR COOKIE Auth Code (end)
      // ******
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
        // The default HSTS value is 30 days. 
        // You may want to change this for production scenarios, 
        // see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      // ******
      // BLAZOR COOKIE Auth Code (begin)
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();
      app.UseAuthentication();
      // BLAZOR COOKIE Auth Code (end)
      // ******

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
              // ******
              // BLAZOR COOKIE Auth Code (begin)
              endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
              // BLAZOR COOKIE Auth Code (end)
              // ******
            });
    }
  }
}
