using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeableAppWeb.Areas.Admin.Helpers;
using TimeableAppWeb.Areas.Admin.ViewModels.AppUserViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin))]
    public class AppUsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBLLApp _bll;

        public AppUsersController(UserManager<AppUser> userManager, IBLLApp bll)
        {
            _userManager = userManager;
            _bll = bll;
        }

        // GET: Admin/AppUsers
        public async Task<IActionResult> Index()
        {
            var users = new List<AppUserIndexViewModelDtos>();

            var loggedInUserId = _userManager.GetUserId(User);

            foreach (var user in _userManager.Users)
            {
                if(user.Id.ToString() == loggedInUserId)
                    continue;

                var roles = await _userManager.GetRolesAsync(user);
                var userScreen = await _bll.AppUsersScreens.GetScreenForUserAsync(user.Id.ToString());
                users.Add(new AppUserIndexViewModelDtos
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FirstName + " " + user.LastName,
                    ScheduleManagement = roles.Contains(nameof(RoleNamesEnum.ScheduleSettingsAdmin)),
                    EventsManagement = roles.Contains(nameof(RoleNamesEnum.EventSettingsAdmin)),
                    ScreenManagement = roles.Contains(nameof(RoleNamesEnum.ScreenSettingsAdmin)),
                    UserHasScreen = userScreen != null
                });
            }

            return View(users);
        }

        // GET: Admin/AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = vm.Email,
                    Email = vm.Email,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    ChangedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _userManager.GetUserId(User),
                    ChangedBy = _userManager.GetUserId(User),
                    Activated = false
                };

                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    var errorList = new List<string?>()
                    {
                        vm.ScheduleManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.ScheduleSettingsAdmin))
                            : null,
                        vm.ScreenManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.ScreenSettingsAdmin))
                            : null,
                        vm.EventsManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.EventSettingsAdmin))
                            : null
                    };
                    if (errorList.TrueForAll(e => e == null))
                    {
                        var screen = await _bll.Screens.AllAsync();
                        // If there is only one screen!
                        if (screen.Count() == 1)
                        {
                            await _bll.AppUsersScreens.AddAsync(new AppUsersScreen
                            {
                                CreatedAt = DateTime.Now,
                                CreatedBy = _userManager.GetUserId(User),
                                AppUserId = user.Id,
                                ScreenId = screen.First().Id
                            });
                            await _bll.SaveChangesAsync();
                        }
                        return RedirectToAction("Index");
                    }

                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError("UserAdditionFailed", error);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("UserAdditionFailed", error.Description);
                }
            }

            return View(vm);
        }

        // GET: Admin/AppUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id.ToString());
            if (appUser == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            var vm = new AppUserEditViewModel
            {
                AppUserId = (Guid)id,
                Email = appUser.Email,
                FullName = appUser.FirstName + " " + appUser.LastName,
                ScheduleManagement = roles.Contains(nameof(RoleNamesEnum.ScheduleSettingsAdmin)),
                EventsManagement = roles.Contains(nameof(RoleNamesEnum.EventSettingsAdmin)),
                ScreenManagement = roles.Contains(nameof(RoleNamesEnum.ScreenSettingsAdmin))
            };
            return View(vm);
        }

        // POST: Admin/AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AppUserEditViewModel vm)
        {
            if (id != vm.AppUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(vm.AppUserId.ToString());
                    var errorList = new List<string?>()
                    {
                        vm.ScheduleManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.ScheduleSettingsAdmin))
                            : await RemoveUserFromRoleAsync(user, nameof(RoleNamesEnum.ScheduleSettingsAdmin)),
                        vm.ScreenManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.ScreenSettingsAdmin))
                            : await RemoveUserFromRoleAsync(user, nameof(RoleNamesEnum.ScreenSettingsAdmin)),
                        vm.EventsManagement
                            ? await AddUserToRoleAsync(user, nameof(RoleNamesEnum.EventSettingsAdmin))
                            : await RemoveUserFromRoleAsync(user, nameof(RoleNamesEnum.EventSettingsAdmin))
                    };
                    if (errorList.TrueForAll(e => e == null))
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError("UserAdditionFailed", error);
                    }

                    return View(vm);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await AppUserExists(vm.AppUserId)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(vm);
        }

        // GET: Admin/AppUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.FindByIdAsync(id.ToString());

            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: Admin/AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appUser = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(appUser);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AppUserExists(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString()) != null;
        }

        private async Task<string?> AddUserToRoleAsync(AppUser user, string roleName)
        {
            if ((await _userManager.GetRolesAsync(user)).Any(e => e.Contains(roleName)))
            {
                return null;
            }
            var userToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!userToRoleResult.Succeeded)
            {
                return $"User (ID: {user.Id}, full name: {user.FirstName} {user.LastName}) was not added to role {roleName}";
            }

            return null;
        }

        private async Task<string?> RemoveUserFromRoleAsync(AppUser user, string roleName)
        {
            if (!(await _userManager.GetRolesAsync(user)).Any(e => e.Contains(roleName)))
            {
                return null;
            }

            var userFromRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!userFromRoleResult.Succeeded)
            {
                return $"User (ID: {user.Id}, full name: {user.FirstName} {user.LastName}) was not removed from role {roleName}";
            }

            return null;
        }

        public async Task<IActionResult> AddScreenToUser(Guid id)
        {
            var screen = await _bll.Screens.AllAsync();

            if (!screen.Any())
            {
                return RedirectToAction("Create", "ScreenSettings");
            }

            await _bll.AppUsersScreens.AddAsync(new AppUsersScreen
            {
                CreatedAt = DateTime.Now,
                CreatedBy = _userManager.GetUserId(User),
                AppUserId = id,
                ScreenId = screen.First().Id
            });
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
