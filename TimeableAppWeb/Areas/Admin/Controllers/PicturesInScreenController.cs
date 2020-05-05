using System;
using System.IO;
using System.Threading.Tasks;
using BLL.App.Helpers;
using BLL.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using TimeableAppWeb.Areas.Admin.Helpers;
using TimeableAppWeb.Areas.Admin.ViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
    public class PicturesInScreenController : Controller
    {
        private readonly string PromotionsDirectory = "Promotions";
        private readonly string BackgroundDirectory = "Background.Pictures";

        private readonly IBLLApp _bll;
        private readonly IHostEnvironment _appEnvironment;
        private readonly UserManager<AppUser> _userManager;

        public PicturesInScreenController(IBLLApp bll, IHostEnvironment appEnvironment, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        // GET: Admin/Pictures/Details/5
        public async Task<IActionResult> Details(int? pictureInScreenId)
        {
            if (pictureInScreenId == null)
            {
                return NotFound();
            }

            var pictureInScreen = await _bll.PictureInScreens.FindAsync(pictureInScreenId);

            if (pictureInScreen == null)
            {
                return NotFound();
            }

            return View(pictureInScreen);
        }

        // GET: Admin/Pictures/Create
        public async Task<IActionResult> Create(int screenId, bool isBackgroundImage)
        {
            var vm = new PictureCreateEditViewModel
            {
                IsBackgroundPicture = isBackgroundImage,
                ScreenId = screenId
            };

            if (!isBackgroundImage)
            {
                vm.PromotionSecondsSelectList = new SelectList(SecondsValueManager.GetDictionaryKeysList(false));
                var screen = await _bll.Screens.FindAsync(screenId);
                await _bll.SaveChangesAsync();
                if (screen.ShowScheduleSeconds != null)
                {
                    vm.ScheduleSecondsSelectList = new SelectList(
                        SecondsValueManager.GetDictionaryKeysList(true),
                        screen.ShowScheduleSeconds
                    );
                }
                else
                {
                    vm.ScheduleSecondsSelectList = new SelectList(SecondsValueManager.GetDictionaryKeysList(true));
                }
                
            }
            return View(vm);
        }

        // POST: Admin/Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PictureCreateEditViewModel vm, IFormFile file)
        {
            if (file == null || file.Length < 0)
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "File is missing!");
                if (!vm.IsBackgroundPicture)
                {
                    vm.PromotionSecondsSelectList = new SelectList(
                        SecondsValueManager.GetDictionaryKeysList(false),
                        vm.ShowPromotionSecondsString);
                    var screen = await _bll.Screens.FindAsync(vm.ScreenId);
                    await _bll.SaveChangesAsync();
                    if (screen.ShowScheduleSeconds != null)
                    {
                        vm.ScheduleSecondsSelectList = new SelectList(
                            SecondsValueManager.GetDictionaryKeysList(true),
                            screen.ShowScheduleSeconds
                        );
                    }
                    else
                    {
                        vm.ScheduleSecondsSelectList = new SelectList(SecondsValueManager.GetDictionaryKeysList(true));
                    }

                }
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);
            vm.Picture.CreatedAt = vm.Picture.ChangedAt = DateTime.Now;
            vm.Picture.CreatedBy = vm.Picture.ChangedBy = userId;

            var innerDirName = vm.IsBackgroundPicture ? BackgroundDirectory : PromotionsDirectory;
            var directoryPath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot", "Images", innerDirName);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            var path = Path.Combine(directoryPath, file.FileName);
            vm.Picture.Path = path.Substring(path.IndexOf("Images", StringComparison.Ordinal) - 1).Replace("\\", "/");

            try
            {
                await using var fileStream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            var guid = await _bll.Pictures.AddAsync(vm.Picture);
            await _bll.SaveChangesAsync();

            var picture = _bll.Pictures.GetUpdatesAfterUowSaveChanges(guid);
            string? showAddSeconds = SecondsValueManager.GetSelectedValue(null, false);

            // Promotion was added and ShowScheduleSeconds and ShowAddSeconds values were selected
            if (vm.IsBackgroundPicture == false && vm.ShowPromotionSecondsString != null && vm.ShowScheduleSecondsString != null)
            {
                showAddSeconds = vm.ShowPromotionSecondsString;
                var screen = await _bll.Screens.FindAsync(vm.ScreenId);
                screen.ShowScheduleSeconds = vm.ShowScheduleSecondsString;
                _bll.Screens.Update(screen);
            }
            
            await _bll.PictureInScreens.AddAsync(new PictureInScreen
            {
                CreatedAt = DateTime.Now,
                ChangedAt = DateTime.Now,
                ChangedBy = userId,
                CreatedBy = userId,
                IsBackgroundPicture = vm.IsBackgroundPicture,
                ScreenId = vm.ScreenId,
                PictureId = picture.Id,
                ShowAddSeconds = showAddSeconds
            });

            await _bll.SaveChangesAsync();

            return RedirectToAction("Index", "ScreenSettings");
        }

        // GET: Admin/Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _bll.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            return View(picture);
        }

        // POST: Admin/Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Picture picture, IFormFile file)
        {
            if (id != picture.Id)
            {
                return NotFound();
            }

            picture.ChangedAt = DateTime.Now;
            picture.ChangedBy = _userManager.GetUserId(User);

            // Delete old image
            var oldImagePathArray = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot", Path.Combine(picture.Path.Split("/")));
            if (System.IO.File.Exists(oldImagePathArray))
            {
                System.IO.File.Delete(oldImagePathArray);
            }

            // Create new picture path
            var newImagePath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot", "Images", BackgroundDirectory, file.FileName);
            picture.Path = newImagePath.Substring(newImagePath.IndexOf("Images", StringComparison.Ordinal) - 1).Replace("\\", "/");

            try
            {
                // Copy picture to path
                await using var fileStream = new FileStream(newImagePath, FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _bll.Pictures.Update(picture);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureExists(picture.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "ScreenSettings");
            }

            return View(picture);
        }

        private bool PictureExists(int id)
        {
            return _bll.Pictures.FindAsync(id) != null;
        }
    }
}
