using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.DTO;
using Contracts.BLL.App;
using HTMLParser;

namespace TimeableAppWeb.Areas.Admin.Helpers
{
    public class ScheduleUpdateService
    {
        public static async Task GetAndSaveScheduleForScreen(IBLLApp bll, string userId, Screen screen)
        {
            var timeplanGettingSystem = new GetTimePlanFromInformationSystem(screen.Prefix);
            var schedule = timeplanGettingSystem.GetScheduleForToday();

            var bllSchedule =
                ScheduleMapper.MapFromInternal(DAL.App.Mappers.ScheduleMapper.MapFromDomain(schedule));

            bllSchedule.Prefix = screen.Prefix;

            var scheduleGuid = await bll.Schedules.AddAsync(bllSchedule);
            await bll.SaveChangesAsync();

            var scheduleIdAfterSaveChanges = bll.Schedules.GetUpdatesAfterUowSaveChanges(scheduleGuid).Id;

            var subjects = schedule.SubjectsInSchedules;

            if (subjects != null)
            {
                foreach (var subjectInSchedule in subjects)
                {
                    var subjectInScheduleThatAlreadyExists =
                        await bll.SubjectInSchedules.FindByUniqueIdentifierAsync(subjectInSchedule.UniqueIdentifier);
                    if (subjectInScheduleThatAlreadyExists != null)
                    {
                        subjectInScheduleThatAlreadyExists.ScheduleId = scheduleIdAfterSaveChanges;
                        bll.SubjectInSchedules.Update(subjectInScheduleThatAlreadyExists);
                        await bll.SaveChangesAsync();
                        continue;
                    }

                    var bllSubjectInSchedule = new SubjectInSchedule
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        Rooms = subjectInSchedule.Rooms,
                        Groups = subjectInSchedule.Groups,
                        UniqueIdentifier = subjectInSchedule.UniqueIdentifier,
                        StartDateTime = subjectInSchedule.StartDateTime,
                        EndDateTime = subjectInSchedule.EndDateTime,
                        SubjectType = subjectInSchedule.SubjectType,
                        ScheduleId = scheduleIdAfterSaveChanges
                    };

                    var subject = await bll.Subjects
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
                            CreatedBy = userId,
                            SubjectCode = subjectInSchedule.Subject.SubjectCode,
                            SubjectName = subjectInSchedule.Subject.SubjectName
                        };
                        var subjectGuid = await bll.Subjects.AddAsync(bllSubject);
                        await bll.SaveChangesAsync();
                        bllSubjectInSchedule.SubjectId = bll.Subjects.GetUpdatesAfterUowSaveChanges(subjectGuid).Id;
                    }

                    var teachers = new List<Teacher>();

                    if (subjectInSchedule.TeacherInSubjectEvents != null)
                    {
                        foreach (var teacherInSubjectEvent in subjectInSchedule.TeacherInSubjectEvents)
                        {
                            var teacher = await bll.Teachers
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
                                    CreatedBy = userId,
                                    TeacherName = teacherInSubjectEvent.Teacher.FullName,
                                    TeacherRole = teacherInSubjectEvent.Teacher.Role
                                };
                                var teacherGuid = await bll.Teachers.AddAsync(newTeacher);
                                await bll.SaveChangesAsync();
                                teachers.Add(bll.Teachers.GetUpdatesAfterUowSaveChanges(teacherGuid));
                            }
                        }
                    }

                    var subjInScheduleGuid = await bll.SubjectInSchedules.AddAsync(bllSubjectInSchedule);
                    await bll.SaveChangesAsync();
                    var subjectInScheduleAfterUpdate =
                        bll.SubjectInSchedules.GetUpdatesAfterUowSaveChanges(subjInScheduleGuid);
                    foreach (var teacher in teachers)
                    {
                        bll.TeacherInSubjectEvents.Add(new TeacherInSubjectEvent
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = userId,
                            TeacherId = teacher.Id,
                            SubjectInScheduleId = subjectInScheduleAfterUpdate.Id
                        });
                    }
                    await bll.SaveChangesAsync();
                }
            }

            await bll.ScheduleInScreens.AddAsync(new ScheduleInScreen
            {
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                ScreenId = screen.Id,
                ScheduleId = scheduleIdAfterSaveChanges
            });

            var futureEvents = await bll.Events.GetAllFutureEventsAsync(DateTime.Now);

            foreach (var futureEvent in futureEvents)
            {
                if (futureEvent.ShowStartDateTime <= DateTime.Now && futureEvent.ShowEndDateTime > DateTime.Now)
                {
                    await bll.EventInSchedules.AddAsync(new EventInSchedule
                    {
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId,
                        ScheduleId = scheduleIdAfterSaveChanges,
                        EventId = futureEvent.Id
                    });
                }
            }

            await bll.SaveChangesAsync();
        }
    }
}
