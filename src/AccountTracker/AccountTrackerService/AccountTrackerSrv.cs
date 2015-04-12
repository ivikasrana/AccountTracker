using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AccountTrackerService
{
    public partial class AccountTrackerService : ServiceBase
    {
        public AccountTrackerService()
        {
            InitializeComponent();
        }

        List<AccountTracker> events = new List<AccountTracker>()
        {
            new AccountTracker(4624),
            new AccountTracker(4625),
            new AccountTracker(4634)
        };

        protected override void OnStart(string[] args)
        {
            for (int i = 0; i < events.Count; i++)
            {
                var activity = events[i];
                activity.Negotiated += activity_Negotiated;
                activity.Start();
            }
        }

        void activity_Negotiated(object sender, NegotiationdEventArgs data)
        {
            try
            {
                if (Command.Connection.State != ConnectionState.Open)
                    Command.Connection.Open();
                Command.Parameters.Clear();
                Command.Parameters.AddWithValue("@IP", data.IpAddress);
                Command.Parameters.AddWithValue("@EventID", data.EventId);
                Command.Parameters.AddWithValue("@EventName", data.EventName);
                Command.Parameters.AddWithValue("@EventMessage", data.EventMessageXml);
                Command.Parameters.AddWithValue("@Created", data.CreateDate);
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("AccountTrackerError", ex.ToString());
            }
        }

        SqlCommand _Command;
        SqlCommand Command
        {
            get
            {
                if (_Command == null)
                {
                    _Command = new SqlCommand("INSERT SecurityLog (IP, EventID, EventName, EventMessage, Created) VALUES(@IP, @EventID, @EventName, @EventMessage, @Created)");
                    _Command.Connection = new SqlConnection("Data Source=(local);Initial Catalog=ServerCloak;User ID=sa;Password=@;Connect Timeout=120;");
                }
                return _Command;
            }
        }

        protected override void OnStop()
        {
            for (int i = 0; i < events.Count; i++)
                events[i].Stop();
            Command.Connection.Close();
        }
    }
}
