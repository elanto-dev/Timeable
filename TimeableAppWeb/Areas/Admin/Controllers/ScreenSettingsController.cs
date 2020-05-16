using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Helpers;
using BLL.DTO;
using Contracts.BLL.App;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeableAppWeb.Areas.Admin.Helpers;
using TimeableAppWeb.Areas.Admin.ViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ScreenSettingsController : Controller
    {
        private readonly IHostEnvironment _appEnvironment;
        private readonly IBLLApp _bll;
        private readonly UserManager<AppUser> _userManager;

        public ScreenSettingsController(IBLLApp bll, IHostEnvironment appEnvironment, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        // GET: Admin/Screens
        public async Task<IActionResult> Index(bool showNoActiveScreenAlert = false)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!user.Activated)
            {
                return RedirectToAction("Activate", "Home");
            }

            var vm = new ScreenIndexViewModel
            {
                ShowScreenNotActiveAlert = showNoActiveScreenAlert
            };

            var userScreen = await _bll.AppUsersScreens.GetScreenForUserAsync(user.Id.ToString());
            var screen = userScreen?.Screen;

            if (screen == null)
                return RedirectToAction("Create");

            var scheduleForScreen =
                await _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(screen.Id, screen.Prefix, DateTime.Today);

            if (scheduleForScreen == null)
            {
                vm.ShowPrefixError = true;
            }
            else
            {
                var subjectsForSchedule =
                    await _bll.SubjectInSchedules.SubjectsInScheduleExistForScheduleAsync(scheduleForScreen.ScheduleId);
                vm.ShowPrefixError = !subjectsForSchedule;
            }

            vm.Screen = screen;
            if (vm.Promotions == null)
            {
                vm.Promotions = new List<PictureInScreen>();
            }

            var screenPromotions = (await _bll.PictureInScreens.GetAllPromotionsForScreenAsync(vm.Screen.Id));
            foreach (var screenPromotion in screenPromotions)
            {
                vm.Promotions.Add(screenPromotion);
            }

            // Check user rights
            var userRoles = await _userManager.GetRolesAsync(user);
            vm.UserHasRightsToEdit = userRoles.Contains(nameof(RoleNamesEnum.ScreenSettingsAdmin))
                                     || userRoles.Contains(nameof(RoleNamesEnum.HeadAdmin));

            vm.BackgroundPicture = (await _bll.PictureInScreens.GetBackgroundPictureForScreen(vm.Screen.Id))?.Picture;
            return View(vm);
        }

        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        // GET: Admin/Screens/Create
        public async Task<IActionResult> Create()
        {
            if ((await _bll.AppUsersScreens.GetScreenForUserAsync(_userManager.GetUserId(User))) != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // TODO! Some workaround for this!!!!
            if (await _bll.Screens.GetFirstAndActiveScreenAsync() != null)
            {
                return NotFound();
            }

            var screen = new Screen();
            screen.ShowScheduleSeconds = SecondsValueManager.GetSelectedValue(null, true);
            return View(screen);
        }

        // POST: Admin/Screens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> Create(Screen screen)
        {
            screen.CreatedAt = screen.ChangedAt = DateTime.Now;
            screen.CreatedBy = screen.ChangedBy = _userManager.GetUserId(User);
            screen.UniqueIdentifier = Guid.NewGuid().ToString();
            ModelState.Clear();
            TryValidateModel(screen);
            if (ModelState.IsValid)
            {
                var guid = await _bll.Screens.AddAsync(screen);
                await _bll.SaveChangesAsync();

                var updatedScreen = _bll.Screens.GetUpdatesAfterUowSaveChanges(guid);

                await _bll.AppUsersScreens.AddAsync(new AppUsersScreen
                {
                    CreatedAt = DateTime.Now,
                    ChangedAt = DateTime.Now,
                    AppUserId = Guid.Parse(_userManager.GetUserId(User)),
                    ScreenId = updatedScreen.Id
                });

                await _bll.SaveChangesAsync();
                await ScheduleUpdateService.GetAndSaveScheduleForScreen(_bll, _userManager.GetUserId(User), updatedScreen);

                return RedirectToAction(nameof(Index));
            }

            return View(screen);
        }

        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        // GET: Admin/Screens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screen = await _bll.Screens.FindAsync(id);
            if (screen == null)
            {
                return NotFound();
            }

            var vm = new ScreenCreateEditViewModel
            {
                Screen = screen,
                ScheduleAlwaysShown = false,
                PictureInScreens = new List<PictureInScreen>(),
                PromotionSecondsSelectListDictionary = new Dictionary<int, SelectList>(),
                ScheduleSecondsSelectList = new SelectList(
                    SecondsValueManager.GetDictionaryKeysList(true),
                    screen.ShowScheduleSeconds
                ),
                ShowScheduleSecondsString = screen.ShowScheduleSeconds,
                ScreenOldPrefix = screen.Prefix,
                ShowPromotionSecondsStringDictionary = new Dictionary<int, string>()
            };

            var promotions = (await _bll.PictureInScreens.GetAllPromotionsForScreenAsync((int) id)).ToList();
            await _bll.SaveChangesAsync();

            foreach (var promotion in promotions)
            {
                vm.PictureInScreens.Add(promotion);
                vm.PromotionSecondsSelectListDictionary[promotion.Id] = new SelectList(
                    SecondsValueManager.GetDictionaryKeysList(false),
                    promotion.ShowAddSeconds);
                vm.ShowPromotionSecondsStringDictionary.Add(promotion.Id, promotion.ShowAddSeconds);
            }

            if (vm.Screen.ShowScheduleSeconds.Equals(SecondsValueManager.GetSelectedValue(null, true)) &&
                vm.PictureInScreens.TrueForAll(p => p.ShowAddSeconds.Equals(SecondsValueManager.GetSelectedValue(null, false))))
                vm.ScheduleAlwaysShown = true;

            return View(vm);
        }

        // POST: Admin/Screens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> Edit(int id, ScreenCreateEditViewModel vm)
        {
            if (id != vm.Screen.Id)
            {
                return NotFound();
            }

            vm.Screen.ChangedAt = DateTime.Now;
            vm.Screen.ChangedBy = _userManager.GetUserId(User);

            // If there is no promotions - show schedule always
            vm.Screen.ShowScheduleSeconds = vm.ShowPromotionSecondsStringDictionary == null
                ? SecondsValueManager.GetSelectedValue(null, true)
                : vm.ShowScheduleSecondsString;

            if (vm.PictureInScreens == null)
                vm.PictureInScreens = new List<PictureInScreen>();

            if (vm.PromotionSecondsSelectListDictionary == null)
                vm.PromotionSecondsSelectListDictionary = new Dictionary<int, SelectList>();

            if (vm.ScheduleSecondsSelectList == null)
                vm.ScheduleSecondsSelectList = new SelectList(SecondsValueManager.GetDictionaryKeysList(true));

            if (vm.ShowPromotionSecondsStringDictionary != null)
            {
                foreach (var promotionId in vm.ShowPromotionSecondsStringDictionary.Keys)
                {
                    var promotion = await _bll.PictureInScreens.FindAsync(promotionId);
                    promotion.ShowAddSeconds = vm.ShowPromotionSecondsStringDictionary[promotionId];
                    vm.PictureInScreens.Add(promotion);
                }

                foreach (var pictureInScreen in vm.PictureInScreens)
                {
                    vm.PromotionSecondsSelectListDictionary[pictureInScreen.Id] = new SelectList(
                        SecondsValueManager.GetDictionaryKeysList(false),
                        vm.ShowPromotionSecondsStringDictionary[pictureInScreen.Id]);
                }
            }

            // Before model validation set values to the following parameters to pass model validation.
            vm.ShowPromotionSecondsStringDictionary ??= new Dictionary<int, string>();
            vm.ShowScheduleSecondsString ??= SecondsValueManager.GetSelectedValue(null, true);

            ModelState.Clear();
            TryValidateModel(vm.Screen);
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var pictureInScreen in vm.PictureInScreens)
                    {
                        _bll.PictureInScreens.Update(pictureInScreen);
                    }

                    _bll.Screens.Update(vm.Screen);
                    await _bll.SaveChangesAsync();

                    if (vm.ScreenOldPrefix != vm.Screen.Prefix)
                    {
                        await ScheduleUpdateService.GetAndSaveScheduleForScreen(_bll, _userManager.GetUserId(User), vm.Screen);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScreenExists(vm.Screen.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Edit));
        }

        // GET: Admin/Screens/Delete/5
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var screen = await _bll.Screens.FindAsync(id);

            if (screen == null)
            {
                return NotFound();
            }

            return View(screen);
        }

        // POST: Admin/Screens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screen = await _bll.Screens.FindAsync(id);
            _bll.Screens.Remove(screen);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScreenExists(int id)
        {
            return _bll.Screens.Find(id) != null;
        }

        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> UseDefaultBackground(int screenId)
        {
            var screen = await _bll.Screens.FindAsync(screenId);
            var bgPicture = (await _bll.PictureInScreens.GetBackgroundPictureForScreen(screen.Id))?.Picture;

            if (bgPicture == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var picturesWithSamePath = await _bll.Pictures.FindPicturesByPathAsync(bgPicture.Path);

            if (picturesWithSamePath == null || !picturesWithSamePath.Any() || (picturesWithSamePath.Count() == 1 && picturesWithSamePath.First().Id == bgPicture.Id))
            {
                var path = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot",
                    Path.Combine(bgPicture.Path.Split("/")));

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _bll.Pictures.Remove(bgPicture);

            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> DeletePromotionFromScreen(int promotionId)
        {
            var promotionInScreen = await _bll.PictureInScreens.FindAsync(promotionId);
            var picturesWithSamePath = await _bll.Pictures.FindPicturesByPathAsync(promotionInScreen.Picture.Path);
            if (picturesWithSamePath == null || !picturesWithSamePath.Any() || (picturesWithSamePath.Count() == 1 && picturesWithSamePath.First().Id == promotionId))
            {
                var path = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot", 
                    Path.Combine(promotionInScreen.Picture.Path.Split("/")));

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _bll.Pictures.Remove(promotionInScreen.Picture);
            await _bll.SaveChangesAsync();

            var promotions = await _bll.PictureInScreens.GetAllPromotionsForTimetableAsync(promotionInScreen.ScreenId);

            if (promotions == null || !promotions.Any())
            {
                var screen = await _bll.Screens.FindAsync(promotionInScreen.ScreenId);
                screen.ShowScheduleSeconds = SecondsValueManager.GetSelectedValue(null, true);
                _bll.Screens.Update(screen);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> ActivateScreen(int screenId, bool activate)
        {
            var screen = await _bll.Screens.FindAsync(screenId);
            screen.IsActive = activate;
            _bll.Screens.Update(screen);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}