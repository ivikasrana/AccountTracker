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

        const string ServiceName = "AccountTrackerService";
        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (!ServiceInstaller.ServiceIsInstalled(ServiceName))
            {
                ServiceInstaller.InstallAndStart(ServiceName, ServiceName, string.Format("{0}{1}{2}", Application.StartupPath, "\\", ServiceName));
                MessageBox.Show("AccountTracker Service Installed");
            }
            else
                ServiceInstaller.StartService(ServiceName);
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            ServiceInstaller.StopService(ServiceName);
            ServiceInstaller.Uninstall(ServiceName);
            MessageBox.Show("Uninstall Completed");
        }
    }
}
