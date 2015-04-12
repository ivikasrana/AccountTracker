using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountTracker
{
    static class Program
    {
        public static bool IsAdmin
        {
            get
            {
                try
                {
                    WindowsIdentity user = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(user);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch { }
                return false;
            }
        }

        [STAThread]
        static void Main()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Access Denied as you have to run this utility as Administrator.");
                Application.Exit();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
