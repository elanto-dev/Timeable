using System.Collections.Generic;
using Domain.Enums;

namespace BLL.App.Helpers
{
    public class LectureTypeManager
    {
        private static readonly Dictionary<SubjectType, string> LectureTypeDictionary = new Dictionary<SubjectType, string>
        {
            {SubjectType.Unknown, ""},
            {SubjectType.Lecture, "L"},
            {SubjectType.Exercise, "H"},
            {SubjectType.Practice, "P"},
            {SubjectType.LectureAndPractice, "L+P"},
            {SubjectType.LectureAndExercise, "L+H"},
            {SubjectType.PracticeAndExercise, "P+H"},
            {SubjectType.ExerciseAndPractice, "H+P"},
        };

        public static string GetLectureTypeShortString(int subjectTypeInt)
        {
            return LectureTypeDictionary[(SubjectType) subjectTypeInt];
        }
    }
}
