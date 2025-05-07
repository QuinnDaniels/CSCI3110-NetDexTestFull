using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Azure;
using toolExtensions;


namespace NetDexTest_01.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocialMediaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IPersonRepository _personRepo;
        private readonly IUserRepository _userRepo;
        private readonly IToolService _tools;

        public SocialMediaController(ApplicationDbContext context,
            IUserRepository userRepo,
            IPersonRepository personRepo,
            IToolService tools)
        {
            _context = context;
            _personRepo = personRepo;
            _userRepo = userRepo;

            _tools = tools;
        }

        #region Read Methods

        [HttpGet("transfer/one/{id}")]
        public async Task<ActionResult<SocialMediaDTO>> GetOneDTO(int id)
        {
            var social = await _context.SocialMedia
                .Include(s => s.ContactInfo)
                    .ThenInclude(c => c.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(d => d.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (social == null) return NotFound();

            var dto = new SocialMediaDTO
            {
                SocialMediaId = social.Id,
                ContactInfoId = social.ContactInfo.Id,
                PersonId = social.ContactInfo.Person.Id,
                DexHolderId = social.ContactInfo.Person.DexHolder.Id,
                
                LocalCounter = null,
                
                PersonNickname = social.ContactInfo.Person.Nickname,
                ApplicationUserEmail = social.ContactInfo.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = social.ContactInfo.Person.DexHolder.ApplicationUser.UserName,
                ContactInfoNoteText = social.ContactInfo.NoteText,
                CategoryField = social.CategoryField,
                SocialHandle = social.SocialHandle ?? "---",
                LogTimestamp = social.LogTimestamp
            };

            // READONE:
            var person = await _userRepo.GetPersonPlusDexListVMAsync(dto.ApplicationUserName, dto.PersonNickname);
            dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            // List<SocialMediaDTO> outListCount = new();
            // await outList.ForEachAsync(async i =>
            // {
            //     var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            //     i.LocalCounter = person?.LocalCounter ?? -4040;
            //     outListCount.Add(i);
            // });
            // outListCount.OrderBy(s => s.SocialMediaId);
            // outList = outListCount;


            return Ok(dto);
        }



        [HttpGet("transfer/all")]
        public async Task<ActionResult<List<SocialMediaDTO>>> GetAllDTO()
        {
            var list = await _context.SocialMedia
                .Include(s => s.ContactInfo)
                    .ThenInclude(c => c.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(d => d.ApplicationUser)
                .ToListAsync();

            var outList = list.Select(s => new SocialMediaDTO
            {
                SocialMediaId = s.Id,
                ContactInfoId = s.ContactInfo.Id,
                PersonId = s.ContactInfo.Person.Id,
                DexHolderId = s.ContactInfo.Person.DexHolder.Id,
                
                LocalCounter = null,
                
                PersonNickname = s.ContactInfo.Person.Nickname,
                ApplicationUserEmail = s.ContactInfo.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = s.ContactInfo.Person.DexHolder.ApplicationUser.UserName,
                ContactInfoNoteText = s.ContactInfo.NoteText,
                CategoryField = s.CategoryField,
                SocialHandle = s.SocialHandle ?? "---",
                LogTimestamp = s.LogTimestamp
            }).ToList();

                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<SocialMediaDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(s => s.SocialMediaId);
            outList = outListCount;

            return Ok(outList);
        }


        [HttpGet("transfer/user/{input}")]
        public async Task<ActionResult<List<SocialMediaDTO>>> GetAllByUserDTO(string input)
        {

            int i = -1;
            bool success = int.TryParse(input, out i);
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            var list = await _context.SocialMedia
                .Include(s => s.ContactInfo)
                    .ThenInclude(c => c.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(d => d.ApplicationUser)
                .Where(s => s.ContactInfo.Person.DexHolder
                    .ApplicationUserName == input
                        || s.ContactInfo.Person.DexHolder
                    .ApplicationUser.Email == input
                        || s.ContactInfo.Person.DexHolder
                    .ApplicationUserId == input
                        || ((i > 0) && (s.ContactInfo.Person
                        .DexHolder.Id == i)))
                .ToListAsync();

            if (list == null) return NotFound("list was null");
            List<SocialMediaDTO> outList = new();
            
            await Task.Run(() => {
                outList = list.Select(s => new SocialMediaDTO
                {
                    SocialMediaId = s.Id,
                    ContactInfoId = s.ContactInfo.Id,
                    PersonId = s.ContactInfo.Person.Id,
                    DexHolderId = s.ContactInfo.Person.DexHolder.Id,
                    
                    LocalCounter = null,

                    PersonNickname = s.ContactInfo.Person.Nickname,
                    ApplicationUserEmail = s.ContactInfo.Person.DexHolder.ApplicationUser.Email,
                    ApplicationUserName = s.ContactInfo.Person.DexHolder.ApplicationUser.UserName,
                    ContactInfoNoteText = s.ContactInfo.NoteText,
                    CategoryField = s.CategoryField,
                    SocialHandle = s.SocialHandle ?? "---",
                    LogTimestamp = s.LogTimestamp
                }).OrderBy(s => s.SocialMediaId).ToList();
            });


            


                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<SocialMediaDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(s => s.SocialMediaId);
            outList = outListCount;

            return Ok(outList);
        }
            //NOTE: required reading: https://stackoverflow.com/questions/18667633/how-can-i-use-async-with-foreach
            //var results = await Task.WhenAll(tasks);


        [HttpGet("transfer/person/{input}/{criteria}")]
        public async Task<ActionResult<List<SocialMediaDTO>>> GetAllByUserPersonDTO(string input, string criteria)
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            int i = -1;
            bool success = int.TryParse(input, out i);
            var person = await _personRepo.GetOneByUserInputAsync(input, criteria);
            if (person == null) return NotFound("person not found");


            var list = await _context.SocialMedia
                .Include(s => s.ContactInfo)
                    .ThenInclude(c => c.Person)
                        .ThenInclude(p => p.DexHolder)
                            .ThenInclude(d => d.ApplicationUser)
                .Where(s => s.ContactInfo.Person == person
                        && s.ContactInfo.Person.DexHolder
                    .ApplicationUserName == input
                        || s.ContactInfo.Person.DexHolder
                    .ApplicationUser.Email == input
                        || s.ContactInfo.Person.DexHolder
                    .ApplicationUserId == input
                        || ((i > 0) && (s.ContactInfo.Person
                        .DexHolder.Id == i)))
                .ToListAsync();

            if (list == null) return NotFound();
            List<SocialMediaDTO> outList = new();

            await Task.Run(() => {
                outList = list.Select(s => new SocialMediaDTO
                {
                    SocialMediaId = s.Id,
                    ContactInfoId = s.ContactInfo.Id,
                    PersonId = s.ContactInfo.Person.Id,
                    DexHolderId = s.ContactInfo.Person.DexHolder.Id,
                    
                    LocalCounter = null,
                    
                    PersonNickname = s.ContactInfo.Person.Nickname,
                    ApplicationUserEmail = s.ContactInfo.Person.DexHolder.ApplicationUser.Email,
                    ApplicationUserName = s.ContactInfo.Person.DexHolder.ApplicationUser.UserName,
                    ContactInfoNoteText = s.ContactInfo.NoteText,
                    CategoryField = s.CategoryField,
                    SocialHandle = s.SocialHandle ?? "---",
                    LogTimestamp = s.LogTimestamp
                }).OrderBy(s => s.SocialMediaId).ToList();
            });


            // foreach (EntryItemDTO e in result)
            // {
            //     string? preview = null;
            //     string tag = "[PREVIEW]: ";
            //     if (e.ShortTitle?.Trim().IsNullOrEmpty() ?? true)
            //     {
            //         preview = $"{tag}{e.FlavorText.Truncate(120 - tag.Length)}";
            //     }
            //     else
            //     {
            //         if (e.ShortTitle.Trim().Length > 123) { preview = e.ShortTitle.Trim(); }
            //         else { preview = e.ShortTitle.Truncate(120); }
            //     }
            //     e.ShortTitle = preview;
            //     outList.Add(e);
            // }
            //outList = outList.OrderBy(s => s.EntryItemId).ToList();

                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<SocialMediaDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(s => s.SocialMediaId);
            outList = outListCount;

            return Ok(outList);
        }







        #endregion Read Methods



        #region Create

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SocialMediaVM vm)
        {
            var contact = await _context.ContactInfo.FindAsync(vm.ContactInfoId);
            if (contact == null) return NotFound("ContactInfo not found");

            var social = new SocialMedia
            {
                ContactInfo = contact,
                CategoryField = vm.CategoryField,
                SocialHandle = vm.SocialHandle,
                LogTimestamp = DateTime.UtcNow
            };

            _context.SocialMedia.Add(social);
            await _context.SaveChangesAsync();
            return Ok("SocialMedia created.");
        }

        #endregion Create



        #region Update

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Int64 id, [FromForm] SocialMediaVM vm)
        {
            if (vm.Id != id) return BadRequest("ID mismatch.");
            var social = await _context.SocialMedia.FindAsync(vm.Id);
            if (social == null) return NotFound();

            social.CategoryField = vm.CategoryField;
            social.SocialHandle = vm.SocialHandle;
            await _context.SaveChangesAsync();

            return Ok("Updated.");
        }

        [Route("put")]
        [HttpPut]
        public async Task<IActionResult> PutUpdate([FromForm] SocialMediaVM vm)
        {
            if (await UpdateSocialMediaAsync(vm))
                return Ok("EntryItem updated.");
            return BadRequest("Invalid Entry or Person.");
        }


        #endregion Update

        #region UpdateTools

        public async Task<bool> UpdateSocialMediaAsync(SocialMediaVM item)
        {
            var social = await _context.SocialMedia.FindAsync(item.Id);
            if (social == null) return false;

            ContactInfo? contactExists = await _personRepo.ReadContactInfoByIdAsync(social.ContactInfoId);// != null;
            if (contactExists == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: ContactInfo not found for item.ContactInfoId, {item.ContactInfoId} ");
                return false;
            }
            var pId = contactExists!.PersonId;
            Person? personExists = await _personRepo.ReadPersonByIdAsync(contactExists.PersonId); //!= null;
            if (personExists == null)
            {
                await Console.Out.WriteLineAsync($"\n\nNOTICE: Person not found for ContactInfo, {pId} ");
                return false;
            }

            // HACK - I should really put lastUpdated on the items, then have a calc column on the collector tables or smthg
            social.LogTimestamp = DateTime.UtcNow;

            social.CategoryField = item.CategoryField;
            social.SocialHandle = item.SocialHandle;
            return await _context.SaveChangesAsync() > 0;
        }

        #endregion UpdateTools


        #region Delete

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Int64 id)
        {
            var social = await _context.SocialMedia.FindAsync(id);
            if (social == null) return NotFound();

            _context.SocialMedia.Remove(social);
            await _context.SaveChangesAsync();
            return Ok("Deleted.");
        }

        #endregion Delete



        #region ContactInfo.NoteText

        [Route("contactinfo/note/Get/{input}/{criteria}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetNoteText(string input, string criteria)    //Int64 contactInfoId)
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            var person = await _personRepo.GetOneByUserInputAsync(input, criteria);
            if (person == null) return NotFound();

            var contact = await _context.ContactInfo
                .Where(c => c.Id == person.ContactInfo.Id && c.PersonId == person.Id)
                .FirstOrDefaultAsync();

            return contact == null ? NotFound("ContactInfo not found.") : Ok($"[NOTE]: {contact.NoteText}");
        }


        [Route("contactinfo/note/Get/all/{input}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetAllNoteText(string input)    //Int64 contactInfoId)
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            ICollection<Person>? checkers = await _personRepo.ReadAllPeopleAsync(input);
            if (checkers == null || checkers.ToArray().Length < 1) return BadRequest();

            //var person = await _personRepo.GetOneByUserInputAsync(input, criteria);
            //if (person == null) return NotFound();
            List<string> strings = new();
            string wiper = string.Empty;
            foreach (var item in checkers)
            {
                var contact = await _context.ContactInfo
                    .Include(c => c.Person)
                    .Where(c => c.Id == item.ContactInfo.Id && c.PersonId == item.Id)
                    .FirstOrDefaultAsync();


                if (contact?.NoteText.Trim().IsNullOrEmpty() ?? true)
                {
                    wiper = $"[{contact?.Person.Nickname}]: ___";
                }
                else
                {
                    wiper = $"[{contact?.Person.Nickname}]: {contact?.NoteText}";

                    //if (contact?.NoteText.Trim().Length > 123) { preview = entry.ShortTitle.Trim(); }
                    //else { preview = entry.ShortTitle.Truncate(120); }

                    //preview = entry.ShortTitle.Truncate(120 - tag.Length);
                }

                //wiper += "/n";

                //wiper = $"[{contact.Person.Nickname}]: {contact?.NoteText ?? "---"}";
                strings.Add(wiper);
            }

            string modelJson = JsonSerializer.Serialize(strings);
            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);

            //var contact = await _context.ContactInfo
            //    .Where(c => c.Id == person.ContactInfo.Id && c.PersonId == person.Id)
            //    .FirstOrDefaultAsync();

            // return contact == null ? NotFound("ContactInfo not found.") : Ok($"[NOTE]: {contact.NoteText}");
        }





        [Route("contactinfo/note/Put/{input}/{criteria}")]
        [HttpPut]
        public async Task<IActionResult> UpdateNoteText(string input, string criteria, [FromForm] string newText)   // Int64 contactInfoId, )
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            var person = await _personRepo.GetOneByUserInputAsync(input, criteria);
            if (person == null) return NotFound();

            var contact = await _context.ContactInfo
                .Where(c => c.Id == person.ContactInfo.Id && c.PersonId == person.Id)
                .FirstOrDefaultAsync();

            if (contact == null) return NotFound("ContactInfo not found.");

            contact.NoteText = newText;
            await _context.SaveChangesAsync();

            return Ok("NoteText updated.");
        }

        #endregion ContactInfo.NoteText



        // if input parse -> dexId
        //  // ReadPersonByNickNameAsync(int dexHolderId, string personNickname)
        // endif
        //
        // GetPersonByNickNameWithUserIdAsync
        // GetPersonByNickNameWithUserNameAsync
        // GetPersonByNickNameWithEmailAsync


    }
}