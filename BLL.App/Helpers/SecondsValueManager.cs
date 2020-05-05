using System.Collections.Generic;
using System.Linq;

namespace BLL.App.Helpers
{
    public static class SecondsValueManager
    {
        private static readonly List<string> SpecialValuesForScheduleAndPromotion = new List<string> { "Always", "Never" };

        private static readonly Dictionary<string, int?> SecondsValuesDictionary = new Dictionary<string, int?>
        {
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
            { "4 minutes", 240},
            { "5 minutes", 300},
            { "6 minutes", 360},
            { "7 minutes", 420},
            { "8 minutes", 480},
            { "9 minutes", 540},
            { "10 minutes", 600}
        };

        public static List<string> GetDictionaryKeysList(bool schedule)
        {
            var res = new List<string>
            {
                schedule ? SpecialValuesForScheduleAndPromotion[0] : SpecialValuesForScheduleAndPromotion[1]
            };
            res.AddRange(SecondsValuesDictionary.Keys.ToList());
            return res;
        }

        public static string GetSelectedValue(int? intValue, bool schedule)
        {
            if (intValue == null)
                return schedule ? SpecialValuesForScheduleAndPromotion[0] : SpecialValuesForScheduleAndPromotion[1];

            return SecondsValuesDictionary.FirstOrDefault(x => x.Value.Equals(intValue)).Key;
        }

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
