using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Models;
//using NetDexTest_01.Models.;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;


namespace NetDexTest_01.Services
{


    public partial interface ISocialMediaRepository
    {
    }


    public partial class DbSocialMediaRepository : ISocialMediaRepository
    {
        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IRoleStore<IdentityRole> _roleStore;
        private readonly IUserRepository _userRepo;
        private readonly IPersonRepository _personRepo;
        //private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<ApplicationUser> _logger;
        public IConfiguration _configuration;
        private readonly IToolService _tools;


        /// <summary>
        /// Initializes a new instance of the <see cref="ISocialMediaRepository"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        public DbSocialMediaRepository(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IUserRepository userRepo,
            IPersonRepository personRepo,
            IRoleStore<IdentityRole> roleStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ApplicationUser> logger,
            IConfiguration configuration,
            IToolService tools
            )
        {
            _userRepo = userRepo;
            _personRepo = personRepo;
            _db = db; //context
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _roleStore = roleStore;
            //_emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _tools = tools;

        }









    }


}
