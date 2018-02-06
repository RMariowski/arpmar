using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using ArpmarCore.Domain;
using ArpmarService.Services;
using InterfacesAndArpTables = System.Collections.Concurrent.ConcurrentDictionary<string, ArpmarCore.Domain.ArpTable>;

namespace ArpmarService
{
    public class ArpmarService : ServiceBase
    {
        private readonly LogService _logService;
        private readonly MailService _mailService;

        private readonly Arp _arp = new Arp();
        private InterfacesAndArpTables _cachedTables;
        private Timer _serviceTimer;

        public int CheckForChangesInterval { get; set; }

        public ArpmarService(LogService logService, MailService mailService)
        {
            _logService = logService;
            _mailService = mailService;

            ServiceName = Service.Name;
        }

        protected override void OnStart(string[] args)
        {
            _cachedTables = new InterfacesAndArpTables(_arp.Tables);
            CreateServiceTimer();

            _serviceTimer.Start();

            _logService.WriteInfo("Service started.");
        }

        protected override void OnStop()
        {
            _serviceTimer.Stop();

            _logService.WriteInfo("Service stopped.");
        }

        private void CreateServiceTimer()
        {
            _serviceTimer = new Timer(CheckForChangesInterval) {AutoReset = true};
            _serviceTimer.Elapsed += (sender, args) =>
            {
                _arp.ExecuteDisplay();
                CheckForChanges();
            };
        }

        private void CheckForChanges()
        {
            var currentTables = _arp.Tables;

            var differences = new List<ArpEntryDiff>();

            foreach (var table in currentTables.Values)
            {
                var diffs = table.Entries.Except(_cachedTables[table.Interface]);
                foreach (var diffEntry in diffs)
                {
                    var added = true;
                    foreach (var entry in table.Entries)
                    {
                        if (diffEntry.Equals(entry))
                            continue;

                        if (!diffEntry.IpAddress.Equals(entry.IpAddress) &&
                            !diffEntry.MacAddress.Equals(entry.MacAddress))
                            continue;

                        differences.Add(new ArpEntryDiff(diffEntry, entry));
                        added = false;
                        break;
                    }

                    if (added)
                        differences.Add(new ArpEntryDiff(null, diffEntry));
                }
            }

            foreach (var table in _cachedTables.Values)
            {
                var diffs = table.Entries.Except(currentTables[table.Interface]);
                foreach (var diffEntry in diffs)
                {
                    var deleted = true;
                    foreach (var entry in table.Entries)
                    {
                        if (diffEntry.Equals(entry))
                            continue; 

                        if (!diffEntry.IpAddress.Equals(entry.IpAddress) &&
                            !diffEntry.MacAddress.Equals(entry.MacAddress))
                            continue;

                        differences.Add(new ArpEntryDiff(entry, diffEntry));
                        deleted = false;
                        break;
                    }

                    if (deleted)
                        differences.Add(new ArpEntryDiff(diffEntry, null));
                }
            }

            if (differences.Count <= 0)
                return;

            _cachedTables = new InterfacesAndArpTables(currentTables);

            var builder = new StringBuilder();
            foreach (var diff in differences)
                builder.Append($"{diff}\n");

            string changeList = builder.ToString();
            _logService.WriteInfo($"{differences.Count} changes detected.\nChange list:\n{changeList}");

            _mailService.SendMail(changeList);
        }
    }
}