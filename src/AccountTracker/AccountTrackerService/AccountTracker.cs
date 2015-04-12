using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccountTrackerService
{
    public delegate void NegotiationdHandler(object sender, NegotiationdEventArgs data);

    public class NegotiationdEventArgs
    {
        public DateTime CreateDate { get; set; }
        public int EventId { get; set; }
        public string EventMessage { get; set; }
        public string IpAddress { get; set; }
    }

    public class AccountTracker
    {
        int eventID;
        EventLogQuery query;
        EventLogWatcher watcher;
        public event NegotiationdHandler Negotiated;

        public AccountTracker(int _eventID)
        {
            eventID = _eventID;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                query = new EventLogQuery("Security", PathType.LogName, string.Format("<QueryList>\r\n                  <Query Id=\"0\" Path=\"Security\">\r\n                    <Select Path=\"Security\">\r\n                        *[System[(EventID={0}) and\r\n                        TimeCreated[timediff(@SystemTime) &lt;= 86400000]]]\r\n                    </Select>\r\n                  </Query>\r\n                </QueryList>", eventID));
                watcher = new EventLogWatcher(query);
                watcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
                watcher.Enabled = true;
                IsRunning = true;
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                watcher.Enabled = false;
                watcher = null;
                query = null;
                IsRunning = false;
            }
        }
        
        private void OnEventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            try
            {
                string[] propertyQueries = new string[] { "Event/EventData/Data[@Name=\"IpAddress\"]" };
                EventLogPropertySelector propertySelector = new EventLogPropertySelector(propertyQueries);
                string str = ((EventLogRecord)e.EventRecord).GetPropertyValues(propertySelector)[0].ToString();
                NegotiationdEventArgs data = new NegotiationdEventArgs
                {
                    CreateDate = e.EventRecord.TimeCreated.Value,
                    EventId = e.EventRecord.Id,
                    IpAddress = str
                };
                if (Negotiated != null)
                    Negotiated(this, data);
            }
            catch (Exception exception)
            {
                WriteEntry(exception.Message);
            }
        }
       
        internal void WriteEntry(string msg)
        {
            EventLog.WriteEntry("VikasRana.AccountTracker", msg);
        }

        internal bool IsRunning { get; set; }

        private const string v4 = "(?:[0-9]{1,3}.){3}[0-9]{1,3}";
        private const string v6 = @"^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:)))(%.+)?s*";
        internal bool IsValidIP(string address, out string ip)
        {
            ip = string.Empty;
            if (Regex.IsMatch(address, v4))
            {
                ip = Regex.Match(address, v4).Value;
                return true;
            }
            if (Regex.IsMatch(address, v6))
            {
                ip = Regex.Match(address, v6).Value;
                return true;
            }
            return false;
        }
    }
}
