using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Helpers;
using BLL.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using TimeableAppWeb.ViewModels;

namespace TimeableAppWeb.Controllers
{
    public class TimetablesController : Controller
    {
        private readonly IBLLApp _bll;

        public TimetablesController(IBLLApp bll)
        {
            _bll = bll;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var screen = await _bll.Screens.GetFirstAndActiveScreenAsync();

            if (screen == null)
                return RedirectToAction("Index", "Home", new { noActiveScreen = true });

            var scheduleInScreen =
                await _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(screen.Id, screen.Prefix, DateTime.Today);

            var vm = new TimetableIndexViewModel
            {
                Events = new List<EventForTimetable>(),
                Promotions = new List<PromotionsForTimetable>(),
                BackgroundPicture = (await _bll.PictureInScreens.GetBackgroundPictureForScreen(screen.Id))?.Picture,
                ShowScheduleSeconds = SecondsValueManager.GetIntValue(screen.ShowScheduleSeconds),
                WeekNumber = 0
            };

            // Get all promotions
            var pictureInScreens = await _bll.PictureInScreens.GetAllPromotionsForTimetableAsync(screen.Id);
            var promotions = pictureInScreens.ToList();
            if (promotions.Any())
            {
                vm.Promotions = promotions.ToList();
            }

            if (scheduleInScreen != null)
            {
                var schedule = scheduleInScreen.Schedule;
                var lecturesForTimetable = (await _bll.SubjectInSchedules.GetAllSubjectsForTimetableByScheduleIdWithoutFinishedAsync(schedule.Id, DateTime.Now)).ToList();

                foreach (var lectureForTimetable in lecturesForTimetable)
                {
                    var teacherNames = new List<string>();
                    (await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(lectureForTimetable.SubjectInScheduleId)).ToList().ForEach(e => teacherNames.Add(e.Teacher.TeacherName));
                    lectureForTimetable.Lecturers = string.Join(", ", teacherNames);
                }
                vm.LecturesForTimetable = lecturesForTimetable;
                var eventsInSchedule = (await _bll.EventInSchedules.GetAllEventsForCurrentScheduleAsync(scheduleInScreen.Schedule.Id)).ToList();
                foreach (var eventInSchedule in eventsInSchedule)
                {
                    vm.Events.Add(eventInSchedule);
                }

                vm.WeekNumber =schedule.WeekNumber;
            }
            else
            {
                vm.LecturesForTimetable = new List<SubjectForTimetableAndSettings>();
            }

            return View(vm);

        }
    }
}
