using System;
using System.Text.RegularExpressions;
using BLL.App.Helpers;
using BLL.DTO;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    class SubjectInScheduleToSTimetableMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(SubjectInSchedule))
            {
                return MapFromInternal((DAL.DTO.SubjectInSchedule)inObject) as TOutObject ?? default!;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static SubjectForTimetableAndSettings MapFromInternal(DAL.DTO.SubjectInSchedule subjectInSchedule)
        {
            var res = subjectInSchedule == null ? null : new SubjectForTimetableAndSettings
            {
                SubjectInScheduleId = subjectInSchedule.Id,
                SubjectId = subjectInSchedule.SubjectId,
                StartTime = subjectInSchedule.StartDateTime,
                StartToEndString = subjectInSchedule.StartDateTime.ToString("HH:mm") + " - " + subjectInSchedule.EndDateTime.ToString("HH:mm"),
                LectureNameWithCode = subjectInSchedule.Subject.SubjectName + " (" + subjectInSchedule.Subject.SubjectCode + ")",
                LectureType = LectureTypeManager.GetLectureTypeShortString(subjectInSchedule.SubjectType),
            };

            if (res == null) return default!;

            // Rooms mapping is relevant for TalTech room mapping. Please change this for your application!
            var rooms = subjectInSchedule.Rooms.Split(",");

            for (var i = 0; i < rooms.Length; i++)
            {
                if (!rooms[i].Trim().StartsWith(subjectInSchedule.Schedule.Prefix)) continue;

                var roomWithoutPrefix = rooms[i].Trim().Substring(subjectInSchedule.Schedule.Prefix.Length);
                var regex = new Regex("[^a-zA-Z0-9]");
                // When prefix was chosen "ICO" - it deletes ICO and -, when prefix was chosen "IC" - it doesn't delete anything.
                if (!regex.IsMatch(roomWithoutPrefix.Substring(0, 1)))
                {
                    continue;
                }
                roomWithoutPrefix = roomWithoutPrefix.Substring(1);
                rooms[i] = roomWithoutPrefix;
            }

            res.Rooms = string.Join(", ", rooms);

            return res;
        }
    }
}
