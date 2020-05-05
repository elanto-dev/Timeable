using System;
using System.Linq;
using Domain;
using Domain.Identity;
using HTMLParser;
using HTMLParser.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Helpers
{
    public class DataInitializers
    {
        private const string UserName = "johndoe@gmail.ee";
        private const string Password = "XSy3Vfwd9dWQrLFrz8yQ6e8gX7Kqzbr6";
        private const string FirstName = "John";
        private const string LastName = "Doe";

        public static void MigrateDatabase(AppDbContext context)
        {
            context.Database.Migrate();
        }
        public static bool DeleteDatabase(AppDbContext context)
        {
            return context.Database.EnsureDeleted();
        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {

            var roleNames = new[] { "HeadAdmin", "ScreenSettingsAdmin", "ScheduleSettingsAdmin", "EventSettingsAdmin" };
            foreach (var roleName in roleNames)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;
                if (role == null)
                {
                    role = new AppRole();
                    role.Name = roleName;
                    role.CreatedAt = role.ChangedAt = DateTime.Now;
                    role.CreatedBy = role.ChangedBy = null;
                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed!");
                    }
                }

            }

            var user = userManager.FindByNameAsync(UserName).Result;
            if (user == null)
            {
                user = new AppUser { UserName = UserName, Email = UserName };
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.ChangedAt = user.CreatedAt = DateTime.Now;
                user.Activated = false;
                var userResult = userManager.CreateAsync(user, Password).Result;
                if (!userResult.Succeeded)
                {
                    throw new ApplicationException("User creation failed!");

                }

                var userToRoleResult = userManager.AddToRoleAsync(user, roleNames[0]).Result;
                if (!userToRoleResult.Succeeded)
                {
                    throw new ApplicationException("User to role addition failed!");

                }
            }
        }

        public static void SeedData(UserManager<AppUser> userManager, AppDbContext context, string prefix)
        {
            Screen screen;
            if (context.Screens.Any())
            {
                screen = context.Screens.First();
                prefix = screen.Prefix;
            }
            else
            {
                if(userManager.FindByNameAsync(UserName).Result == null)
                    return;

                screen = new Screen()
                {
                    CreatedAt = DateTime.Now,
                    ChangedAt = DateTime.Now,
                    Prefix = prefix,
                    IsActive = false,
                    UniqueIdentifier = Guid.NewGuid().ToString()
                };

                context.Screens.Add(screen);

                context.SaveChanges();

                if (context.Screens.Any(s => s.Prefix.Equals(prefix)))
                {
                    context.AppUsersScreens.Add(new AppUsersScreen()
                    {
                        CreatedAt = DateTime.Now,
                        ChangedAt = DateTime.Now,
                        AppUserId = userManager.FindByNameAsync(UserName).Result.Id,
                        ScreenId = screen.Id
                    });
                }
                else
                {
                    return;
                }
            }

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

            schedule.Prefix = prefix;

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

            context.Schedules.Add(schedule);
            context.SaveChanges();

            context.ScheduleInScreens.Add(new ScheduleInScreen
            {
                CreatedAt = DateTime.Now,
                ChangedAt = DateTime.Now,
                ScreenId = screen.Id,
                ScheduleId = schedule.Id
            });

            context.SaveChanges();
        }
    }
}
