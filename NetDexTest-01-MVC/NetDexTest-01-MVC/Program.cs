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


namespace NetDexTest_01_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.Run();
        }
    }
}
