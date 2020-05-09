using System;

namespace BLL.App.Mappers
{
    public class EventInScheduleToSettingsMapper
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.EventForSettings))
            {
                return MapFromInternal((DAL.DTO.Event)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.EventForSettings MapFromInternal(DAL.DTO.Event eventToMap)
        {
            var res = eventToMap == null ? null : new DTO.EventForSettings
            {
                Id = eventToMap.Id,
                Name = eventToMap.Name,
                Place = eventToMap.Place,
                StartDateTime = eventToMap.StartDateTime.ToString(),
                EndDateTime = eventToMap.EndDateTime.ToString(),
                ShowStartDateTime = eventToMap.ShowStartDateTime.ToString(),
                ShowEndDateTime = eventToMap.ShowEndDateTime.ToString(),
            };

            return res ?? default!;
        }
    }
}
