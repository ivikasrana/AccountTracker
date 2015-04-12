using ServerCloak;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountTracker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var serviceStatus = ServiceInstaller.GetServiceStatus(ServiceName);
            if (serviceStatus == ServiceState.NotFound || serviceStatus == ServiceState.Stop)
                ToggleService();
            else if (serviceStatus == ServiceState.Run || serviceStatus == ServiceState.Starting)
                btnInstall.Text = "Stop Service";
        }

        const string ServiceName = "AccountTrackerService";
        private void btnInstall_Click(object sender, EventArgs e)
        {
            ToggleService();
        }

        void ToggleService()
        {
            if (Convert.ToString(btnInstall.Text).Contains("Start"))
            {
                if (!ServiceInstaller.ServiceIsInstalled(ServiceName))
                {
                    ServiceInstaller.InstallAndStart(ServiceName, ServiceName, string.Format("{0}{1}{2}", Application.StartupPath, "\\", ServiceName));
                    MessageBox.Show("Service Installed");
                }
                else
                    ServiceInstaller.StartService(ServiceName);
                btnInstall.Text = "Stop Service";
            }
            else
            {
                ServiceInstaller.StopService(ServiceName);
                btnInstall.Text = "Start Service";
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            ServiceInstaller.StopService(ServiceName);
            ServiceInstaller.Uninstall(ServiceName);
            MessageBox.Show("Uninstall Completed");
        }
    }
}
