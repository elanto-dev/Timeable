using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLParser.Services
{
    public class GetWeekNumber
    {
        public static async Task<int> GetCurrentWeekNumberAsync()
        {
            var httpClient = new HttpClient();
            var regex = new Regex("jooksev.*:bold\\\">([1-2][1-9]|[1-9])");

            var dataString = await httpClient.GetStringAsync(
                "https://ois.ttu.ee/portal/page?_pageid=37,675060&_dad=portal&_schema=PORTAL&from_menu=1&_xid=55911802910352613330516068114841537"
                );

            var regexMatch = regex.Match(dataString).Groups[1].Value;

            httpClient.Dispose();

            if (int.TryParse(regexMatch, out int result))
            {
                return result;
            }

            return 0;
        }
    }
}
