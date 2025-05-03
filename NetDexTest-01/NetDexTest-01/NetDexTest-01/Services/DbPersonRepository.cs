using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;

using System.Diagnostics;
using System.Linq.Expressions;

namespace NetDexTest_01.Services
{
    // TODO: do this!
    public partial class DbPersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IToolService _tools;


        public DbPersonRepository(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ApplicationUser> logger,
            IUserRepository userRepo,
            IToolService tools
            )
            {
                _db = db; //context
                _userManager = userManager;
                _roleManager = roleManager;
                _userStore = userStore;
                _signInManager = signInManager;
                _logger = logger;
                _tools = tools;
                _userRepo = userRepo;
            }



        // create a P & Ci,Fn,Rc using Authorixation

        //public async Task<Person?> GetPersonAsync(PropertyField pType, string input, string personNickname)
        //{

        //    switch (pType)
        //    {
        //        case PropertyField.id:
        //            break;
        //        case PropertyField.username:
        //            break;
        //        case PropertyField.email:
        //            break;
        //        default:
        //            break;
        //    }
        //}


        /// NOTE
        /// create()
        /// person = Read PersonAsync
        /// if person not null
        /// person.Recommendations.add(object)
        /// object.Person = person
        /// db.savechanges
        /// fi
        /// return recommendation
        /// end create()
        /// |
        /// |
        /// |
        /// in controller...
        /// -------------
        /// 
        /// pub async Task<IAResult> Create([Bind(Prefix = "id")]int personId)
        /// {
        ///  var person = await _repo.ReadAsync(personId);
        ///  if person null
        ///      return redirect to action
        ///  fi
        ///  
        /// CreateRecommendationVM vm = new()
        /// {
        ///     Person = person
        /// };
        /// 
        /// return View(vm);
        /// }
        ///
        /// 
        /// in CreateRecommendationVM...
        /// 
        /// class CreReccomVM
        /// {
        ///     pub Person? Person
        ///     pub Rating Rating { g s }
        ///     
        ///     ctor 
        /// }
        /// 
        /// end NOTE
        public void NOTEstopper() { }

        public async Task<ICollection<Person>> ReadAllPeopleAsync()
        {
            return await _db.Person
                .Include(p => p.FullName)
                .Include(p => p.ContactInfo)
                    //.ThenInclude(ci => ci.SocialMedias) // then eager loading
                .Include(p => p.RecordCollector)
                    //.ThenInclude(rc => rc.EntryItems)
                .Include(p => p.DexHolder)      // eager loading
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                    //.ThenInclude(pp => pp)
                .ToListAsync(); // I/O Bound Operation
        }



        public async Task<ICollection<Person>> ReadAllPeopleAsync(DexHolder dexHolder)
        {
            return await _db.Person
                .Where(p => p.DexHolder == dexHolder)
                .Include(p => p.FullName)
                .Include(p => p.ContactInfo)
                //.ThenInclude(ci => ci.SocialMedias) // then eager loading
                .Include(p => p.RecordCollector)
                //.ThenInclude(rc => rc.EntryItems)
                .Include(p => p.DexHolder)      // eager loading
                .Include(p => p.PersonParents)
                .Include(p => p.PersonChildren)
                //.ThenInclude(pp => pp)

                .ToListAsync(); // I/O Bound Operation
        }
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        public async Task<ICollection<Person>> ReadAllPeopleAsync(ApplicationUser user)
        {
            return await _db.Person
                .Where(p => p.DexHolder == user.DexHolder)
                .ToListAsync(); // I/O Bound Operation
        }

        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        public async Task<ICollection<Person>> ReadAllPeopleAsync(string inputString)
        {
            DexHolder? dex = new();

            dex = await _userRepo.GetDexHolderByUserNameAsync(inputString);
            if (dex == null)
            {
                dex = await _userRepo.GetDexHolderByUserIdAsync(inputString);
            }
            if (dex == null)
            {
                dex = await _userRepo.GetDexHolderByEmailAsync(inputString);
            }

            if (dex != null)
            {
                return await _db.Person
                    .Where(p => p.DexHolder == dex)
                    .ToListAsync(); // I/O Bound Operation

            }
            else
            {
                throw new NullReferenceException(nameof(dex));
            }

        }

        public async Task<ICollection<Person>> ReadAllPeopleAsync(int dexHolderId)
        {
            DexHolder? dex = await _userRepo.ReadDexByIdAsync(dexHolderId);

            if (dex != null)
            {
                return await _db.Person
                    .Where(p => p.DexHolder == dex)
                    .ToListAsync(); // I/O Bound Operation

            }
            else
            {
                throw new NullReferenceException(nameof(dex));
            }
        }




        // get personparent
        // get personchildren
        // get 



        // read all persons

        // read all persons by username

        // read all persons by userid


        // update/edit


        // delete a person




        //public void AddPersonPersonAsync(Person parent, Person child, PersonPerson ppIn)
        //{
        //    parent.PersonParents.Add(ppIn);
        //    child.PersonChildren.Add(ppIn);

        //}









        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
