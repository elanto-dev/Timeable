using System;
using System.Collections.Generic;
using Domain;

namespace HTMLParser.Interfaces
{
    public interface IGetTimePlanFromInformationSystem
    {
        /// <summary>
        /// Returns schedules for the days between the dates (including).
        /// </summary>
        /// <param name="timetableStarts">From which date start searching</param>
        /// <param name="timetableEnds">To which date start searching (including)</param>
        /// <returns>Domain Schedule entities</returns>
        List<Schedule> GetScheduleForPeriod(DateTime timetableStarts, DateTime timetableEnds);

        /// <summary>
        /// Returns schedule entity for current day.
        /// </summary>
        /// <returns>Domain Schedule entity</returns>
        Schedule GetScheduleForToday();
    }
}
