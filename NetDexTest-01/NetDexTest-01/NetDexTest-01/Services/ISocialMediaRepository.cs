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


    #region Interface
    public partial interface ISocialMediaRepository
    {
        Task<SocialMedia?> GetSocialMediaAsync(Int64 id);
        Task<List<SocialMedia>?> GetAllSocialMediasByUserAsync(string input);
        Task<List<SocialMedia>> GetAllSocialMediasAsync();
        Task<bool> CreateSocialMediaAsync(SocialMedia item);
        Task<bool> UpdateSocialMediaAsync(Int64 id, string newTitle, string newText);
        Task<bool> UpdateSocialMediaAsync(Int64 id, SocialMediaVM item);
        Task<bool> UpdateSocialMediaAsync(SocialMediaVM item);

        Task<bool> DeleteSocialMediaAsync(Int64 id);
        Task<bool> CreateSocialMediaWithVMAsync(SocialMediaVM item);
    }
    #endregion


    public partial class DbSocialMediaRepository : ISocialMediaRepository
    {
        private readonly ApplicationDbContext _db; //_db
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




        public async Task<SocialMedia?> GetSocialMediaAsync(Int64 id)
        {
            return await _db.SocialMedia
                .Include(e => e.ContactInfo)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.FullName)
                .Include(e => e.ContactInfo)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(dh => dh.ApplicationUser)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<SocialMedia>?> GetAllSocialMediasByUserAsync(string input)
        {
            List<SocialMedia>? tasker = null;

            if (Int64.TryParse(input, out Int64 idout))
            {
                await Console.Out.WriteLineAsync($"\n\nGetEntriesByUser:\n\t Criteria, {input}, is an Int64. Checking DexHolderID!!!\n");
                tasker = await _db.SocialMedia
                    .Include(e => e.ContactInfo)
                        .ThenInclude(rc => rc.Person)
                            .ThenInclude(p => p.FullName)
                    .Include(e => e.ContactInfo)
                        .ThenInclude(rc => rc.Person)
                            .ThenInclude(p => p.DexHolder)
                                .ThenInclude(dh => dh.ApplicationUser)
                    .Where(e => e.ContactInfo.Person.DexHolder.Id == idout)
                    .ToListAsync();
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\nGetEntriesByUser:\n\t Criteria, {input}, is NOT an Int64. Checking DexHolderID!!!\n");
                tasker = await _db.SocialMedia
                        .Include(e => e.ContactInfo)
                            .ThenInclude(rc => rc.Person)
                                .ThenInclude(p => p.FullName)
                        .Include(e => e.ContactInfo)
                            .ThenInclude(rc => rc.Person)
                                .ThenInclude(p => p.DexHolder)
                                    .ThenInclude(dh => dh.ApplicationUser)
                        .Where(e => e.ContactInfo.Person.DexHolder.ApplicationUser.UserName == input
                        || e.ContactInfo.Person.DexHolder.ApplicationUser.Id == input
                        || e.ContactInfo.Person.DexHolder.ApplicationUser.Email == input
                            )
                        .ToListAsync();
            }
            if (tasker != null)
            {
                return tasker;
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\nGetEntriesByUser:\n\t Criteria, {input}, NOT FOUND!!! Returning null...\n");
                return null;
            }
        }

        public async Task<List<SocialMedia>> GetAllSocialMediasAsync()
        {
            return await _db.SocialMedia
                .Include(e => e.ContactInfo)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.FullName)
                .Include(e => e.ContactInfo)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(dh => dh.ApplicationUser)
                .ToListAsync();
        }

        public async Task<bool> CreateSocialMediaAsync(SocialMedia item)
        {
            ContactInfo? contactsExist = await _personRepo.ReadContactInfoByIdAsync(item.ContactInfoId);// != null;
            if (contactsExist != null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: ContactInfo not found for item.ContactInfoId, {item.ContactInfoId} ");
                return false;
            }
            var pId = contactsExist!.PersonId;
            Person? personExists = await _personRepo.ReadPersonByIdAsync(contactsExist.PersonId); //!= null;
            if (personExists != null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for ContactInfo, {pId} ");
                return false;
            }

            item.ContactInfo = contactsExist;
            item.ContactInfoId = contactsExist.Id;
            item.ContactInfo.LastUpdated = DateTime.UtcNow;

            _db.SocialMedia.Add(item);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateSocialMediaWithVMAsync(SocialMediaVM item)
        {
            ContactInfo? contactsExist = await _personRepo.ReadContactInfoByIdAsync(item.ContactInfoId);// != null;
            if (contactsExist == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: ContactInfo not found for item.ContactInfoId, {item.ContactInfoId}:");
                return false;
            }
            var pId = contactsExist!.PersonId;
            Person? personExists = await _personRepo.ReadPersonByIdAsync(contactsExist.PersonId); //!= null;
            if (personExists == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for ContactInfo, {pId} ");
                return false;
            }

            SocialMedia toAdd = new SocialMedia()
            {
                ContactInfoId = contactsExist.Id,
                CategoryField = item.CategoryField,
                SocialHandle = item.SocialHandle,
                ContactInfo = contactsExist
            };
            _db.SocialMedia.Add(toAdd);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSocialMediaAsync(Int64 id, string newTitle, string newText)
        {
            var entry = await _db.SocialMedia.FindAsync(id);
            if (entry == null) return false;

            var personExists = await _personRepo.ReadPersonByIdAsync(entry.ContactInfo.PersonId) != null;
            if (!personExists) return false;

            entry.CategoryField = newTitle;
            entry.SocialHandle = newText;
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateSocialMediaAsync(Int64 id, SocialMediaVM item)
        {
            var entry = await _db.SocialMedia.FindAsync(id);
            if (entry == null) return false;

            ContactInfo? contactsExist = await _personRepo.ReadContactInfoByIdAsync(item.ContactInfoId);// != null;
            if (contactsExist != null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: ContactInfo not found for item.ContactInfoId, {item.ContactInfoId} ");
                return false;
            }
            var pId = contactsExist!.PersonId;
            Person? personExists = await _personRepo.ReadPersonByIdAsync(contactsExist.PersonId); //!= null;
            if (personExists != null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for ContactInfo, {pId} ");
                return false;
            }


            // HACK - I should really put lastUpdated on the items, then have a calc column on the collector tables or smthg
            entry.LogTimestamp = DateTime.UtcNow;

            entry.CategoryField = item.CategoryField;
            entry.SocialHandle = item.SocialHandle;
            return await _db.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateSocialMediaAsync(SocialMediaVM item)
        {
            var entry = await _db.SocialMedia.FindAsync(item.Id);
            if (entry == null) return false;

            ContactInfo? contactsExist = await _personRepo.ReadContactInfoByIdAsync(entry.ContactInfoId);// != null;
            if (contactsExist == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: ContactInfo not found for item.ContactInfoId, {item.ContactInfoId} ");
                return false;
            }
            var pId = contactsExist!.PersonId;
            Person? personExists = await _personRepo.ReadPersonByIdAsync(contactsExist.PersonId); //!= null;
            if (personExists == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for ContactInfo, {pId} ");
                return false;
            }


            // HACK - I should really put lastUpdated on the items, then have a calc column on the collector tables or smthg
            entry.LogTimestamp = DateTime.Now;

            entry.CategoryField = item.CategoryField;
            entry.SocialHandle = item.SocialHandle;
            return await _db.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteSocialMediaAsync(Int64 id)
        {
            var entry = await _db.SocialMedia.FindAsync(id);
            if (entry == null) return false;

            var personExists = await _personRepo.ReadPersonByIdAsync(entry.ContactInfo.PersonId) != null;
            if (!personExists) return false;

            _db.SocialMedia.Remove(entry);
            return await _db.SaveChangesAsync() > 0;
        }











    }
}
