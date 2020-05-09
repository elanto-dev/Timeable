using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class EventMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Event))
            {
                return MapFromInternal((DalDto.Event)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(DalDto.Event))
            {
                return MapFromExternal((BllDto.Event)inObject) as TOutObject ?? default!;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Event MapFromInternal(DalDto.Event currentEvent)
        {
            var res = currentEvent == null ? null : new BllDto.Event
            {
                Id = currentEvent.Id,
                CreatedAt = currentEvent.CreatedAt,
                ChangedAt = currentEvent.ChangedAt,
                CreatedBy = currentEvent.CreatedBy,
                ChangedBy = currentEvent.ChangedBy,
                Name = currentEvent.Name,
                Place = currentEvent.Place,
                StartDateTime =  currentEvent.StartDateTime,
                EndDateTime = currentEvent.EndDateTime,
                ShowStartDateTime = currentEvent.ShowStartDateTime,
                ShowEndDateTime = currentEvent.ShowEndDateTime
            };

            return res ?? default!;
        }

        public static DalDto.Event MapFromExternal(BllDto.Event currentEvent)
        {
            var res = currentEvent == null ? null : new DalDto.Event
            {
                Id = currentEvent.Id,
                CreatedAt = currentEvent.CreatedAt,
                ChangedAt = currentEvent.ChangedAt,
                CreatedBy = currentEvent.CreatedBy,
                ChangedBy = currentEvent.ChangedBy,
                Name = currentEvent.Name,
                Place = currentEvent.Place,
                StartDateTime = currentEvent.StartDateTime,
                EndDateTime = currentEvent.EndDateTime,
                ShowStartDateTime = currentEvent.ShowStartDateTime,
                ShowEndDateTime = currentEvent.ShowEndDateTime
            };
            return res ?? default!;
        }
    }
}
