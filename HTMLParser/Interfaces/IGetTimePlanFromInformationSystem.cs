using System;
using System.Collections.Generic;
using Domain;

namespace HTMLParser.Interfaces
{
    public interface IGetTimePlanFromInformationSystem
    {
        List<Schedule> GetScheduleForPeriod(DateTime timetableStarts, DateTime timetableEnds);
        Schedule GetScheduleForToday();
    }
}
