using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using HTMLParser.Interfaces;
using HTMLParser.Mappers;
using HTMLParser.Services;

namespace HTMLParser
{
    public class GetTimePlanFromInformationSystem : IGetTimePlanFromInformationSystem
    {
        private readonly string _housePrefix;

        public GetTimePlanFromInformationSystem(string housePrefix) 
        {
            _housePrefix = housePrefix;
        }

        public List<Schedule> GetScheduleForPeriod(DateTime timetableStarts, DateTime timetableEnds)
        {
            var schedulesList = new List<Schedule>();

            var getTimePlanEvents =
                new TimePlanEventsService(_housePrefix, timetableStarts, timetableEnds);

            var timeTable = getTimePlanEvents.GetFullTimePlan().Result;

            for (int i = 0; i <= (timetableEnds - timetableStarts).TotalDays; i++)
            {
                schedulesList.Add(ScheduleMapper.MapFromDto(
                    timeTable.Where(e => e.StartDateTime >= timetableStarts.AddDays(i) && e.EndDateTime < timetableStarts.AddDays(i + 1)).ToList(),
                    timetableStarts.AddDays(i))
                );
            }

            getTimePlanEvents.Dispose();

            return schedulesList;
        }

        public Schedule GetScheduleForToday()
        {
            var getTimePlanEvents =
                new TimePlanEventsService(_housePrefix, DateTime.Today, DateTime.Today.AddDays(1));
            var timeTable = getTimePlanEvents.GetFullTimePlan().Result;

            getTimePlanEvents.Dispose();

            var schedule = ScheduleMapper.MapFromDto(timeTable.ToList(), DateTime.Today);

            schedule.WeekNumber = GetWeekNumber.GetCurrentWeekNumberAsync().Result;

            return schedule;
        }
    }
}
