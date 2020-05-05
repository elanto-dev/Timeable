using System;
using System.Linq;
using Domain;
using HTMLParser;
using HTMLParser.Interfaces;

namespace DAL.App.Helpers
{
    public class UpdateTimetable
    {
        public static void UpdateScheduleForTimeplan(AppDbContext context)
        {
            if (!context.Screens.Any()) return;

            var screen = context.Screens.First();
            var prefix = screen.Prefix;

            var scheduleFromDb = context.Schedules.FirstOrDefault(s => s.Date == DateTime.Today && s.Prefix == prefix);
            if (scheduleFromDb != null)
            {
                if (context.ScheduleInScreens.Any(s =>
                    s.ScheduleId == scheduleFromDb.Id && s.ScreenId == screen.Id))
                    return;

                context.ScheduleInScreens.Add(new ScheduleInScreen
                {
                    CreatedAt = DateTime.Now,
                    ChangedAt = DateTime.Now,
                    ScreenId = screen.Id,
                    ScheduleId = scheduleFromDb.Id
                });
                context.SaveChanges();
                return;
            }

            IGetTimePlanFromInformationSystem timeplanGettingSystem = new GetTimePlanFromInformationSystem(prefix);
            var schedule = timeplanGettingSystem.GetScheduleForToday();

            if (schedule == null)
                return;

            if (schedule.SubjectsInSchedules != null)
            {
                foreach (var subjectInSchedule in schedule.SubjectsInSchedules)
                {
                    if (context.SubjectInSchedules.Any(s =>
                        s.UniqueIdentifier.Equals(subjectInSchedule.UniqueIdentifier)))
                        continue;

                    var subject = context.Subjects.FirstOrDefault(t => t.SubjectCode == subjectInSchedule.Subject.SubjectCode
                                                                       && t.SubjectName == subjectInSchedule.Subject.SubjectName);
                    if (subject != null)
                    {
                        subjectInSchedule.Subject = subject;
                        subjectInSchedule.SubjectId = subject.Id;
                    }
                    else
                    {
                        context.Subjects.Add(subjectInSchedule.Subject);

                    }

                    if (subjectInSchedule.TeacherInSubjectEvents == null) continue;

                    foreach (var teacherInSubjectEvent in subjectInSchedule.TeacherInSubjectEvents)
                    {
                        var teacher = context.Teachers.FirstOrDefault(
                            t => t.FullName == teacherInSubjectEvent.Teacher.FullName
                                 && t.Role == teacherInSubjectEvent.Teacher.Role);

                        if (teacher != null)
                        {
                            teacherInSubjectEvent.Teacher = teacher;
                            teacherInSubjectEvent.TeacherId = teacher.Id;
                        }
                        else
                        {
                            context.Teachers.Add(teacherInSubjectEvent.Teacher);
                        }
                    }

                    context.SaveChanges();
                }
            }

            schedule.Prefix = prefix;
            context.Schedules.Add(schedule);
            context.SaveChanges();

            context.ScheduleInScreens.Add(new ScheduleInScreen
            {
                CreatedAt = DateTime.Now,
                ChangedAt = DateTime.Now,
                ScreenId = screen.Id,
                ScheduleId = schedule.Id
            });

            var futureEvents =
                context.Events.Where(e => e.ShowStartDateTime < DateTime.Now && e.EndDateTime >= DateTime.Now);

            foreach (var futureEvent in futureEvents)
            {
                context.EventInSchedules.Add(new EventInSchedule
                {
                    CreatedAt = DateTime.Now,
                    ScheduleId = schedule.Id,
                    EventId = futureEvent.Id
                });
            }

            context.SaveChanges();
        }
    }
}
