using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HTMLParser.Enums;

namespace HTMLParser.Services
{
    internal class TimePlanUrlSourcesService
    {
        private readonly HttpClient _httpClient;

        private Dictionary<StudyTypes, string> GroupLinkTemplate { get; set; } = new Dictionary<StudyTypes, string>
        {
            {
                StudyTypes.DayStudies,
                "https://ois.ttu.ee/pls/portal/tunniplaan.PRC_EXPORT_DATA?p_page=view_plaan&pn=i&pv=1&pn=e_sem&pv=261&pn=e&pv=-1&pn=b&pv=1&pn=g&pv=<GroupID>&pn=is_oppejoud&pv=false&pn=q&pv=1"
            },
            {
                StudyTypes.SessionStudies,
                "https://ois.ttu.ee/pls/portal/tunniplaan.PRC_EXPORT_DATA?p_page=view_plaan&pn=i&pv=1&pn=e_sem&pv=261&pn=e&pv=-1&pn=b&pv=2&pn=g&pv=<GroupID>&pn=is_oppejoud&pv=false&pn=q&pv=1"
            },
            {
                StudyTypes.DoctoralStudies,
                "https://ois.ttu.ee/pls/portal/tunniplaan.PRC_EXPORT_DATA?p_page=view_plaan&pn=i&pv=1&pn=e_sem&pv=261&pn=e&pv=-1&pn=b&pv=3&pn=g&pv=-1&pn=is_oppejoud&pv=false&pn=q&pv=neto"
            },
            {
                StudyTypes.FreeElectives,
                "https://ois.ttu.ee/pls/portal/tunniplaan.PRC_EXPORT_DATA?p_page=view_plaan&pn=i&pv=1&pn=e_sem&pv=261&pn=e&pv=-1&pn=b&pv=4&pn=g&pv=-1&pn=is_oppejoud&pv=false&pn=q&pv=neto"
            }
        };

        private Dictionary<StudyTypes, string> GroupLinksSources { get; set; } = new Dictionary<StudyTypes, string>
        {
            {
                StudyTypes.DayStudies,
                "https://ois.ttu.ee/portal/page?_pageid=37,675060&_dad=portal&_schema=PORTAL&e=-1&e_sem=261&a=1&b=1&c=-1&d=-1&i=2&k=&q=neto&g="
            },
            {
                StudyTypes.SessionStudies,
                "https://ois.ttu.ee/portal/page?_pageid=37,675060&_dad=portal&_schema=PORTAL&i=2&e=-1&e_sem=261&a=1&b=2&c=-1&d=-1&k=&q=neto&g="
            },
            {
                StudyTypes.IntroductoryWeek,
                "https://ois.ttu.ee/portal/page?_pageid=37,675060&_dad=portal&_schema=PORTAL&e=-1&e_sem=261&a=1&b=5&c=-1&d=-1&i=2&k=&q=neto&g="
            },

        };

        internal TimePlanUrlSourcesService(HttpClient httpClient)
        {
            _httpClient = httpClient ??
                          throw new ArgumentNullException(
                              $"{nameof(httpClient)} cannot be null. Cannot get URL for timetables.");
        }

        /// <summary>
        /// Returns dictionary of the URLs from where timetable can be found.
        /// </summary>
        /// <returns></returns>
        internal async Task<Dictionary<string, string>> GetTimeTablesUrlsDictionary()
        {
            Console.WriteLine($"GroupLinksSources[StudyTypes.DayStudies] {GroupLinksSources[StudyTypes.DayStudies]}");
            var dayStudiesDataString = await _httpClient.GetStringAsync(GroupLinksSources[StudyTypes.DayStudies]);
            Console.WriteLine($"dayStudiesDataString data {dayStudiesDataString.Length}");

            Console.WriteLine(
                $"GroupLinksSources[StudyTypes.SessionStudies] {GroupLinksSources[StudyTypes.SessionStudies]}");
            var sessionStudiesDataString =
                await _httpClient.GetStringAsync(GroupLinksSources[StudyTypes.SessionStudies]);
            Console.WriteLine($"sessionStudiesDataString data {sessionStudiesDataString.Length}");


            var regex = new Regex(
                "\\<span\\s*kava=\"[^\\>]*\\&g=(\\d*)[^\\>]*\\>\\<a[^\\>]*\\>([^\\<]*)\\<\\/a\\>\\s*\\<\\/span\\>");

            var result = regex.Matches(dayStudiesDataString).ToDictionary(
                k => k.Groups[2].Value,
                v => GroupLinkTemplate[StudyTypes.DayStudies].Replace("<GroupID>", v.Groups[1].Value));

            foreach (var groupUrl in regex.Matches(sessionStudiesDataString).ToDictionary(
                k => k.Groups[2].Value,
                v => GroupLinkTemplate[StudyTypes.SessionStudies].Replace("<GroupID>", v.Groups[1].Value)))
            {
                result.Add(groupUrl.Key, groupUrl.Value);
            }

            result.Add(StudyTypes.DoctoralStudies.ToString(), GroupLinkTemplate[StudyTypes.DoctoralStudies]);
            result.Add(StudyTypes.FreeElectives.ToString(), GroupLinkTemplate[StudyTypes.FreeElectives]);

            return result;
        }
    }
}