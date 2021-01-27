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
                var subjectsInSchedule = schedule.SubjectsInSchedules.ToList();
                for (var i = 0; i < subjectsInSchedule.Count; i++)
                {
                    var subjectInScheduleFromDb = context.SubjectInSchedules.FirstOrDefault(s =>
                        s.UniqueIdentifier.Equals(subjectsInSchedule[i].UniqueIdentifier));

                    if (subjectInScheduleFromDb != null)
                    {
                        context.SubjectInSchedules.Remove(subjectInScheduleFromDb);
                        context.SaveChanges();
                    }

                    var subject = context.Subjects.FirstOrDefault(t => t.SubjectCode == subjectsInSchedule[i].Subject.SubjectCode
                                                                       && t.SubjectName == subjectsInSchedule[i].Subject.SubjectName);
                    if (subject != null)
                    {
                        subjectsInSchedule[i].Subject = subject;
                        subjectsInSchedule[i].SubjectId = subject.Id;
                    }
                    else
                    {
                        context.Subjects.Add(subjectsInSchedule[i].Subject);

                    }

                    if (subjectsInSchedule[i]?.TeacherInSubjectEvents == null) continue;

                    foreach (var teacherInSubjectEvent in subjectsInSchedule[i].TeacherInSubjectEvents!)
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

                schedule.SubjectsInSchedules = subjectsInSchedule;
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
                context.Events.Where(e => e.ShowStartDateTime <= DateTime.Now && e.EndDateTime >= DateTime.Now);

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

        public static void DeleteScheduleRecordsOlderThan30Days(AppDbContext context)
        {
            var schedules = context.Schedules.Where(s => s.Date < DateTime.Today.AddDays(-30));
            foreach (var schedule in schedules)
            {
                context.Schedules.Remove(schedule);
            }

            var teachers = context.Teachers.Where(t =>
                t.CreatedAt < DateTime.Today.AddDays(-30) && t.ChangedAt == null ||
                t.ChangedAt < DateTime.Today.AddDays(-30));
            foreach (var teacher in teachers)
            {
                context.Teachers.Remove(teacher);
            }

            var subjects = context.Subjects.Where(t =>
                t.CreatedAt < DateTime.Today.AddDays(-30) && t.ChangedAt == null ||
                t.ChangedAt < DateTime.Today.AddDays(-30));
            foreach (var subject in subjects)
            {
                context.Subjects.Remove(subject);
            }

            var oldEvents = context.Events.Where(e => e.EndDateTime < DateTime.Today.AddDays(-30) && e.ShowEndDateTime < DateTime.Today.AddDays(-30));
            foreach (var oldEvent in oldEvents)
            {
                context.Events.Remove(oldEvent);
            }

            context.SaveChanges();
        }
    }
}
