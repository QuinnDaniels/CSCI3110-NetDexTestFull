/* NetDexTest-01-MVC
 * Model-View-Controller (MVC) Web Application Project
 * (For the Web API Project, see: NetDexTest-01)
 * 
 * CSCI 3110-001 - Advanced Topics in Web Development - Spring 2025
 * 
 * 
 * * Sends API Requests
 * * Hosts Views
 * * Uses JavaScript (.js) files to handle data
 * 
 * 
 */
using Microsoft.AspNetCore.Authentication.Cookies;
using NetDexTest_01_MVC.Services;

namespace NetDexTest_01_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
// see: yt video - 5 ways to use httpclient
// add endpoint explorer
// add swaggergen            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IApiCallerService, ApiCallerService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            
            
            builder.Services.AddSingleton<HttpClient>();
            //add DI for IHttpContextAccessor to access HttpContext object
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/auth/login";  // the page where our app will be redirected to in case the cookie has expired or the cookie is not found
                    options.Cookie.HttpOnly = true;     // ensures that the cookie can't be accessed via javascript on the client browser
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;        // ensures that the cookie transfer (as well as authentication) happens over HTTPS and not HTTP
                    options.Cookie.SameSite = SameSiteMode.Strict;      // ensures that the browser does not send this cookie as part of a request unless that request is directed to our server/domain

                    options.Events.OnValidatePrincipal = async context =>   // whenever the OnValidatePrincipal event is triggered
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                        await authService.TakeActionIfTokenExpired(context);    //we also want to execute the authService.TakeActionIfTokenExpired() method
                    };

                });

            // By default, the cookie is HttpOnly and Secure which is what we want.
            // However, we have anyway gone ahead and explicitly set both properties above.
            // The Samesite.Strict property needs to be set explicitly though, as the default is Samesite.Lax in .NET.
            // These three properties HttpOnly, Secure and SameSite.Strict tell the browser how we want our cookies to be handled by it.
            // https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                //pattern: "{controller=Landing}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
