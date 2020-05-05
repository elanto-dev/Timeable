using System.Collections.Generic;
using System.Linq;
using BLL.DTO;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class SubjectInScheduleCreateEditViewModel
    {
        public int? ScheduleId { get; set; }
        public Subject Subject { get; set; } = default!;
        public SubjectInSchedule SubjectInSchedule { get; set; } = default!;
        public SelectList SelectList { get; set; } = default!;
        public string SelectedSubjectType { get; set; } = default!;
        public List<Teacher> Teachers { get; set; } = default!;
    }

    internal class MapSubjectTypes
    {
        private static readonly Dictionary<string, SubjectType> LectureTypeDictionary = new Dictionary<string, SubjectType>
        {
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.Lecture, SubjectType.Lecture },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.Exercise, SubjectType.Exercise },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.Practice, SubjectType.Practice },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.LectureAndPractice, SubjectType.LectureAndPractice },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.LectureAndExercise, SubjectType.LectureAndExercise },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.PracticeAndExercise, SubjectType.PracticeAndExercise },
            {Resources.Views.SubjectInScheduleViewModel.SubjectTypes.ExerciseAndPractice, SubjectType.ExerciseAndPractice }
        };

        public static List<string> GetSubjectTypes()
        {
            return LectureTypeDictionary.Keys.ToList();
        }

        public static SubjectType GetResultSubjectTypeByString(string subjectTypeName)
        {
            return LectureTypeDictionary[subjectTypeName];
        }

        public static string GetPreviouslySelectedSubjectType(SubjectType subjectTypeInt)
        {
            return LectureTypeDictionary.FirstOrDefault(e => e.Value.Equals(subjectTypeInt)).Key;
        }
    }
}
