using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class EventMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Event))
            {
                return MapFromDomain((Event) inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(Event))
            {
                return MapFromDal((DTO.Event) inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException(
                $"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Event MapFromDomain(Event currentEvent)
        {
            var res = currentEvent == null ? null : new DTO.Event
            {
                Id = currentEvent.Id,
                CreatedAt = currentEvent.CreatedAt,
                ChangedAt = currentEvent.ChangedAt,
                CreatedBy = currentEvent.CreatedBy,
                ChangedBy = currentEvent.ChangedBy,
                Name = currentEvent.Name,
                Place = currentEvent.Place,
                StartDateTime = currentEvent.StartTime,
                EndDateTime = currentEvent.EndDateTime,
                ShowStartDateTime = currentEvent.ShowStartDateTime,
                ShowEndDateTime = currentEvent.ShowEndDateTime
            };

            return res ?? default!;
        }

        public static Event MapFromDal(DTO.Event currentEvent)
        {
            var res = currentEvent == null ? null : new Event
            {
                Id = currentEvent.Id,
                CreatedAt = currentEvent.CreatedAt,
                ChangedAt = currentEvent.ChangedAt,
                CreatedBy = currentEvent.CreatedBy,
                ChangedBy = currentEvent.ChangedBy,
                Name = currentEvent.Name,
                Place = currentEvent.Place,
                StartTime = currentEvent.StartDateTime,
                EndDateTime = currentEvent.EndDateTime,
                ShowStartDateTime = currentEvent.ShowStartDateTime,
                ShowEndDateTime = currentEvent.ShowEndDateTime
            };

            return res ?? default!;
        }
    }
}
