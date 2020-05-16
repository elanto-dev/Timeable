using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Enums;
using HTMLParser.DTO;

namespace HTMLParser.Mappers
{
    public class ScheduleMapper
    {
        /// <summary>
        /// Object mapping from HTMLParser.DTO to Domain.Schedule object.
        /// </summary>
        /// <param name="timePlanEvents">HTMLParser.DTO entity</param>
        /// <param name="forDate">Schedule date</param>
        /// <returns>Domain.Schedule entity</returns>
        public static Schedule MapFromDto(List<TimePlanEvent> timePlanEvents, DateTime forDate)
        {
            var schedule = new Schedule
            {
                Date = forDate
            };

            var subjectsInSchedule = new List<SubjectInSchedule>();

            foreach (var tpEvent in timePlanEvents)
            {
                var newSubject = new Subject
                {
                    SubjectCode = tpEvent.SubjectCode,
                    SubjectName = tpEvent.SubjectName
                };

                var teacherInSubject = tpEvent.LecturersWithRoles.Select(lecturer =>
                    new TeacherInSubjectEvent
                    {
                        Teacher = new Teacher
                        {
                            FullName = string.Join(" ", lecturer.Trim().Split(" ").Where(e => e.ToCharArray().Any(char.IsUpper)).ToList()).Trim(),
                            Role = string.Join(" ", lecturer.Trim().Split(" ").Where(e => e.ToCharArray().All(char.IsLower)).ToList()).Trim()
                        }
                    }).ToList();

                var subjectInSchedule = new SubjectInSchedule()
                {
                    StartDateTime = tpEvent.StartDateTime,
                    EndDateTime = tpEvent.EndDateTime,
                    Groups = string.Join(", ", tpEvent.Groups),
                    Rooms = string.Join(", ", tpEvent.Locations),
                    Schedule = schedule,
                    Subject = newSubject,
                    TeacherInSubjectEvents = teacherInSubject,
                    UniqueIdentifier = tpEvent.EventIdentifier
                };

                subjectInSchedule.SubjectType = tpEvent.SubjectEventType.Trim() switch
                {
                    "praktikum" => (int) SubjectType.Practice,
                    "harjutus" => (int) SubjectType.Exercise,
                    "loeng" => (int) SubjectType.Lecture,
                    "loeng+harjutus" => (int) SubjectType.LectureAndExercise,
                    "loeng+praktikum" => (int) SubjectType.LectureAndPractice,
                    "praktikum+harjutus" =>(int) SubjectType.PracticeAndExercise,
                    "harjutus+praktikum" => (int) SubjectType.ExerciseAndPractice,
                    _ => (int) SubjectType.Unknown
                };

                subjectsInSchedule.Add(subjectInSchedule);
            }

            schedule.SubjectsInSchedules = subjectsInSchedule;

            return schedule;
        }
    }
}
