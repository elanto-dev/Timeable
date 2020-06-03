using System.Collections.Generic;
using System.Linq;

namespace BLL.App.Helpers
{
    public static class SecondsValueManager
    {
        private static readonly List<string> SpecialValuesForScheduleAndPromotion = new List<string> { "Always", "Never" };

        private static readonly Dictionary<string, int?> SecondsValuesDictionary = new Dictionary<string, int?>
        {
            { "0 seconds", 0 },
            { "5 seconds", 5 },
            { "10 seconds", 10},
            { "15 seconds", 15},
            { "20 seconds", 20},
            { "25 seconds", 25},
            { "30 seconds", 30},
            { "40 seconds", 40},
            { "50 seconds", 50},
            { "60 seconds", 60},
            { "Minute and 15 seconds", 75},
            { "Minute and 30 seconds", 90},
            { "2 minutes", 120},
            { "2 minute and 30 seconds", 150},
            { "3 minutes", 180},
            { "3 minute and 30 seconds", 210},
            { "4 minutes", 240},
            { "4 minute and 30 seconds", 270},
            { "5 minutes", 300}
        };

        /// <summary>
        /// Get list of string values of seconds to show schedule or promotion.
        /// </summary>
        /// <param name="schedule">Boolean whether it is schedule/promotion</param>
        /// <returns>List of string seconds values</returns>
        public static List<string> GetDictionaryKeysList(bool schedule)
        {
            var res = new List<string>
            {
                schedule ? SpecialValuesForScheduleAndPromotion[0] : SpecialValuesForScheduleAndPromotion[1]
            };
            res.AddRange(SecondsValuesDictionary.Keys.ToList());
            return res;
        }

        /// <summary>
        /// Returns string equivalent of the seconds chosen.
        /// </summary>
        /// <param name="intValue">Seconds integer</param>
        /// <param name="schedule">Boolean whether it is schedule/promotion</param>
        /// <returns>String of second value</returns>
        public static string GetSelectedValue(int? intValue, bool schedule)
        {
            if (intValue == null)
                return schedule ? SpecialValuesForScheduleAndPromotion[0] : SpecialValuesForScheduleAndPromotion[1];

            return SecondsValuesDictionary.FirstOrDefault(x => x.Value.Equals(intValue)).Key;
        }

        /// <summary>
        /// Returns the integer seconds value by the string seconds value.
        /// </summary>
        /// <param name="stringValue">String of second value</param>
        /// <returns>Seconds integer</returns>
        public static int? GetIntValue(string? stringValue)
        {
            if (stringValue == null)
                return null;

            if (stringValue.Equals(SpecialValuesForScheduleAndPromotion[0]) ||
                stringValue.Equals(SpecialValuesForScheduleAndPromotion[1]))
                return null;

            return SecondsValuesDictionary[stringValue];
        }
    }
}
