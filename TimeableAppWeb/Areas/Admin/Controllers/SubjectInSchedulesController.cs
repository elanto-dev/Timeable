using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TimeableAppWeb.Areas.Admin.Helpers;
using TimeableAppWeb.Areas.Admin.ViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.ScheduleSettingsAdmin))]
    public class SubjectInSchedulesController : Controller
    {
        private readonly IBLLApp _bll;
        private readonly UserManager<AppUser> _userManager;

        public SubjectInSchedulesController(IBLLApp bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/SubjectInSchedules/Create
        public IActionResult Create(int scheduleId)
        {
            var vm = new SubjectInScheduleCreateEditViewModel
            {
                SelectList = new SelectList(MapSubjectTypes.GetSubjectTypes()), 
                ScheduleId = scheduleId,
                Teachers = new List<Teacher>()
            };

            return View(vm);
        }

        // POST: Admin/SubjectInSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectInScheduleCreateEditViewModel vm)
        {
            if (vm.ScheduleId == null)
            {
                return NotFound();
            }
            vm.SelectList = new SelectList(MapSubjectTypes.GetSubjectTypes(), vm.SelectedSubjectType);
            vm.SubjectInSchedule.SubjectType = (int)MapSubjectTypes.GetResultSubjectTypeByString(vm.SelectedSubjectType);
            vm.SubjectInSchedule.UniqueIdentifier = $"{vm.Subject.SubjectCode}-{vm.SubjectInSchedule.StartDateTime:yyyyMMddHHmmss}-{vm.SubjectInSchedule.EndDateTime:yyyyMMddHHmmss}";
            vm.SubjectInSchedule.ScheduleId = (int)vm.ScheduleId;
            vm.SubjectInSchedule.Schedule = await _bll.Schedules.FindAsync(vm.ScheduleId); // used to pass through model state check
            if (vm.Teachers == null)
            {
                vm.Teachers = new List<Teacher>();
            }
            ModelState.Clear();
            if (!string.IsNullOrWhiteSpace(vm.SubjectInSchedule.Groups) && !string.IsNullOrWhiteSpace(vm.SubjectInSchedule.Rooms)
                                                                        && TryValidateModel(vm.Subject) 
                                                                        && TryValidateModel(vm.SubjectInSchedule.Groups) 
                                                                        && TryValidateModel(vm.SubjectInSchedule.Rooms))
            {
                if (vm.SubjectInSchedule.StartDateTime == DateTime.MinValue ||
                    vm.SubjectInSchedule.EndDateTime == DateTime.MinValue)
                {
                    ModelState.AddModelError(string.Empty, "Dates should be chosen!");
                    vm.SubjectInSchedule.StartDateTime = vm.SubjectInSchedule.EndDateTime = DateTime.Today;
                    return View(vm);
                }

                if (vm.SubjectInSchedule.StartDateTime >= vm.SubjectInSchedule.EndDateTime)
                {
                    ModelState.AddModelError(string.Empty, "Start time should be before end time!");
                    return View(vm);
                }

                // Add subject
                vm.Subject.CreatedAt = DateTime.Now;
                vm.Subject.CreatedBy = _userManager.GetUserId(User);
                var subjectFromDb = await AddSubjectIfDontExistAsync(vm.Subject);
                vm.SubjectInSchedule.SubjectId = subjectFromDb.Id;
                vm.SubjectInSchedule.Subject = subjectFromDb;

            }
            ModelState.Clear();
            TryValidateModel(vm);
            if (ModelState.IsValid)
            {
                // set schedule to null so there will be no tracking error!
                vm.SubjectInSchedule.Schedule = null; 
                vm.SubjectInSchedule.Subject = null;
                vm.SubjectInSchedule.CreatedAt = DateTime.Now;
                vm.SubjectInSchedule.CreatedBy = _userManager.GetUserId(User);

                // Add SubjectInSchedule
                var subjectInScheduleGuid = await _bll.SubjectInSchedules.AddAsync(vm.SubjectInSchedule);
                await _bll.SaveChangesAsync();

                // If teacher list is not empty - add teachers to subject
                if (vm.Teachers != null && vm.Teachers.Count > 0)
                {
                    var subjectInScheduleFromDb =
                        _bll.SubjectInSchedules.GetUpdatesAfterUowSaveChanges(subjectInScheduleGuid);
                    foreach (var teacher in vm.Teachers)
                    {
                        var teacherFromDb = await AddTeacherIfDontExistAsync(teacher);
                        await _bll.TeacherInSubjectEvents.AddAsync(new TeacherInSubjectEvent
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = _userManager.GetUserId(User),
                            SubjectInScheduleId = subjectInScheduleFromDb.Id,
                            TeacherId = teacherFromDb.Id
                        });
                    }

                    await _bll.SaveChangesAsync();
                }

                return RedirectToAction("Index", "ScheduleAndEvents");

            }

            if (vm.SubjectInSchedule.StartDateTime == DateTime.MinValue ||
                vm.SubjectInSchedule.EndDateTime == DateTime.MinValue)
            {
                ModelState.AddModelError(string.Empty, "Dates should be chosen!");
                vm.SubjectInSchedule.StartDateTime = vm.SubjectInSchedule.EndDateTime = DateTime.Today;
                return View(vm);
            }

            if (vm.SubjectInSchedule.StartDateTime >= vm.SubjectInSchedule.EndDateTime)
            {
                ModelState.AddModelError(string.Empty, "Start time should be before end time!");
                return View(vm);
            }

            return View(vm);
        }

        // GET: Admin/SubjectInSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectInSchedule = await _bll.SubjectInSchedules.FindAsync(id);
            if (subjectInSchedule == null)
            {
                return NotFound();
            }

            var subject = await _bll.Subjects.FindAsync(subjectInSchedule.SubjectId);
            if (subject == null)
            {
                return NotFound();
            }

            var vm = new SubjectInScheduleCreateEditViewModel
            {
                SelectList = new SelectList(
                    MapSubjectTypes.GetSubjectTypes(), 
                    MapSubjectTypes.GetPreviouslySelectedSubjectType((SubjectType)subjectInSchedule.SubjectType)
                    ),
                SubjectInSchedule = subjectInSchedule,
                Subject = subject,
                SelectedSubjectType = MapSubjectTypes.GetPreviouslySelectedSubjectType((SubjectType)subjectInSchedule.SubjectType),
                Teachers = new List<Teacher>()
            };

            var teachersInSubjectEvent =
                await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(subjectInSchedule.Id);

            foreach (var teacherInEvent in teachersInSubjectEvent)
            {
                vm.Teachers.Add(teacherInEvent.Teacher);
            }

            return View(vm);
        }

        // POST: Admin/SubjectInSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectInScheduleCreateEditViewModel vm)
        {
            if (id != vm.SubjectInSchedule.Id)
            {
                return NotFound();
            }
            vm.SelectList = new SelectList(MapSubjectTypes.GetSubjectTypes(), vm.SelectedSubjectType);
            vm.SubjectInSchedule.SubjectType = (int)MapSubjectTypes.GetResultSubjectTypeByString(vm.SelectedSubjectType);
            vm.SubjectInSchedule.UniqueIdentifier = $"{vm.Subject.SubjectCode}-{vm.SubjectInSchedule.StartDateTime:yyyyMMddHHmmss}-{vm.SubjectInSchedule.EndDateTime:yyyyMMddHHmmss}";
            vm.SubjectInSchedule.Schedule = await _bll.Schedules.FindAsync(vm.SubjectInSchedule.ScheduleId); // used to pass through model state check
            if (vm.Teachers == null)
            {
                vm.Teachers = new List<Teacher>();
            }
            ModelState.Clear();

            if (!string.IsNullOrWhiteSpace(vm.SubjectInSchedule.Groups) && !string.IsNullOrWhiteSpace(vm.SubjectInSchedule.Rooms)
                                                                        && TryValidateModel(vm.Subject)
                                                                        && TryValidateModel(vm.SubjectInSchedule.Groups)
                                                                        && TryValidateModel(vm.SubjectInSchedule.Rooms))
            {
                if (vm.SubjectInSchedule.StartDateTime == DateTime.MinValue ||
                    vm.SubjectInSchedule.EndDateTime == DateTime.MinValue)
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.SubjectInSchedule.SubjectInSchedule.DatesChooseError);
                    vm.SubjectInSchedule.StartDateTime = vm.SubjectInSchedule.EndDateTime = DateTime.Today;
                    return View(vm);
                }

                if (vm.SubjectInSchedule.StartDateTime >= vm.SubjectInSchedule.EndDateTime)
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.SubjectInSchedule.SubjectInSchedule.DatesDifferenceError);
                    return View(vm);
                }

                // Add subject
                vm.Subject.ChangedAt = DateTime.Now;
                vm.Subject.ChangedBy = _userManager.GetUserId(User);
                _bll.Subjects.Update(vm.Subject);
                vm.SubjectInSchedule.Subject = vm.Subject;
            }
            ModelState.Clear();
            TryValidateModel(vm);
            if (ModelState.IsValid)
            {
                try
                {
                    // set schedule to null so there will be no tracking error!
                    vm.SubjectInSchedule.Schedule = null;
                    vm.SubjectInSchedule.Subject = null;
                    vm.SubjectInSchedule.ChangedAt = DateTime.Now;
                    vm.SubjectInSchedule.ChangedBy = _userManager.GetUserId(User);

                    // Add SubjectInSchedule
                    _bll.SubjectInSchedules.Update(vm.SubjectInSchedule);
                    await _bll.SaveChangesAsync();

                    // If teacher list is not empty - add teachers to subject
                    if (vm.Teachers != null && vm.Teachers.Count > 0)
                    {
                        var updatedTeachers = new List<Teacher>();
                        foreach (var teacher in vm.Teachers)
                        {
                            var teacherFromDb = await AddTeacherIfDontExistAsync(teacher);
                            updatedTeachers.Add(teacherFromDb);
                        }

                        await AddTeacherInSubjectEventIfDontExistAsync(vm.SubjectInSchedule.Id, updatedTeachers);
                    }
                    else
                    {
                        await RemoveAllTeachersFromSubject(vm.SubjectInSchedule.Id);
                    }

                    return RedirectToAction("Index", "ScheduleAndEvents");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectInScheduleExists(vm.SubjectInSchedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            if (vm.SubjectInSchedule.StartDateTime == DateTime.MinValue ||
                vm.SubjectInSchedule.EndDateTime == DateTime.MinValue)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.SubjectInSchedule.SubjectInSchedule.DatesChooseError);
                vm.SubjectInSchedule.StartDateTime = vm.SubjectInSchedule.EndDateTime = DateTime.Today;
                return View(vm);
            }

            if (vm.SubjectInSchedule.StartDateTime >= vm.SubjectInSchedule.EndDateTime)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.SubjectInSchedule.SubjectInSchedule.DatesDifferenceError);
                return View(vm);
            }
            return View(vm);
        }

        private bool SubjectInScheduleExists(int id)
        {
            return _bll.SubjectInSchedules.Find( id) != null;
        }

        private async Task<Subject> AddSubjectIfDontExistAsync(Subject subject)
        {
            var subjectInDb =
                await _bll.Subjects.FindBySubjectNameAndCodeAsync(subject.SubjectName, subject.SubjectCode);
            if (subjectInDb != null)
            {
                return subjectInDb;
            }

            subject.CreatedAt = DateTime.Now;
            subject.CreatedBy = _userManager.GetUserId(User);
            var subjectGuid = await _bll.Subjects.AddAsync(subject);
            await _bll.SaveChangesAsync();

            return _bll.Subjects.GetUpdatesAfterUowSaveChanges(subjectGuid);
        }

        private async Task<Teacher> AddTeacherIfDontExistAsync(Teacher teacher)
        {
            var teacherInDb =
                await _bll.Teachers.FindTeacherByNameAndRoleAsync(teacher.FullName, teacher.Role);
            if (teacherInDb != null)
            {
                return teacherInDb;
            }

            teacher.CreatedAt = DateTime.Now;
            teacher.CreatedBy = _userManager.GetUserId(User);
            var subjectGuid = await _bll.Teachers.AddAsync(teacher);
            await _bll.SaveChangesAsync();

            return _bll.Teachers.GetUpdatesAfterUowSaveChanges(subjectGuid);
        }

        private async Task AddTeacherInSubjectEventIfDontExistAsync(int subjectEventId, List<Teacher> teachers)
        {
            var teachersInSubjects = await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(subjectEventId);
            foreach (var teacher in teachers)
            {
                if (teachersInSubjects != null && teachersInSubjects.Any())
                {
                    var teacherInSubjectEvent = teachersInSubjects.FirstOrDefault(e => e.Teacher.Id == teacher.Id);
                    if (teacherInSubjectEvent != null)
                    {
                        teacherInSubjectEvent.ChangedAt = DateTime.Now;
                        teacherInSubjectEvent.ChangedBy = _userManager.GetUserId(User);
                        _bll.TeacherInSubjectEvents.Update(teacherInSubjectEvent);
                    }
                    continue;
                }

                await _bll.TeacherInSubjectEvents.AddAsync(new TeacherInSubjectEvent
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = _userManager.GetUserId(User),
                    SubjectInScheduleId = subjectEventId,
                    TeacherId = teacher.Id
                });
            }

            await _bll.SaveChangesAsync();
        }

        private async Task RemoveAllTeachersFromSubject(int subjectEventId)
        {
            var teachersInSubjects = await _bll.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(subjectEventId);
            foreach (var teacherInSubjectEvent in teachersInSubjects)
            {
                _bll.TeacherInSubjectEvents.Remove(teacherInSubjectEvent);
            }

            await _bll.SaveChangesAsync();
        }
    }
}
