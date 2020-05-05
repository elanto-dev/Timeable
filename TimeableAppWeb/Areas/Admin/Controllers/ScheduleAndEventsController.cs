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
            var userScreen = (await _bll.AppUsersScreens.GetScreenForUserAsync(_userManager.GetUserId(User))).Screen;

            if (userScreen == null)
            {
                return RedirectToAction("Create", "ScreenSettings");
            }

            var scheduleInScreen = await 
                _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(userScreen.Id, userScreen.Prefix, DateTime.Today);
            if (scheduleInScreen == null)
            {
                // TODO!!!!!!
                // Add Update schedule button. Do something with subject adding!
                return RedirectToAction("Index", "ScreenSettings");
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
                    (await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(lectureForTimetable.SubjectInScheduleId)).ToList().ForEach(e => teacherNames.Add(e.Teacher.FullName));
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
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);
            vm.UserHasRightsToEditEvents = userRoles.Contains(nameof(RoleNamesEnum.EventSettingsAdmin))
                                     || userRoles.Contains(nameof(RoleNamesEnum.HeadAdmin));
            vm.UserHasRightsToEditSchedule = userRoles.Contains(nameof(RoleNamesEnum.ScheduleSettingsAdmin))
                                           || userRoles.Contains(nameof(RoleNamesEnum.HeadAdmin));

            return View(vm);
        }


        [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.EventSettingsAdmin) + "," + nameof(RoleNamesEnum.EventSettingsAdmin))]
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
    }
}
