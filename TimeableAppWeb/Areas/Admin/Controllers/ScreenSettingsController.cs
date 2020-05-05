using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Helpers;
using BLL.App.Mappers;
using BLL.DTO;
using Contracts.BLL.App;
using Domain.Identity;
using HTMLParser;
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
        public async Task<IActionResult> Index()
        {
            var vm = new ScreenIndexViewModel();

            var userScreen = await _bll.AppUsersScreens.GetScreenForUserAsync(_userManager.GetUserId(User));
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

            var screenPromotions = (await _bll.PictureInScreens.GetAllPromotionsForScreen(vm.Screen.Id));
            foreach (var screenPromotion in screenPromotions)
            {
                vm.Promotions.Add(screenPromotion);
            }

            // Check user rights
            var user = await _userManager.GetUserAsync(User);
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
                await GetAndSaveScheduleForScreen(updatedScreen);

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

            var promotions = (await _bll.PictureInScreens.GetAllPromotionsForScreen((int) id)).ToList();
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
                    await GetAndSaveScheduleForScreen(vm.Screen);
                }
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

            return RedirectToAction(nameof(Index));
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

            var path = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot",
                Path.Combine(bgPicture.Path.Split("/")));
            System.IO.File.Delete(path);

            _bll.Pictures.Remove(bgPicture);

            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScreenSettingsAdmin))]
        public async Task<IActionResult> DeletePromotionFromScreen(int promotionId)
        {
            var picture = await _bll.Pictures.FindAsync(promotionId);

            var path = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot", Path.Combine(picture.Path.Split("/")));

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _bll.Pictures.Remove(picture);

            await _bll.SaveChangesAsync();

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

        public async Task GetAndSaveScheduleForScreen(Screen updatedScreen)
        {
            var timeplanGettingSystem = new GetTimePlanFromInformationSystem(updatedScreen.Prefix);
            var schedule = timeplanGettingSystem.GetScheduleForToday();

            var bllSchedule =
                ScheduleMapper.MapFromInternal(DAL.App.Mappers.ScheduleMapper.MapFromDomain(schedule));

            bllSchedule.Prefix = updatedScreen.Prefix;

            var scheduleGuid = await _bll.Schedules.AddAsync(bllSchedule);
            await _bll.SaveChangesAsync();

            var scheduleIdAfterSaveChanges = _bll.Schedules.GetUpdatesAfterUowSaveChanges(scheduleGuid).Id;

            var subjects = schedule.SubjectsInSchedules;

            if (subjects != null)
            {
                foreach (var subjectInSchedule in subjects)
                {
                    var subjectInScheduleThatAlreadyExists =
                        await _bll.SubjectInSchedules.FindByUniqueIdentifierAsync(subjectInSchedule.UniqueIdentifier);
                    if (subjectInScheduleThatAlreadyExists != null)
                    {
                        subjectInScheduleThatAlreadyExists.ScheduleId = scheduleIdAfterSaveChanges;
                        _bll.SubjectInSchedules.Update(subjectInScheduleThatAlreadyExists);
                        await _bll.SaveChangesAsync();
                        continue;
                    }

                    var bllSubjectInSchedule = new SubjectInSchedule
                    {
                        CreatedAt = DateTime.Now,
                        ChangedAt = DateTime.Now,
                        ChangedBy = _userManager.GetUserId(User),
                        CreatedBy = _userManager.GetUserId(User),
                        Rooms = subjectInSchedule.Rooms,
                        Groups = subjectInSchedule.Groups,
                        UniqueIdentifier = subjectInSchedule.UniqueIdentifier,
                        StartDateTime = subjectInSchedule.StartDateTime,
                        EndDateTime = subjectInSchedule.EndDateTime,
                        SubjectType = subjectInSchedule.SubjectType,
                        ScheduleId = scheduleIdAfterSaveChanges
                    };

                    var subject = await _bll.Subjects
                        .FindBySubjectNameAndCodeAsync(subjectInSchedule.Subject.SubjectName,
                            subjectInSchedule.Subject.SubjectCode);
                    if (subject != null)
                    {
                        bllSubjectInSchedule.SubjectId = subject.Id;
                        bllSubjectInSchedule.Subject = null;
                    }
                    else
                    {
                        var bllSubject = new Subject
                        {
                            CreatedAt = DateTime.Now,
                            ChangedAt = DateTime.Now,
                            SubjectCode = subjectInSchedule.Subject.SubjectCode,
                            SubjectName = subjectInSchedule.Subject.SubjectName
                        };
                        var subjectGuid = await _bll.Subjects.AddAsync(bllSubject);
                        await _bll.SaveChangesAsync();
                        bllSubjectInSchedule.SubjectId = _bll.Subjects.GetUpdatesAfterUowSaveChanges(subjectGuid).Id;
                    }

                    var teachers = new List<Teacher>();

                    if (subjectInSchedule.TeacherInSubjectEvents != null)
                    {
                        foreach (var teacherInSubjectEvent in subjectInSchedule.TeacherInSubjectEvents)
                        {
                            var teacher = await _bll.Teachers
                                .FindTeacherByNameAndRoleAsync(teacherInSubjectEvent.Teacher.FullName,
                                    teacherInSubjectEvent.Teacher.Role);

                            if (teacher != null)
                            {
                                teachers.Add(teacher);
                            }
                            else
                            {
                                var newTeacher = new Teacher
                                {
                                    CreatedAt = DateTime.Now,
                                    ChangedAt = DateTime.Now,
                                    FullName = teacherInSubjectEvent.Teacher.FullName,
                                    Role = teacherInSubjectEvent.Teacher.Role
                                };
                                var teacherGuid = await _bll.Teachers.AddAsync(newTeacher);
                                await _bll.SaveChangesAsync();
                                teachers.Add(_bll.Teachers.GetUpdatesAfterUowSaveChanges(teacherGuid));
                            }
                        }
                    }

                    var subjInScheduleGuid = await _bll.SubjectInSchedules.AddAsync(bllSubjectInSchedule);
                    await _bll.SaveChangesAsync();
                    var subjectInScheduleAfterUpdate =
                        _bll.SubjectInSchedules.GetUpdatesAfterUowSaveChanges(subjInScheduleGuid);
                    foreach (var teacher in teachers)
                    {
                        _bll.TeacherInSubjectEvents.Add(new TeacherInSubjectEvent
                        {
                            CreatedAt = DateTime.Now,
                            ChangedAt = DateTime.Now,
                            TeacherId = teacher.Id,
                            SubjectInScheduleId = subjectInScheduleAfterUpdate.Id
                        });
                    }
                    await _bll.SaveChangesAsync();
                }
            }

            await _bll.ScheduleInScreens.AddAsync(new ScheduleInScreen
            {
                CreatedAt = DateTime.Now,
                ChangedAt = DateTime.Now,
                ScreenId = updatedScreen.Id,
                ScheduleId = scheduleIdAfterSaveChanges
            });

            var futureEvents = await _bll.Events.GetAllFutureEventsAsync(DateTime.Now);

            foreach (var futureEvent in futureEvents)
            {
                if (futureEvent.ShowStartDateTime <= DateTime.Now && futureEvent.ShowEndDateTime > DateTime.Now)
                {
                    await _bll.EventInSchedules.AddAsync(new EventInSchedule
                    {
                        CreatedAt = DateTime.Now,
                        ScheduleId = schedule.Id,
                        EventId = futureEvent.Id
                    });
                }
            }

            await _bll.SaveChangesAsync();
        }
    }
}