using System;
using System.Diagnostics;
using ArpmarCore.Domain;

namespace ArpmarService.Services
{
    public class LogService : IDisposable
    {
        private readonly EventLog _eventLog;

        public LogService()
        {
            InitEventSource();
            _eventLog = new EventLog(Service.EventLogName) { Source = Service.EventSourceName };
        }

        private static void InitEventSource()
        {
            try
            {
                if (EventLog.SourceExists(Service.EventSourceName))
                    return;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            EventLog.CreateEventSource(Service.EventSourceName, Service.EventLogName);
        }

        public void WriteInfo(string text) 
            => _eventLog.WriteEntry(text, EventLogEntryType.Information);

        public void WriteWarning(string text)
            => _eventLog.WriteEntry(text, EventLogEntryType.Warning);

        public void Dispose()
        {
            _eventLog?.Close();
            _eventLog?.Dispose();
        }
    }
}