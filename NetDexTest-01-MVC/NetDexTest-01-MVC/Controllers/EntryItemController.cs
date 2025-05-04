using Microsoft.AspNetCore.Mvc;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;
using NetDexTest_01_MVC.Services;
using Microsoft.Docs.Samples;
using toolExtensions;


namespace NetDexTest_01_MVC.Controllers
{
    [Route("entryitem")]
    public class EntryItemController : Controller
    {
        private readonly IEntryItemService _entryItemService;

        public EntryItemController(IEntryItemService entryItemService)
        {
            _entryItemService = entryItemService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var items = await _entryItemService.GetAllAsync();
            return View(items);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var item = await _entryItemService.GetByIdAsync(id);
            return View(item);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(EntryItemVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _entryItemService.CreateAsync(model);
            if (!success) return BadRequest("Failed to create entry.");
            return RedirectToAction("List");
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(long id)
        {
            var item = await _entryItemService.GetByIdAsync(id);
            return View(item);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EntryItemVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _entryItemService.UpdateAsync(model);
            if (!success) return BadRequest("Failed to update entry.");
            return RedirectToAction("List");
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _entryItemService.DeleteAsync(id);
            if (!success) return BadRequest("Failed to delete entry.");
            return RedirectToAction("List");
        }
    }
}
