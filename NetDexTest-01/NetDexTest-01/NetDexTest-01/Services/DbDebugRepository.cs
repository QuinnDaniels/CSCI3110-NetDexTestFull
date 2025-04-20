using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public class DbDebugRepository : IDebugRepository
    {
        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IPersonRepository _personRepo;


        public DbDebugRepository(ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<ApplicationUser> logger,
        IUserRepository userRepo,
        IPersonRepository personRepo
    )
        {
            _db = db; //context
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _userRepo = userRepo;
            _personRepo = personRepo;
        }


        //public async  Task ReadAllUserOne()
        //{
        //    _userManager.

        //}


        public async Task<ICollection<Person>> ReadAllPeopleDebugAsync()
        {
            return await _db.Person
                .Include(p => p.DexHolder)      // eager loading
                .Include(p => p.FullName)
                .Include(p => p.ContactInfo)
                    .ThenInclude(ci => ci.SocialMedias) // then eager loading
                .Include(p => p.RecordCollector)
                    .ThenInclude(ci => ci.EntryItems)
                .Include(p => p.PersonParents)
                    //.ThenInclude(pp => pp)
                .Include(p => p.PersonChildren)
                .ToListAsync();
        }

        public async Task<ICollection<PersonPerson>> ReadAllRelationsDebugAsync()
        {
            return await _db.PersonPerson
                    .Include(pp => pp.PersonParent)
                        .ThenInclude(pa => pa.FullName)
                    .Include(pp => pp.PersonParent)
                        .ThenInclude(pa => pa.DexHolder)
                    .Include(pp => pp.PersonChild)
                        .ThenInclude(pc => pc.FullName)
                    .Include(pp => pp.PersonChild)
                        .ThenInclude(pc => pc.DexHolder)
                   .ToListAsync();
        }


        //public async Task<ICollection<PersonPerson>> ReadAllPeoplePeopleDebugAsync()
        //{
        //    return await _db.PersonPerson
        //            .Include(pp => pp.PersonParent)
        //                .ThenInclude(pa => pa.FullName)
        //            .Include(pp => pp.PersonParent)
        //                .ThenInclude(pa => pa.DexHolder)
        //            .Include(pp => pp.PersonChild)
        //                .ThenInclude(pc => pc.FullName)
        //            .Include(pp => pp.PersonChild)
        //                .ThenInclude(pc => pc.DexHolder)
        //           .ToListAsync();
        //}


        /// <summary>
        /// gets ApplicationUser eagerload
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<ApplicationUser>> ReadAllUsersDebugAsync()
        {
            return await _db.Users
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                        .ThenInclude(p => p.FullName)
                .ToListAsync();
        }


        public async Task<ICollection<DexHolder>> ReadAllDexHoldersDebugAsync()
        {
            return await _db.DexHolder
                .Include(dh => dh.ApplicationUser)
                .Include(dh => dh.People)
                    .ThenInclude(p => p.FullName)
                .ToListAsync();
        }


        public async Task<ICollection<ApplicationUser>> ReadAbsolutelyAllDebugAsync()
        {
            return await _db.Users
                //.Include(u => u!.Roles)

                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                        .ThenInclude(p => p.FullName)

                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                        .ThenInclude(p => p.ContactInfo)
                            .ThenInclude(ci => ci.SocialMedias) // then eager loading

                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)

                        .ThenInclude(p => p.RecordCollector)
                            .ThenInclude(ci => ci.EntryItems)
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                        .ThenInclude(p => p.PersonChildren)
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh!.People)
                        .ThenInclude(p => p.PersonParents)

                .ToListAsync();
        }


    }
}
