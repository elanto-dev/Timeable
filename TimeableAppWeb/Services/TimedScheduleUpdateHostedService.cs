using System;
using System.Threading;
using System.Threading.Tasks;
using DAL.App;
using DAL.App.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TimeableAppWeb.Services
{
    public class TimedScheduleUpdateHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer? _timer;
        private DateTime _lastRunDate;
        private TimeSpan _time;

        public TimedScheduleUpdateHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _lastRunDate = DateTime.Today; // When application starts - table update is called by DataSeed from Startup.cs. No need with one more update!
            _time = new TimeSpan(03, 00, 00);
            _timer = new Timer(DownloadNewTimeplan, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        private void DownloadNewTimeplan(object state)
        {
            if (_lastRunDate == DateTime.Today)
                return;

            if (DateTime.Now.TimeOfDay < _time)
                return;

            _lastRunDate = DateTime.Today;
            using var scope = _scopeFactory.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            UpdateTimetable.DeleteScheduleRecordsOlderThan30Days(context);
            UpdateTimetable.UpdateScheduleForTimeplan(context);
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_timer == null)
                return;

            _timer.Dispose();
            _timer = null;
        }
    }
}
