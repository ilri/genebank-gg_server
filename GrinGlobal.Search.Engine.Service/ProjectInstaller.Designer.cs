﻿namespace GrinGlobal.Search.Engine.Service {
	partial class ProjectInstaller {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_BeforeUninstall);
            this.serviceProcessInstaller1.Committed += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_Committed);
            this.serviceProcessInstaller1.Committing += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_Committing);
            this.serviceProcessInstaller1.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_BeforeInstall);
            this.serviceProcessInstaller1.AfterUninstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterUninstall);
            this.serviceProcessInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "The search engine used by all GRIN-Global applications.";
            this.serviceInstaller1.DisplayName = "GRIN-Global Search Engine";
            this.serviceInstaller1.ServiceName = "ggse";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller1.BeforeUninstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_BeforeUninstall);
            this.serviceInstaller1.Committed += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_Committed);
            this.serviceInstaller1.Committing += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_Committing);
            this.serviceInstaller1.BeforeInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_BeforeInstall);
            this.serviceInstaller1.AfterUninstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterUninstall);
            this.serviceInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

		}

		#endregion

		private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
		private System.ServiceProcess.ServiceInstaller serviceInstaller1;
	}
}