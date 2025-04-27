using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace NetDexTest_01.Services
{
    /// <summary>
    /// Choose a category that matches the property field. Used to cut down on redundant methods that use the same Type variables for different properties.
    /// <br />
    /// <list type="bullet">
    /// <item>
    /// <term>id</term>
    /// <description>String StringLength(450) </description>
    /// </item>
    /// <item>
    ///<term>username</term>
    /// <description>String StringLength(256) </description>
    /// </item>
    /// </list>
    /// <br />
    /// <remarks>Example - ApplicationUserId and ApplicationUsername are both strings</remarks>
    /// 
    /// </summary>
    public enum PropertyField
    {
        id,
        username,
        email
    }


    /// <summary>
    /// QoL class for logging errors and bugs
    /// </summary>
    public class QuinnException
    {
        public QuinnException(string? exceptionTitle, Exception ex)
        {
            if (exceptionTitle != null)
            {
                ExceptionTitle = exceptionTitle;
            }
            Message = $"\n\n---------------- {exceptionTitle} --- log ---------\n\n"
                        + $"An error occurred. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- {exceptionTitle} --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- {exceptionTitle} --- console ----\n\n"; 
        }

        public QuinnException(string? exceptionTitle, string? exceptionFlavor, Exception ex)
        {
            if (exceptionTitle != null)
            {
                ExceptionTitle = exceptionTitle;
            }
            if (exceptionFlavor != null)
            {
                ExceptionFlavor = exceptionFlavor;
            }
            Message = $"\n\n---------------- {exceptionTitle} --- log ---------\n\n"
                        + $"An error occurred {exceptionFlavor}. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- {exceptionTitle} --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- {exceptionTitle} --- console ----\n\n" ;
        }



        public string Message { get; set; }
        public string? ExceptionTitle { get; set; } = null;
        public string? ExceptionFlavor { get; set; } = null;

    }






    /// <summary>
    /// The user repository class that interacts with the Db.
    /// </summary>
    /// <inheritdoc cref="IUserRepository"/>
    public partial class DbUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IRoleStore<IdentityRole> _roleStore;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IEmailSender _emailSender;
        public IConfiguration _configuration;
        private readonly IToolService _tools;


        /// <summary>
        /// Initializes a new instance of the <see cref="DbUserRepository"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        public DbUserRepository(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IRoleStore<IdentityRole> roleStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            ILogger<ApplicationUser> logger,
            IConfiguration configuration,
            IToolService tools
            )
        {
            _db = db; //context
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _roleStore = roleStore;
            //_emailStore = GetEmailStore();
            _emailSender = emailSender;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _tools = tools;

        }









        











        public async Task AssignUserToRoleAsync(string userName, string roleName)
        {
            var roleCheck = await _roleManager.RoleExistsAsync(roleName);
            if (!roleCheck)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            var user = await ReadByUsernameAsync(userName);
            if (user != null)
            {
                if (!user.HasRole(roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
        }

        public DexHolder CreateTempDexHolder(string? firstName = null, string? middleName = null, string? lastName = null, string? gender = null, string? pronouns = null)
        {
            var dexHolder = new DexHolder
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Gender = gender,
                Pronouns = pronouns
            };
            return dexHolder;
        }



        //public async Task<string> DebugConsoleWriter(string level, string title, var input)
        //{

        //}





    }
} 

