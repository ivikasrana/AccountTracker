namespace AccountTrackerService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AccountTrackerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.AccountTrackerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // AccountTrackerServiceProcessInstaller
            // 
            this.AccountTrackerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.AccountTrackerServiceProcessInstaller.Password = null;
            this.AccountTrackerServiceProcessInstaller.Username = null;
            // 
            // AccountTrackerServiceInstaller
            // 
            this.AccountTrackerServiceInstaller.ServiceName = "AccountTrackerService";
            this.AccountTrackerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.AccountTrackerServiceProcessInstaller,
            this.AccountTrackerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller AccountTrackerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller AccountTrackerServiceInstaller;
    }
}