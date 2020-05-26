using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailSender _emailSender;
        private readonly IBLLApp _bll;

        public AppUsersController(UserManager<AppUser> userManager, IBLLApp bll, IEmailSender emailSender)
        {
            _userManager = userManager;
            _bll = bll;
            _emailSender = emailSender;
        }

        // GET: Admin/AppUsers
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var currentUserScreen = (await _bll.AppUsersScreens.GetScreenForUserAsync(loggedInUser.Id.ToString())).Screen;

            if (currentUserScreen == null)
            {
                return RedirectToAction("Create", "ScreenSettings");
            }

            var users = new List<AppUserIndexViewModelDtos>();

            foreach (var user in _userManager.Users)
            {
                if(user.Id == loggedInUser.Id)
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
                    UserIsHeadAdmin = roles.Contains(nameof(RoleNamesEnum.HeadAdmin)),
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
                    ChangedBy = _userManager.GetUserId(User)
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
                        var screens = screen.ToList();

                        if (screens.Count == 1)
                        {
                            await _bll.AppUsersScreens.AddAsync(new AppUsersScreen
                            {
                                CreatedAt = DateTime.Now,
                                CreatedBy = _userManager.GetUserId(User),
                                AppUserId = user.Id,
                                ScreenId = screens.First().Id
                            });
                            await _bll.SaveChangesAsync();
                        }

                        var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                        passwordCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordCode));

                        var accountCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        accountCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(accountCode));

                        var callbackUrl = Url.Action(
                            "ActivateAccountAndResetPassword",
                            "Account",
                            new { Area = "", accountCode, passwordCode },
                            Request.Scheme);

                        var htmlMessageText = "<h4>Timeable registration notification!</h4>" +
                                              "<p>You have been registered to Timeable application! " +
                                              $"Your email can be verified by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking this link</a>.</p>";

                        await _emailSender.SendEmailAsync(
                            vm.Email,
                            "You have been registered to Timeable",
                            htmlMessageText);

                        return RedirectToAction(nameof(Index));
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

            // Disable head admin editing!
            if (roles.Contains(nameof(RoleNamesEnum.HeadAdmin)))
            {
                return RedirectToAction(nameof(Index));
            }

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

                    var roles = await _userManager.GetRolesAsync(user);
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

            var screens = screen.ToList();
            if (!screens.Any())
            {
                return RedirectToAction("Create", "ScreenSettings");
            }

            await _bll.AppUsersScreens.AddAsync(new AppUsersScreen
            {
                CreatedAt = DateTime.Now,
                CreatedBy = _userManager.GetUserId(User),
                AppUserId = id,
                ScreenId = screens.First().Id
            });
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
