/* NetDexTest-01
 * API Web Application Project
 * (For the MVC App Project, see: NetDexTest-01-MVC)
 * 
 * CSCI 3110-001 - Advanced Topics in Web Development - Spring 2025
 * 
 * 
 * * Hosts Database
 * * Consumes API Requests
 * 
 * 
 * JWT Tutorial:
 *      https://codewithmukesh.com/blog/aspnet-core-api-with-jwt-authentication/
 * 
 */



using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetDexTest_01.Contexts;
using NetDexTest_01.Models.Entities;
using Microsoft.Extensions.Configuration;
using NetDexTest_01.Services;
using NetDexTest_01.Settings;
using NetDexTest_01.Constants;

using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;




namespace NetDexTest_01
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //var host = new WebHostBuilder(args).Build(); //args

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                //////try
                //////{
                    //Seed Default Users - NOTE: JWT Tutorial Method
                    //var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    //await ApplicationDbContextSeed.SeedEssentialsAsync(userManager, roleManager);

                    //Seed the Database - QUINN's method
                    await SeedDataAsync(services);
                //////}
                //////catch (Exception ex)
                //////{
                //////    var logger = loggerFactory.CreateLogger<Program>();
                //////    logger.LogError(ex, "An error occurred seeding the DB.");
                //////    Console.WriteLine($"\n\n-----------------------\n\n"
                //////        + $"An error occurred while seeding the database. {ex.Message}"
                //////        + $"\n\n-----------------------\n\n"
                //////        + $"{ex}"
                //////        + $"\n\n-----------------------\n\n");

                //////}
            }
            host.Run();

        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

        /// <summary>
        /// Quinn - seed the database
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        static async Task SeedDataAsync(IServiceProvider servicesIn)
        {

            ////var services = scope.ServiceProvider;
            try
            {
                var initializer = servicesIn.GetRequiredService<Initializer>();
                await initializer.SeedUsersAsync();
        }
            catch (Exception ex)
            {
                var logger = servicesIn.GetRequiredService<ILogger<Program>>();
                logger.LogError(
                     $"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n"
                    +$"An error occurred while seeding the database. {ex.Message}"
            //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
            //    Console.WriteLine(
                    +$"\n\n----- 1 -------- SEED DATA ASYNC --- console ----\n\n"
            //        + $"An error occurred while seeding the database. {ex.Message}"
            //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                    + $"{ex}"
                    + $"\n\n---- end ------- SEED DATA ASYNC --- console ----\n\n");
            }
}

    }

        public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IConfiguration _configuration { get; }

        //public MvcOptions GetMvcOptions()
        //{
        //    return MvcOptions;
        //}



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //, MvcOptions mvcOptions)
        {
            //Configuration from AppSettings
            services.Configure<JWT>(_configuration.GetSection("JWT"));


            var jwtSettings = new JwtSettings();
            _configuration.Bind("JwtSettings", jwtSettings);

            services.AddSingleton(jwtSettings);
            services.AddTransient<JwtTokenCreator>();


            //User Manager Service
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IUserService, UserService>();


            var connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //Adding DB Context with MSSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))) ;

            services.AddDatabaseDeveloperPageExceptionFilter();


            //Adding Athentication - JWT
            //services.AddAuthentication(options =>
            services.AddAuthentication(i =>

            {
                /*
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidAudience = _configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
                    };

                    // TODO : COOKIE HTTPONLY
                    //try
                    //{

                    //    o.Events.OnMessageReceived = context =>
                    //    {

                    //        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                    //        {
                    //            context.Token = context.Request.Cookies["X-Access-Token"];
                    //        }

                    //        return Task.CompletedTask;
                    //    };
                    //}
                    //catch (Exception ex) { Console.WriteLine($"\n\n\nEXCEPTION OCCURRED: {ex} \n\n\n\n"); }


                    //o.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = ctx =>
                    //    {
                    //        ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                    //        if (!string.IsNullOrEmpty(accessToken))
                    //            ctx.Token = accessToken;
                    //        return Task.CompletedTask;
                    //    }
                    //};
                })
                //.AddCookie(options =>
                //{
                //    options.Cookie.SameSite = SameSiteMode.Strict;
                //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                //    options.Cookie.IsEssential = true;
                //})
                ;*/


                i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                i.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = jwtSettings.Expire
                    };
                    options.SaveToken = true;
                    options.Events = new JwtBearerEvents();
                    options.Events.OnMessageReceived = context => {

                        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                        {
                            context.Token = context.Request.Cookies["X-Access-Token"];
                        }

                        return Task.CompletedTask;
                    };
                })
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();



            services
                .AddDefaultIdentity<ApplicationUser>(
                    options => options.SignIn.RequireConfirmedAccount = false)    // SOURCE: 04>02_AuthorizationAuthenticationIdentity
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews().AddMvcOptions(options => options.EnableEndpointRouting = false);

            //services.AddMvc();

            // Setting to allow app.MapControllerRoute equivalent (app.UseMvc)
            //mvcOptions.EnableEndpointRouting = false;

            // Add Custom Services to the container
            services.AddScoped<IUserRepository, DbUserRepository>();
            services.AddScoped<Initializer>();
            services.AddScoped<Authorization>();
            services.AddScoped<ApplicationDbContextSeed>();
            services.AddScoped<IUserService, UserService>();


            // Allow postman to send requests
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                   builder =>
                   {
                       // configure the API app to allow requests from external application url (localhost)
                       builder.WithOrigins(
                           "https://web.postman.co",
                           "https://localhost:7005",    // NetDexTest-01-MVC
                           "http://localhost:5134"      // NetDexTest-01-MVC
                           )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                   });
            });


        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage(); // from JWT Tutorial
                // app.UseSwagger();
                // app.UseSwaggerUI();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable Cross Origin Resource Sharing
            // SEE ABOVE: Allows to send requests via Postman, Curl.exe, or an MVC Application
            app.UseCors();


            app.UseAuthentication(); //SOURCE: 02>04-AuthorizationAuthenticationIdentity
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // NOTE: MapControllerRoute

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "custom",
                //    template: "{area:my area name}/{controller=AdminHome}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");
            //app.MapRazorPages();

        }

    }
}





























