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
    public partial interface IEntryItemRepository
    {
        // Task<ICollection<EntryItem>> ReadAllEntriesAsync();
        // Task<ICollection<EntryItem>> ReadAllEntriesAsync(string criteria);
        // Task<ICollection<EntryItem>?> ReadAllEntriesByUserAsync(string criteria);
        Task<EntryItem?> GetEntryItemAsync(Int64 id);
        Task<List<EntryItem>?> GetAllEntryItemsByUserAsync(string input);
        Task<List<EntryItem>> GetAllEntryItemsAsync();
        Task<bool> CreateEntryItemAsync(EntryItem item);
        Task<bool> UpdateEntryItemAsync(Int64 id, string newTitle, string newText);
        Task<bool> UpdateEntryItemAsync(Int64 id, EntryItemVM item);
        Task<bool> UpdateEntryItemAsync(EntryItemVM item);

        Task<bool> DeleteEntryItemAsync(Int64 id);
        Task<bool> CreateEntryItemWithVMAsync(EntryItemVM item);
    }



}


    public partial class DbEntryItemRepository : IEntryItemRepository
    {
        private readonly ApplicationDbContext _context; //_context
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
        public DbEntryItemRepository(
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
            _context = db; //context
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
    /*---------------------------------*/


    public async Task<EntryItem?> GetEntryItemAsync(Int64 id)
    {
        return await _context.EntryItem
            .Include(e => e.RecordCollector)
                .ThenInclude(rc => rc.Person)
                    .ThenInclude(p => p.FullName)
            .Include(e => e.RecordCollector)
                .ThenInclude(rc => rc.Person)
                    .ThenInclude(p => p.DexHolder)
                        .ThenInclude(dh => dh.ApplicationUser)
            .FirstOrDefaultAsync(e => e.Id == id);
    }


    public async Task<List<EntryItem>?> GetAllEntryItemsByUserAsync(string input)
    {
        List<EntryItem>? tasker = null;

        if (Int64.TryParse(input, out Int64 idout))
        {
            await Console.Out.WriteLineAsync($"\n\nGetEntriesByUser:\n\t Criteria, {input}, is an Int64. Checking DexHolderID!!!\n");
            tasker = await _context.EntryItem
                .Include(e => e.RecordCollector)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.FullName)
                .Include(e => e.RecordCollector)
                    .ThenInclude(rc => rc.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(dh => dh.ApplicationUser)
                .Where(e => e.RecordCollector.Person.DexHolder.Id == idout)
                .ToListAsync();
        }
        else
        {
            await Console.Out.WriteLineAsync($"\n\nGetEntriesByUser:\n\t Criteria, {input}, is NOT an Int64. Checking DexHolderID!!!\n");
            tasker = await _context.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(rc => rc.Person)
                            .ThenInclude(p => p.FullName)
                    .Include(e => e.RecordCollector)
                        .ThenInclude(rc => rc.Person)
                            .ThenInclude(p => p.DexHolder)
                                .ThenInclude(dh => dh.ApplicationUser)
                    .Where(e => e.RecordCollector.Person.DexHolder.ApplicationUser.UserName == input
                    || e.RecordCollector.Person.DexHolder.ApplicationUser.Id == input
                    || e.RecordCollector.Person.DexHolder.ApplicationUser.Email == input
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


    public async Task<List<EntryItem>> GetAllEntryItemsAsync()
    {
        return await _context.EntryItem
            .Include(e => e.RecordCollector)
                .ThenInclude(rc => rc.Person)
                    .ThenInclude(p => p.FullName)
            .Include(e => e.RecordCollector)
                .ThenInclude(rc => rc.Person)
                    .ThenInclude(p => p.DexHolder)
                        .ThenInclude(dh => dh.ApplicationUser)
            .ToListAsync();
    }

    public async Task<bool> CreateEntryItemAsync(EntryItem item)
    {
        RecordCollector? recordsExist = await _personRepo.ReadRecordByIdAsync(item.RecordCollectorId);// != null;
        if (recordsExist != null) {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: RecordCollector not found for item.RecordCollectorId, {item.RecordCollectorId} ");
        return false;
        }
        var pId = recordsExist!.PersonId;
        Person? personExists = await _personRepo.ReadPersonByIdAsync(recordsExist.PersonId); //!= null;
        if (personExists != null) {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for RecordCollector, {pId} ");
        return false;
        }

        item.RecordCollector = recordsExist;
        item.RecordCollectorId = recordsExist.Id;
        item.RecordCollector.LastUpdated = DateTime.UtcNow;

        _context.EntryItem.Add(item);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> CreateEntryItemWithVMAsync(EntryItemVM item)
    {
        RecordCollector? recordsExist = await _personRepo.ReadRecordByIdAsync(item.RecordCollectorId);// != null;
        if (recordsExist == null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: RecordCollector not found for item.RecordCollectorId, {item.RecordCollectorId}:");
            return false;
        }
        var pId = recordsExist!.PersonId;
        Person? personExists = await _personRepo.ReadPersonByIdAsync(recordsExist.PersonId); //!= null;
        if (personExists == null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for RecordCollector, {pId} ");
            return false;
        }

        EntryItem toAdd = new EntryItem(){
            RecordCollectorId = recordsExist.Id,
            ShortTitle = item.ShortTitle,
            FlavorText = item.FlavorText,
            RecordCollector = recordsExist
        };
        _context.EntryItem.Add(toAdd);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateEntryItemAsync(Int64 id, string newTitle, string newText)
    {
        var entry = await _context.EntryItem.FindAsync(id);
        if (entry == null) return false;

        var personExists = await _personRepo.ReadPersonByIdAsync(entry.RecordCollector.PersonId) != null;
        if (!personExists) return false;

        entry.ShortTitle = newTitle;
        entry.FlavorText = newText;
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> UpdateEntryItemAsync(Int64 id, EntryItemVM item)
    {
        var entry = await _context.EntryItem.FindAsync(id);
        if (entry == null) return false;

        RecordCollector? recordsExist = await _personRepo.ReadRecordByIdAsync(item.RecordCollectorId);// != null;
        if (recordsExist != null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: RecordCollector not found for item.RecordCollectorId, {item.RecordCollectorId} ");
            return false;
        }
        var pId = recordsExist!.PersonId;
        Person? personExists = await _personRepo.ReadPersonByIdAsync(recordsExist.PersonId); //!= null;
        if (personExists != null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for RecordCollector, {pId} ");
            return false;
        }


        // HACK - I should really put lastUpdated on the items, then have a calc column on the collector tables or smthg
        entry.LogTimestamp = DateTime.UtcNow;

        entry.ShortTitle = item.ShortTitle;
        entry.FlavorText = item.FlavorText;
        return await _context.SaveChangesAsync() > 0;
    }



    public async Task<bool> UpdateEntryItemAsync(EntryItemVM item)
    {
        var entry = await _context.EntryItem.FindAsync(item.Id);
        if (entry == null) return false;

        RecordCollector? recordsExist = await _personRepo.ReadRecordByIdAsync(entry.RecordCollectorId);// != null;
        if (recordsExist == null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: RecordCollector not found for item.RecordCollectorId, {item.RecordCollectorId} ");
            return false;
        }
        var pId = recordsExist!.PersonId;
        Person? personExists = await _personRepo.ReadPersonByIdAsync(recordsExist.PersonId); //!= null;
        if (personExists == null)
        {
            await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for RecordCollector, {pId} ");
            return false;
        }


        // HACK - I should really put lastUpdated on the items, then have a calc column on the collector tables or smthg
        entry.LogTimestamp = DateTime.UtcNow;

        entry.ShortTitle = item.ShortTitle ;
        entry.FlavorText = item.FlavorText;
        return await _context.SaveChangesAsync() > 0;
    }




    public async Task<bool> DeleteEntryItemAsync(Int64 id)
    {
        var entry = await _context.EntryItem.FindAsync(id);
        if (entry == null) return false;

        var personExists = await _personRepo.ReadPersonByIdAsync(entry.RecordCollector.PersonId) != null;
        if (!personExists) return false;

        _context.EntryItem.Remove(entry);
        return await _context.SaveChangesAsync() > 0;
    }










    /*--------------------------------------------*/
    //*/
    #region BadCode
    /*

            public async Task<ICollection<EntryItem>> ReadAllEntriesAsync()
            {
                return await _context.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            //.ThenInclude(p => p.FullName)
                    .ToListAsync();
            }


            public async Task<ICollection<EntryItem>> ReadAllEntriesAsync(string criteria)
            {
                if (Int64.TryParse(criteria, out int idout))
                {
                    await Console.Out.WriteLineAsync($"\nReadAllEntries:\n\t Criteria, {criteria}, is an int. Checking PersonId!!!\n");
                    return await _context.EntryItem
                        .Include(e => e.RecordCollector)
                            .ThenInclude(r => r.Person)
                            //// HACK - Discluded due to error (see end of file)
                                //.ThenInclude(p => p.FullName)
                            .Where(e => e.RecordCollector.Person.Id == idout)
                        .ToListAsync();
                }
                else
                {
                    await Console.Out.WriteLineAsync($"\n\nReadAllEntries:\n\t Criteria, {criteria}, is not an int. Checking nickname!!!\n");
                    return await _context.EntryItem
                        .Include(e => e.RecordCollector)
                            .ThenInclude(r => r.Person)
                                //.ThenInclude(p => p.FullName)
                            .Where(e => e.RecordCollector.Person.Nickname == criteria)
                        .ToListAsync();
                }

            }

            public async Task<ICollection<EntryItem>?> ReadAllEntriesByUserAsync(string input)
            {
                    await Console.Out.WriteLineAsync($"\n\nReadAllEntriesByUser:\n\t Criteria, {input}, is an int. Checking DexHolderID!!!\n");
                    var tasker = await _context.EntryItem
                            // .Include(e => e.RecordCollector)
                            //     .ThenInclude(r => r.Person)
                            //         .ThenInclude(p => p.FullName)
                            .Include(e => e.RecordCollector)
                                .ThenInclude(r => r.Person)
                                    .ThenInclude(p => p.DexHolder)
                                        .ThenInclude(p => p.ApplicationUser)
                            .Where(e => e.RecordCollector.Person.DexHolder.ApplicationUser.UserName == input
                                    ||e.RecordCollector.Person.DexHolder.ApplicationUser.Id == input
                                    ||e.RecordCollector.Person.DexHolder.ApplicationUser.Email == input
                                        )
                            .ToListAsync();
                    if (tasker != null)
                    {
                        return tasker;
                    }
                    else
                    {
                        return null;
                    }
                    //await Console.Out.WriteLineAsync($"\n\nReadAllEntriesByUser:\n\t Criteria, {criteria}!!!\n");
                    //await Console.Out.WriteAsync($"\t\t ApplicationUserName, {criteria}!!!\n");
                    //await Console.Out.WriteAsync($"\t\t ApplicationUserId, {criteria}!!!\n");
                    //await Console.Out.WriteAsync($"\t\t Email, {criteria}!!!\n");
                }

            }
    */
    #endregion


}




/* HACK 1 - Exception when including full name



System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles. Path: $.RecordCollector.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.FullName.Person.Id.
   at System.Text.Json.ThrowHelper.ThrowJsonException_SerializerCycleDetected(Int32 maxDepth)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonPropertyInfo`1.GetMemberAndWriteJson(Object obj, WriteStack& state, Utf8JsonWriter writer)
   at System.Text.Json.Serialization.Converters.ObjectDefaultConverter`1.OnTryWrite(Utf8JsonWriter writer, T value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Converters.ListOfTConverter`2.OnWriteResume(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonCollectionConverter`2.OnTryWrite(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.TryWrite(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.JsonConverter`1.WriteCore(Utf8JsonWriter writer, T& value, JsonSerializerOptions options, WriteStack& state)
   at System.Text.Json.Serialization.Metadata.JsonTypeInfo`1.SerializeAsync(Stream utf8Json, T rootValue, CancellationToken cancellationToken, Object rootValueBoxed)
   at System.Text.Json.Serialization.Metadata.JsonTypeInfo`1.SerializeAsync(Stream utf8Json, T rootValue, CancellationToken cancellationToken, Object rootValueBoxed)
   at System.Text.Json.Serialization.Metadata.JsonTypeInfo`1.SerializeAsync(Stream utf8Json, T rootValue, CancellationToken cancellationToken, Object rootValueBoxed)
   at Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter.WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResultFilterAsync>g__Awaited|30_0[TFilter,TFilterAsync](ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResultExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.ResultNext[TFilter,TFilterAsync](State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.InvokeResultFilters()
--- End of stack trace from previous location ---
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|25_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)


**/