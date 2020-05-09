using System;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TimeableAppWeb.Areas.Admin.Helpers;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(RoleNamesEnum.HeadAdmin) + "," + nameof(RoleNamesEnum.EventSettingsAdmin))]
    public class EventsController : Controller
    {
        private readonly IBLLApp _bll;
        private readonly UserManager<AppUser> _userManager;

        public EventsController(IBLLApp bll, UserManager<AppUser> userManager)
        {
            _bll = bll;
            _userManager = userManager;
        }

        // GET: Admin/Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event createdEvent)
        {

            if (createdEvent.EndDateTime < createdEvent.StartDateTime)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.EventView.Events.EventTimeDifferenceError);
            }
            if (createdEvent.ShowEndDateTime <= createdEvent.ShowStartDateTime)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.EventView.Events.EventShowTimeDifferenceError);
            }

            if (ModelState.IsValid)
            {
                if (ModelState.ErrorCount > 0)
                {
                    return View(createdEvent);
                }
                createdEvent.CreatedAt = DateTime.Now;
                createdEvent.CreatedBy = _userManager.GetUserId(User);
                var eventGuid = await _bll.Events.AddAsync(createdEvent);
                await _bll.SaveChangesAsync();
                if (DateTime.Now >= createdEvent.ShowStartDateTime && DateTime.Now < createdEvent.ShowEndDateTime)
                {
                    var eventId = _bll.Events.GetUpdatesAfterUowSaveChanges(eventGuid).Id;
                    var screen = await _bll.Screens.GetFirstAndActiveScreenAsync();

                    if (screen != null)
                    {
                        var scheduleInScreen = await _bll.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(screen.Id, screen.Prefix, DateTime.Today);
                        if (scheduleInScreen != null)
                        {
                            var eventInSchedule = new EventInSchedule
                            {
                                CreatedAt = DateTime.Now,
                                CreatedBy = _userManager.GetUserId(User),
                                EventId = eventId,
                                ScheduleId = scheduleInScreen.ScheduleId
                            };
                            await _bll.EventInSchedules.AddAsync(eventInSchedule);
                            await _bll.SaveChangesAsync();
                        }
                    }
                }
                return RedirectToAction("Index", "ScheduleAndEvents");
            }

            return View(createdEvent);
        }

        // GET: Admin/Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foundEvent = await _bll.Events.FindAsync(id);
            if (foundEvent == null)
            {
                return NotFound();
            }

            return View(foundEvent);
        }

        // POST: Admin/Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return NotFound();
            }

            if (updatedEvent.EndDateTime < updatedEvent.StartDateTime)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.EventView.Events.EventTimeDifferenceError);
            }

            if (updatedEvent.ShowEndDateTime <= updatedEvent.ShowStartDateTime)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.EventView.Events.EventShowTimeDifferenceError);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    updatedEvent.ChangedAt = DateTime.Now;
                    updatedEvent.ChangedBy = _userManager.GetUserId(User);
                    _bll.Events.Update(updatedEvent);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(updatedEvent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "ScheduleAndEvents");
            }
            return View(updatedEvent);
        }

        private bool EventExists(int id)
        {
            return _bll.Events.Find(id) != null;
        }
    }
}
