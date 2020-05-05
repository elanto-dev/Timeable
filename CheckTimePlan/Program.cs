using System;
using System.Globalization;
using System.Linq;
using HTMLParser;
using HTMLParser.Services;

namespace CheckTimePlan
{
    class Program
    {
        static void Main(string[] args)
        {
            var timePlanSource = new GetTimePlanFromInformationSystem( "ICO");

            var date = DateTime.Parse("2020-03-26");

            var list = timePlanSource.GetScheduleForPeriod(DateTime.Parse("2020-03-23"), DateTime.Parse("2020-03-23"));

            foreach (var subj in list[0].SubjectsInSchedules)
            {
                Console.WriteLine(subj.StartDateTime.ToString(CultureInfo.InvariantCulture) + " - " + subj.EndDateTime + 
                                  " " + subj.Subject.SubjectCode +" " + subj.Subject.SubjectName + ", " 
                                  + subj.TeacherInSubjectEvents.ToList()[0].Teacher.FullName + " role: " + subj.TeacherInSubjectEvents.ToList()[0].Teacher.Role);
            }

            Console.WriteLine(GetWeekNumber.GetCurrentWeekNumberAsync().Result);
            
        }
    }
}
