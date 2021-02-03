using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HTMLParser.DTO;
using Microsoft.EntityFrameworkCore.Internal;

namespace HTMLParser.Services
{
    public class TimePlanEventsService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _prefix;
        private readonly DateTime _startDateTime;
        private readonly DateTime _endDateTime;

        /// <summary>
        /// </summary>
        /// <param name="prefix">Prefix of the location to search by</param>
        /// <param name="startDateTime">Start time of the search</param>
        /// <param name="endDateTime">End time of the search</param>
        public TimePlanEventsService(string prefix, DateTime startDateTime, DateTime endDateTime)
        {
            _httpClient = new HttpClient();
            _prefix = prefix;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
        }

        /// <summary>
        /// Returns the full timeplan for the chosen dates and prefix and for all the study groups.
        /// </summary>
        /// <returns>HTMLParser.DTO.TimePlanEvent entities</returns>
        internal async Task<IEnumerable<TimePlanEvent>> GetFullTimePlan()
        {
            var result = new List<TimePlanEvent>();

            var timePlanSourceService = new TimePlanUrlSourcesService(_httpClient);

            foreach (var url in (await timePlanSourceService.GetTimeTablesUrlsDictionary()).Values)
            {
                Console.WriteLine($"Downloading data from: {url}");

                var plan = await GetTimePlanFromUrl(url);
                result.AddRange(plan);
            }

            var res =
                result.GroupBy(x => x.EventIdentifier, (k, g) => g.First()).OrderBy(x => x.StartDateTime);
            return res;
        }

        /// <summary>
        /// Returns all the events for the chosen dates and prefix that are found using the given url.
        /// </summary>
        /// <param name="url">URL to search from</param>
        /// <returns>HTMLParser.DTO.TimePlanEvent entities</returns>
        private async Task<IEnumerable<TimePlanEvent>> GetTimePlanFromUrl(string url)
        {
            var dataStringTask = await _httpClient.GetStringAsync(url);
            Console.WriteLine($"Got data {dataStringTask.Length}");

            var regex = new Regex(
                "BEGIN\\:VEVENT.*DTSTART;([^\\n]*)\\n.*DTEND;([^\\n]*)\\n.*SUMMARY\\:([^\\n]*).*DESCRIPTION\\:([^\\n]*).*LOCATION\\:([^\\n]*)",
                RegexOptions.Singleline);
            var res = (dataStringTask)
                .Split("END:VEVENT")
                .Select(x => regex.Matches(x).FirstOrDefault())
                .Where(x => x != null)
                .Select(x =>
                {
                    var subject = x.Groups[3]
                        .Value
                        .Split("->")
                        .Select(s => s.Trim());
                    var subjectNames = subject.First().Split('-');
                    var subjectData = x.Groups[4].Value
                        .Split("\\n", StringSplitOptions.RemoveEmptyEntries);
                    var comment = subjectData.FirstOrDefault(d => d
                            .StartsWith("kommentaar:"))
                        ?.Replace("kommentaar:", "")
                        .Trim();
                    var locationsString = x.Groups[5].Value
                        .Replace("ruumid: ", "");
                    locationsString = locationsString
                        .Replace("ruumid:", "");

                    if (comment != null)
                    {
                        locationsString = locationsString
                            .Replace(subjectData.First(d => d.StartsWith("kommentaar:")), "");
                    }

                    var subjectCode = subjectNames[0];
                    var subjectNameComponents = subjectNames.ToList();
                    subjectNameComponents.RemoveAt(0);
                    var subjectName = subjectNameComponents.Join("-");

                    return new TimePlanEvent
                    {
                        StartDateTime = DateTime.ParseExact(x.Groups[1].Value.Split(':').Last().TrimEnd(),
                            "yyyyMMddTHHmmss", CultureInfo.InvariantCulture),
                        EndDateTime = DateTime.ParseExact(x.Groups[2].Value.Split(':').Last().TrimEnd(),
                            "yyyyMMddTHHmmss", CultureInfo.InvariantCulture),
                        SubjectEventType = subject.Last(),
                        SubjectCode = subjectCode,
                        SubjectName = subjectName,
                        Groups = subjectData.FirstOrDefault(d => d
                                .StartsWith("rühmad:"))?
                            .Replace("rühmad:", "")
                            .Split("\\,", StringSplitOptions.RemoveEmptyEntries)
                            .Select(e => e.Trim())
                            .ToList() ?? new List<string>(),
                        LecturersWithRoles = subjectData.FirstOrDefault(d => d
                                .StartsWith("õppejõud:"))?
                            .Replace("õppejõud:", "")
                            .Split("\\,", StringSplitOptions.RemoveEmptyEntries)
                            .Select(e => e.Trim())
                            .Distinct() // remove duplicate teachers - if the same teacher is marked as lecturer, assistant etc
                            .ToList() ?? new List<string>(),
                        Comment = comment ?? "",
                        Locations = locationsString
                            .Split("\\,", StringSplitOptions.RemoveEmptyEntries)
                            .Select(e => e.Trim())
                            .ToList()
                    };
                }).AsQueryable();
                res = res.Where(x =>
                    x.StartDateTime >= _startDateTime && x.StartDateTime < _endDateTime &&
                    x.Locations.Any(l => l.ToUpper().Contains(_prefix.ToUpper())));

            return res;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}