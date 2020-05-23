using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.App;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeableAppWeb.Areas.Admin.Helpers;
using TimeableAppWeb.Areas.Admin.ViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ScheduleAndEventsController : Controller
    {
        private readonly IBLLApp _bll;
        private readonly UserManager<AppUser> _userManager;

        public ScheduleAndEventsController(IBLLApp bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/Schedules
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var userScreen = (await _bll.AppUsersScreens.GetScreenForUserAsync(user.Id.ToString())).Screen;

            if (userScreen == null)
            {
                return RedirectToAction("Create", "ScreenSettings");
            }

            var scheduleInScreen = await 
                _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(userScreen.Id, userScreen.Prefix, DateTime.Today);

            if (scheduleInScreen == null)
            {
                await ScheduleUpdateService.GetAndSaveScheduleForScreen(_bll, _userManager.GetUserId(User), userScreen);
                scheduleInScreen = await
                    _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(userScreen.Id, userScreen.Prefix, DateTime.Today);
            }

            var vm = new ScheduleAndEventsIndexViewModel
            {
                ScheduleId = scheduleInScreen.ScheduleId,
                WeekNumber = scheduleInScreen.Schedule.WeekNumber,
                Subjects = new List<SubjectForTimetableAndSettings>(),
                FutureEvents = new List<EventForSettings>()
            };
            var subjectsInSchedule = (await _bll.SubjectInSchedules.GetAllSubjectsForTimetableOrSettingsByScheduleIdAsync(scheduleInScreen.ScheduleId)).ToList();

            if (subjectsInSchedule.Any())
            {
                foreach (var lectureForTimetable in subjectsInSchedule)
                {
                    var teacherNames = new List<string>();
                    (await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(lectureForTimetable.SubjectInScheduleId)).ToList().ForEach(e => teacherNames.Add(e.Teacher.TeacherName));
                    lectureForTimetable.Lecturers = string.Join(", ", teacherNames);
                }
                vm.Subjects = subjectsInSchedule;
            }

            var events = await _bll.Events.GetAllFutureEventsForSettingsAsync(DateTime.Today);

            if (events != null)
            {
                foreach (var eventToShow in events)
                {
                    vm.FutureEvents.Add(eventToShow);
                }
            }

            // Check user rights
            var userRoles = await _userManager.GetRolesAsync(user);
            vm.UserHasRightsToEditEvents = userRoles.Contains(nameof(RoleNamesEnum.EventSettingsAdmin))
                                     || userRoles.Contains(nameof(RoleNamesEnum.HeadAdmin));
            vm.UserHasRightsToEditSchedule = userRoles.Contains(nameof(RoleNamesEnum.ScheduleSettingsAdmin))
                                           || userRoles.Contains(nameof(RoleNamesEnum.HeadAdmin));

            return View(vm);
        }


        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScheduleSettingsAdmin) + "," + nameof(RoleNamesEnum.EventSettingsAdmin))]
        public async Task<IActionResult> DeleteEventOrSubject(int id, bool isSubject)
        {
            if (isSubject)
            {
                _bll.SubjectInSchedules.Remove(id);
            }
            else
            {
                _bll.Events.Remove(id);
            }
            await _bll.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScheduleSettingsAdmin))]
        public async Task<IActionResult> RefreshSchedule(int scheduleId)
        {
            var scheduleInScreen = await _bll.ScheduleInScreens.FindByScheduleIdAsync(scheduleId);
            if (scheduleInScreen != null)
            {
                await ScheduleUpdateService.GetAndSaveScheduleForScreen(_bll, _userManager.GetUserId(User),
                    scheduleInScreen.Screen);
            }
            return RedirectToAction("Index");
        }
    }
}
