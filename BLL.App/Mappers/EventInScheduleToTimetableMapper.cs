using System;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class EventInScheduleToTimetableMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.EventForTimetable))
            {
                return MapFromInternal((DAL.DTO.EventInSchedule)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.EventForTimetable MapFromInternal(DAL.DTO.EventInSchedule eventInSchedule)
        {
            var res = eventInSchedule == null ? null : new DTO.EventForTimetable
            {
                Id = eventInSchedule.EventId,
                EventName = eventInSchedule.Event.Name,
                Place = eventInSchedule.Event.Place
            };

            if (eventInSchedule != null)
            {
                if (eventInSchedule.Event.StartDateTime.Date.Equals(eventInSchedule.Event.EndDateTime.Date))
                {
                    res!.HappeningDateTime = eventInSchedule.Event.StartDateTime.Date.ToString("dd.MM.yyyy") + " "
                                                                                  + eventInSchedule.Event.StartDateTime
                                                                                      .ToString("HH:mm") + " - "
                                                                                  + eventInSchedule.Event.EndDateTime
                                                                                      .ToString("HH:mm");
                    return res;
                }

                res!.HappeningDateTime = eventInSchedule.Event.StartDateTime.Date.ToString("dd.MM.yyyy") + " "
                                                                              + eventInSchedule.Event.StartDateTime
                                                                                  .ToString("HH:mm") + " - " 
                                                                              + eventInSchedule.Event.EndDateTime.Date.ToString("dd.MM.yyyy") + " "
                                                                              + eventInSchedule.Event.EndDateTime
                                                                                  .ToString("HH:mm");
            }

            return res ?? default!;
        }
    }
}
