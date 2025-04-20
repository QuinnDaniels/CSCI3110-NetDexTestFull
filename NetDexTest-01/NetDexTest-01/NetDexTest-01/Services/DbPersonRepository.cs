using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
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


        public DbPersonRepository(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ApplicationUser> logger,
            IUserRepository userRepo
            )
            {
                _db = db; //context
                _userManager = userManager;
                _roleManager = roleManager;
                _userStore = userStore;
                _signInManager = signInManager;
                _logger = logger;
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
            return await _db.Person.ToListAsync(); // I/O Bound Operation
        }
        public async Task<ICollection<Person>> ReadAllPeopleAsync(DexHolder dexHolder)
        {
            return await _db.Person.Where(p => p.DexHolder == dexHolder).ToListAsync(); // I/O Bound Operation
        }
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        public async Task<ICollection<Person>> ReadAllPeopleAsync(ApplicationUser user)
        {
            return await _db.Person.Where(p => p.DexHolder == user.DexHolder).ToListAsync(); // I/O Bound Operation
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
                return await _db.Person.Where(p => p.DexHolder == dex).ToListAsync(); // I/O Bound Operation

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
                return await _db.Person.Where(p => p.DexHolder == dex).ToListAsync(); // I/O Bound Operation

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




        public async Task AddPersonPersonAsync(Person parent, Person child, PersonPerson ppIn)
        {
            await Console.Out.WriteLineAsync("\n\n\n--------- AddPersonAsync -----------\n\n");
            await Console.Out.WriteLineAsync($"\n parent\t{parent.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n child\t{child.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.RelationshipDescription}");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.PersonParent.Nickname} -> {ppIn.PersonChild.Nickname}");


            parent.PersonParents.Add(ppIn);
            child.PersonChildren.Add(ppIn);
            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");
            await SaveChangesAsync();
            await Console.Out.WriteLineAsync("\n\n\n---attempted to add. check results-------\n\n");

            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");

        }
        public async Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn)
        {
            bool noMatch = !FindMatch(ppIn);
            if (noMatch)
            {
                await AddPersonPersonAsync(parent, child, ppIn);
            }
            return noMatch;
        }
        public async Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn, string desc)
        {
            bool noMatch = !FindMatch(ppIn, desc);
            if (noMatch)
            {
                ppIn!.RelationshipDescription = desc;
                await AddPersonPersonAsync(parent, child, ppIn);
            }
            return noMatch;
        }
        public async Task<bool> AddPersonPersonCheckAsync(PersonPerson ppIn, string desc)
        {
            PersonPerson ppInNew = new PersonPerson(ppIn, desc);

            bool noMatch1 = !FindMatch(ppInNew);
            bool noMatch2 = !FindMatch(ppIn.PersonParent, ppIn.PersonChild, desc);
            if (noMatch2) // && noMatch1)
            {
                await AddPersonPersonAsync(ppIn.PersonParent, ppIn.PersonChild, ppInNew);
                return true;
            }
            else return false;
        }

        /*------------------------------------------------------------*/



        public void AddPersonPerson(Person parent, Person child, PersonPerson ppIn)
        {


            Console.WriteLine("\n\n\n--------- AddPerson -----------\n\n");
            Console.WriteLine($"\n parent\t{parent.Nickname} \n");
            Console.WriteLine($"\n child\t{child.Nickname} \n");
            Console.WriteLine($"\n PersonPerson:\t{ppIn.RelationshipDescription}");
            Console.WriteLine($"\n PersonPerson:\t{ppIn.PersonParent.Nickname} -> {ppIn.PersonChild.Nickname}");

            parent.PersonParents.Add(ppIn);
            child.PersonChildren.Add(ppIn);
            Console.WriteLine("\n\n\n--------------------\n\n");

        }

        public bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn)
        {
            bool noMatch = !FindMatch(ppIn);
            if (noMatch)
            {
                AddPersonPerson(parent, child, ppIn);
            }
            return noMatch;
        }

        public bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn, string desc)
        {
            bool noMatch = !FindMatch(ppIn, desc);
            if (noMatch)
            {
                ppIn!.RelationshipDescription = desc;
                AddPersonPerson(parent, child, ppIn);
            }
            return noMatch;
        }


        public bool AddPersonPersonCheck(PersonPerson ppIn, string desc)
        {
            PersonPerson ppInNew = new PersonPerson(ppIn, desc);

            bool noMatch1 = !FindMatch(ppInNew);
            bool noMatch2 = !FindMatch(ppIn.PersonParent, ppIn.PersonChild, desc);
            if (noMatch2) // && noMatch1)
            {
                AddPersonPerson(ppIn.PersonParent, ppIn.PersonChild, ppInNew);
                return true;
            }
            else return false;
        }


        public bool FindMatch(PersonPerson ppIn)
        {
            string desc = string.Empty;
            if (ppIn.RelationshipDescription != null && ppIn.RelationshipDescription != string.Empty)
            {
                desc = ppIn.RelationshipDescription;
            }
            bool outTf = FindMatch(ppIn, desc);
            return outTf;

        }
        public bool FindMatch(PersonPerson ppIn, string desc)
        {
            Person p1 = ppIn.PersonParent;
            Person p2 = ppIn.PersonChild;

            bool outTf = FindMatch(p1, p2, desc);
            return outTf;

        }
        public bool FindMatch(Person p1, Person p2)
        {
            string desc = string.Empty;
            bool outTf = FindMatch(p1, p2, desc);
            return outTf;

        }
        public bool FindMatch(Person p1, Person p2, string desc)
        {
            bool outTf = _db.PersonPerson
                            .Any(pp => pp.PersonParent == p1
                                    && pp.PersonChild == p2
                                    && pp.RelationshipDescription == desc);
            return outTf;
        }





        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
