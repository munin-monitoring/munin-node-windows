namespace Munin_Node_For_Windows.core
{
    abstract partial class ProjectInstaller
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
            this.ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.Installer = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProcessInstaller
            // 
            this.ProcessInstaller.Password = null;
            this.ProcessInstaller.Username = null;
            this.ProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ProcessInstaller_AfterInstall);
            // 
            // Installer
            // 
            this.Installer.ServiceName = "Munin Node for Windows";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {this.ProcessInstaller, this.Installer});
        }

        private System.ServiceProcess.ServiceInstaller Installer;
        private System.ServiceProcess.ServiceProcessInstaller ProcessInstaller;

        #endregion
    }
}