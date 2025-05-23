﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using SQLitePCL;

using toolExtensions;

// using NetDexTest_01.Models.DTOs;
namespace NetDexTest_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEntryItemRepository _repo;
        private readonly IPersonRepository _personRepo;
        private readonly IToolService _tools;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<EntryController> _logger;
        public EntryController(ApplicationDbContext context,
            IPersonRepository personRepo,
            IUserRepository userRepo,
            IEntryItemRepository entryRepo,
            IToolService toolService,
            ILogger<EntryController> logger
            )
        {
            _userRepo = userRepo;
            _personRepo = personRepo;
            _context = context;
            _logger = logger;
            _repo = entryRepo;
            _tools = toolService;

        }

        //HACK
        // NOTE - These will need to use viewmodels or DTOs in the future, esp when connecting tables to Application User

        [Route("one/{id}")]
        [HttpGet]
        public async Task<ActionResult<EntryItem>> Get(Int64 id)
        {
            var item = await _repo.GetEntryItemAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpGet]
        public async Task<ActionResult<List<EntryItem>>> GetAll()
        {
            return Ok(await _repo.GetAllEntryItemsAsync());
        }



        [Route("user/{input}")]
        [HttpGet]
        public async Task<ActionResult<List<EntryItem>>> GetAllByUser(string input)
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            return Ok(await _repo.GetAllEntryItemsByUserAsync(input));
        }





        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EntryItemVM item)
        {
            if (await _repo.CreateEntryItemWithVMAsync(item))
            {
                var result = await _personRepo.ReadRecordByIdAsync(item.RecordCollectorId);

                var returner = result.EntryItems.FirstOrDefault(e => e.ShortTitle == item.ShortTitle && e.FlavorText == item.FlavorText && e.RecordCollectorId == item.RecordCollectorId);

                if (returner == null) return NotFound();
                return Ok(returner);
                //    new
                //{

                    // = inPerson.Nickname,
                    //DateOfBirth = inPerson.DateOfBirth,
                    //Gender = inPerson.Gender,
                    //Pronouns = inPerson.Pronouns,
                    //Rating = inPerson.Rating,
                    //Favorite = inPerson.Favorite,
                    //Email = person.Email);
                //});


            }


                //return Ok("EntryItem created.");
            return BadRequest("Invalid Person or EntryItem.");
        }

        //[Route("create")]
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] EntryItem item)
        //{
        //    if (await _repo.CreateEntryItemAsync(item))
        //        return Ok("EntryItem created.");
        //    return BadRequest("Invalid Person or EntryItem.");
        //}

        [Route("update/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(Int64 id, [FromForm] EntryItemVM item)
        {
            if (item.Id != id)
                return BadRequest($"Query id and EntryItemVM Must be equal!. id, {id}, did not match item.Id, {item.Id}!!");
            if (await _repo.UpdateEntryItemAsync(item)) // NOTE : Skinnerboxes and the illusion of choice
                return Ok("EntryItem updated.");
            return BadRequest("Invalid Entry or Person.");
        }



        [Route("put")]
        [HttpPut]
        public async Task<IActionResult> PutUpdate([FromForm] EntryItemVM item)
        {
            if (await _repo.UpdateEntryItemAsync(item))
            {
                var entry = await _context.EntryItem.FindAsync(item.Id);
                
                return Ok(entry);
            }
            return BadRequest("Invalid Entry or Person.");
        }


        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                if (await _repo.DeleteEntryItemAsync(id))
                    return Ok("EntryItem deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NotFound("EntryItem not found or invalid person.");
        }





        #region Transfer Routes

        // TODO - add IsNullOrEmpty check on ShortTitle. If yes, populate with $"[PREVIEW] {}"

        [HttpGet("transfer/one/{id}")]
        public async Task<ActionResult<EntryItemDTO>> TransferOne(Int64 id)
        {
            var entry = await _repo.GetEntryItemAsync(id);
            if (entry == null) return NotFound();
            string? preview = null;
            string tag = "[PREVIEW]: ";
            if (entry.ShortTitle?.Trim().IsNullOrEmpty() ?? true)
            {
               preview = $"{tag}{entry.FlavorText.Truncate(120)}";
            }
            else
            {
                if (entry.ShortTitle.Trim().Length > 123) { preview = entry.ShortTitle.Trim(); }
                else { preview = entry.ShortTitle.Truncate(120); }

                preview = entry.ShortTitle.Truncate(120 - tag.Length);
            }

            var dto = new EntryItemDTO
            {
                EntryItemId = entry.Id,
                RecordCollectorId = entry.RecordCollector.Id,
                PersonId = entry.RecordCollector.Person.Id,
                DexHolderId = entry.RecordCollector.Person.DexHolder.Id,
                PersonNickname = entry.RecordCollector.Person.Nickname,
                ApplicationUserEmail = entry.RecordCollector.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = entry.RecordCollector.Person.DexHolder.ApplicationUser.UserName,
                ShortTitle = preview,
                FlavorText = entry.FlavorText,
                LogTimestamp = entry.LogTimestamp
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
        public async Task<ActionResult<List<EntryItemDTO>>> TransferAll()
        {
            var entries = await _repo.GetAllEntryItemsAsync();

            var result = entries.Select(entry => new EntryItemDTO
            {
                EntryItemId = entry.Id,
                RecordCollectorId = entry.RecordCollector.Id,
                PersonId = entry.RecordCollector.Person.Id,
                DexHolderId = entry.RecordCollector.Person.DexHolder.Id,
                PersonNickname = entry.RecordCollector.Person.Nickname,
                ApplicationUserEmail = entry.RecordCollector.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = entry.RecordCollector.Person.DexHolder.ApplicationUser.UserName,
                //ShortTitle = preview,
                FlavorText = entry.FlavorText,
                LogTimestamp = entry.LogTimestamp
            }).OrderBy(e => e.EntryItemId).ToList();
            List<EntryItemDTO> outList = new();

            foreach(EntryItemDTO e in result){
                string? preview = null;
                string tag = "[PREVIEW]: ";
                if (e.ShortTitle?.Trim().IsNullOrEmpty() ?? true)
                {
                   preview = $"{tag}{e.FlavorText.Truncate(120 - tag.Length)}";
                }
                else
                {
                    if(e.ShortTitle.Trim().Length > 123){ preview = e.ShortTitle.Trim(); }
                   else { preview = e.ShortTitle.Truncate(120); }
                }
                e.ShortTitle = preview;
                outList.Add(e);
            }
            outList = outList.OrderBy(e => e.EntryItemId).ToList();

                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<EntryItemDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(e => e.EntryItemId);
            outList = outListCount;

            
            return Ok(outList);
        }

        [HttpGet("transfer/user/{input}")]
        public async Task<ActionResult<List<EntryItemDTO>>> TransferByUser(string input)
        {
            if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            var entries = await _repo.GetAllEntryItemsByUserAsync(input);
            if (entries == null) return NotFound();

            var result = entries.Select(entry => new EntryItemDTO
            {
                EntryItemId = entry.Id,
                RecordCollectorId = entry.RecordCollector.Id,
                PersonId = entry.RecordCollector.Person.Id,
                DexHolderId = entry.RecordCollector.Person.DexHolder.Id,
                PersonNickname = entry.RecordCollector.Person.Nickname,
                ApplicationUserEmail = entry.RecordCollector.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = entry.RecordCollector.Person.DexHolder.ApplicationUser.UserName,
                //ShortTitle = preview,
                FlavorText = entry.FlavorText,
                LogTimestamp = entry.LogTimestamp
            }).OrderBy(e => e.EntryItemId).ToList();
            List<EntryItemDTO> outList = new();

            foreach (EntryItemDTO e in result)
            {
                string? preview = null;
                string tag = "[PREVIEW]: ";
                if (e.ShortTitle?.Trim().IsNullOrEmpty() ?? true)
                {
                    preview = $"{tag}{e.FlavorText.Truncate(120 - tag.Length)}";
                }
                else
                {
                    if (e.ShortTitle.Trim().Length > 123) { preview = e.ShortTitle.Trim(); }
                    else { preview = e.ShortTitle.Truncate(120); }
                }
                e.ShortTitle = preview;
                outList.Add(e);
            }
            outList = outList.OrderBy(e => e.EntryItemId).ToList();

                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<EntryItemDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(e => e.EntryItemId);
            outList = outListCount;

            
            return Ok(outList);
        }


        [HttpGet("transfer/person/{input}/{criteria}")]
        public async Task<ActionResult<List<EntryItemDTO>>> TransferByUserPerson(string input, string criteria)
        {
            //if (await _userRepo.GetDexHolderMiddleVMAsync(input) == null) return NotFound($"User could not be found using input, {input}");

            var entries = await _repo.GetAllEntryItemsByUserAsync(input);
            if (entries == null) return NotFound($"Entries could not be found for user, {input}");

            // HACK - modified method [[GetOneByUserInputAsync]] in PersonRepository to accept a parsed int from criteria, in case Person.Id gets used as a parameter
            var person = await _personRepo.GetOneByUserInputAsync(input, criteria);
            //await _personRepo.GetPersonByNickName()
            if (person == null) return NotFound($"Person could not be found for user, {input}, person, {criteria}");

            var result = entries.Where(e => e.RecordCollector.Person == person)
                .Select(entry => new EntryItemDTO
            {
                EntryItemId = entry.Id,
                RecordCollectorId = entry.RecordCollector.Id,
                PersonId = entry.RecordCollector.Person.Id,
                DexHolderId = entry.RecordCollector.Person.DexHolder.Id,
                PersonNickname = entry.RecordCollector.Person.Nickname,
                ApplicationUserEmail = entry.RecordCollector.Person.DexHolder.ApplicationUser.Email,
                ApplicationUserName = entry.RecordCollector.Person.DexHolder.ApplicationUser.UserName,
                //ShortTitle = preview,
                FlavorText = entry.FlavorText,
                LogTimestamp = entry.LogTimestamp
            }).OrderBy(e => e.EntryItemId).ToList();
            List<EntryItemDTO> outList = new();

            foreach (EntryItemDTO e in result)
            {
                string? preview = null;
                string tag = "[PREVIEW]: ";
                if (e.ShortTitle?.Trim().IsNullOrEmpty() ?? true)
                {
                    preview = $"{tag}{e.FlavorText.Truncate(120 - tag.Length)}";
                }
                else
                {
                    if (e.ShortTitle.Trim().Length > 123) { preview = e.ShortTitle.Trim(); }
                    else { preview = e.ShortTitle.Truncate(120); }
                }
                e.ShortTitle = preview;
                outList.Add(e);
            }
            outList = outList.OrderBy(e => e.EntryItemId).ToList();

                        // READONE:
            // var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
            // dto.LocalCounter = person?.LocalCounter ?? -4040;
            // READ MANY:
            List<EntryItemDTO> outListCount = new();
            await outList.ForEachAsync(async i =>
            {
                var person = await _userRepo.GetPersonPlusDexListVMAsync(i.ApplicationUserName, i.PersonNickname);
                i.LocalCounter = person?.LocalCounter ?? -4040;
                outListCount.Add(i);
            });
            outListCount.OrderBy(e => e.EntryItemId);
            outList = outListCount;

            
            return Ok(outList);
        }



        #endregion Transfer Routes







        #region ScaffoldResults
        /*

        // GET: api/Entry
        [HttpGet]
        public async Task<ActionResult<ICollection<EntryItem>>> GetEntryItem()
        {
            return 
                await //_entryRepo.ReadAllEntriesAsync();//_context.EntryItem.ToListAsync();
                _context.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                    .ToListAsync();

        }

        // GET: api/Entry
        [HttpGet("/entries/{id}")]
        public async Task<ActionResult<IEnumerable<EntryItem>>> GetEntryItemVMById(string id)
        {
            //var entries = await _context.EntryItem.ToListAsync();

            //IEnumerable<EntryItemVM> enumEntry = new List<EntryItemVM>();
            //var entries = await entries.ToList().ForEach(e => enumEntry.Append(e)); ;
            if (int.TryParse(id, out int idout))
            {
                await Console.Out.WriteLineAsync($"\nReadAllEntries:\n\t Criteria, {id}, is an int. Checking PersonId!!!\n");
                return await _db.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                        .Where(e => e.RecordCollector.Person.Id == idout)
                    .ToListAsync();
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\nReadAllEntries:\n\t Criteria, {id}, is not an int. Checking nickname!!!\n");
                return await _db.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                        .Where(e => e.RecordCollector.Person.Nickname == id)
                    .ToListAsync();
            }

            //return
        }

        // GET: api/Entry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntryItem>> GetEntryItem(long id)
        {
            var entryItem = await _context.EntryItem.FindAsync(id);

            if (entryItem == null)
            {
                return NotFound();
            }

            return entryItem;
        }

        // PUT: api/Entry/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntryItem(long id, EntryItem entryItem)
        {
            if (id != entryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(entryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Entry
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EntryItem>> PostEntryItem(EntryItem entryItem)
        {
            _context.EntryItem.Add(entryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntryItem", new { id = entryItem.Id }, entryItem);
        }

        // DELETE: api/Entry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntryItem(long id)
        {
            var entryItem = await _context.EntryItem.FindAsync(id);
            if (entryItem == null)
            {
                return NotFound();
            }

            _context.EntryItem.Remove(entryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntryItemExists(long id)
        {
            return _context.EntryItem.Any(e => e.Id == id);
        }
        */
        #endregion ScaffoldResults

    } 
}
