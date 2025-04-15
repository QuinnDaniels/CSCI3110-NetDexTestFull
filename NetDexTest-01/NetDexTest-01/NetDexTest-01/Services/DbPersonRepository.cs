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


        // create just a person

        // create a person with ContactInfo, FullName, RecordCollector

        public void CreatePersonAsync()
        {

        }



        public async Task<Person?> CreatePersonAsync(ApplicationUser user, string personNickname)
        {
            var pp = await GetPersonByNickNameWithUser(personNickname, user);
            if (pp != null)
            {
                var newPerson = new Person
                {
                    Nickname = personNickname,
                    DexHolder = user.DexHolder,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        {
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();

                try
                {
                    return await GetPersonByNickName(personNickname, user);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }





            }
            else { return null; }

        }
        public async Task<Person?> CreatePersonAsync(DexHolder dex, string personNickname)
        {
            var pp = await GetPersonByNickNameWithDex(personNickname, dex);
            if (pp != null)
            {
                var newPerson = new Person(personNickname, dex);
                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();

                try
                {
                    return await GetPersonByNickName(personNickname, dex);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(qEx.Message);
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }


            public async Task<Person?> CreatePersonAsync(PropertyField pType, string inputProperty, string personNickname)
        {
            DexHolder? dex = new DexHolder();
            switch (pType)
            {
                case PropertyField.id:
                    dex = await _userRepo.ReadDexByIdAsync(inputProperty);
                    break;
                case PropertyField.username:
                    dex = await _userRepo.ReadDexByUsernameAsync(inputProperty);
                    break;
                case PropertyField.email:
                    throw new ArgumentException();
                default:
                    throw new ArgumentException();
            }

            if (dex != null) 
            {
                var dexId = dex.Id;
                var newPerson = new Person
                {
                    Nickname = personNickname,
                    DexHolder = dex,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        { 
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" } 
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();


                try
                {
                    return await GetPersonByNickName(personNickname, dex);

                }
                catch(Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }

        // create a P & Ci,Fn,Rc using Authorixation

        // read just a person

        /// <summary>
        /// <para>
        /// Uses the Index on Person to find the appropriate record. Has many overloads.
        /// </para>
        /// <para>
        /// Using a combination of data from ApplicationUser and/or DexHolder with Person.Nickname to find the unique ( Nickname, DexHolderId ) combination
        /// </para>
        /// </summary>
        /// <remarks> NOTICE: Empty parameters is only for documentation purposes!!! </remarks>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        public void GetPersonByNickName() { throw new NotImplementedException(); }
        /// <summary>
        /// A Helper Method that supplements <see cref="GetPersonByNickName()"/>
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <inheritdoc cref="GetPersonByNickName()" />
        public void GetPersonByNickNameTool() { throw new NotImplementedException(); }


        /// <remarks>
        /// Uses an ApplicationUser object to access ApplicationUser.DexHolder directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
        public async Task<Person?> GetPersonByNickName(string nickName, ApplicationUser user) 
        {
            return await GetPersonByNickNameWithUser(nickName, user); 
        }
        /// <remarks>
        /// Uses a DexHolder object to access DexHolderId directly to search for a person with the DexHolderId
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />

        public async Task<Person?> GetPersonByNickName(string nickName, DexHolder dex)
        {
            return await GetPersonByNickNameWithDex(nickName, dex);
        }
        /// <remarks>
        /// Uses a
        /// <see cref="PropertyField"/>
        /// [ <see cref="PropertyField.id"/> | <see cref="PropertyField.username"/> ]
        /// to search for a <see cref="DexHolder"/> with which it combines
        /// with the nickName to find a <see cref="Person"/>
        /// 
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickName()" />
        public async Task<Person?> GetPersonByNickName(PropertyField pType, string inputProperty, string nickName)
        {
            var person = new Person();
            switch (pType)
            {
                case PropertyField.id:
                    person = await GetPersonByNickNameWithUserIdAsync(inputProperty, nickName);
                    break;
                case PropertyField.username:
                    person = await GetPersonByNickNameWithUserNameAsync(inputProperty, nickName);
                    break;
                //case PropertyField.email:
                //    break;
                default:
                    throw new ArgumentException();
                    break;
            }
            return person;
        }


        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, ApplicationUser)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUser(string nickName, ApplicationUser user)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == user.DexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks> Used by: <code><seealso cref="GetPersonByNickName(string, DexHolder)"/></code> </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithDex(string nickName, DexHolder dexHolder)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == dexHolder.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.username" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUserNameAsync(string userName, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserNameAsync(userName);

            var pp = await _db.Person
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <remarks>
        /// Used by: <code><seealso cref="GetPersonByNickName(PropertyField, string, string)"/></code>
        /// with <see cref="PropertyField.id" />
        /// </remarks>
        /// <inheritdoc cref="GetPersonByNickNameTool()" />
        public async Task<Person?> GetPersonByNickNameWithUserIdAsync(string userId, string nickName)
        {
            var dex = await _userRepo.GetDexHolderByUserIdAsync(userId);

            var pp = await _db.Person
                .Where(p => p.DexHolderId == dex.Id)
                .Where(p => p.Nickname == nickName).FirstOrDefaultAsync();
            return pp;
        }

        /// <summary>
        /// Uses the Index to find a Person. Looks for the record with a unique (NickName, DexId) combination
        /// </summary>
        /// <param name="dexHolderId"></param>
        /// <param name="personNickname"></param>
        /// <returns></returns>
        public async Task<Person?> ReadPersonByNickNameAsync(int dexHolderId, string personNickname)
        {
            var pp = await _db.Person
                .Where(p => p.DexHolderId == dexHolderId)
                .Where(p => p.Nickname == personNickname).FirstOrDefaultAsync();
            return pp;

        }

        /// <summary>
        /// Find a Person using the primary key of Persons
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<Person?> ReadPersonByIdAsync(int personId)
        {
            var person = await _db.Person.FirstOrDefaultAsync(p => p.Id == personId);
            return person;
        }
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


        // read all persons

        // read all persons by username

        // read all persons by userid


        // update/edit


        // delete a person


        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
