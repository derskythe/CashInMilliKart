﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;


namespace CashInService
{
    [RunInstaller(true)]
    public partial class CashInServiceInstaller : Installer
    {
        public CashInServiceInstaller()
        {
            InitializeComponent();
        }

        void ServiceInstallerAfterInstall(object sender, InstallEventArgs e)
        {
            string myAssembly = Path.GetFullPath(this.Context.Parameters["assemblypath"]);

            if (!String.IsNullOrEmpty(myAssembly))
            {
                var assemblyDir = Path.GetDirectoryName(myAssembly);
                if (!String.IsNullOrEmpty(assemblyDir))
                {
                    string logPath = Path.Combine(assemblyDir, "log");

                    Directory.CreateDirectory(logPath);
                    ReplacePermissions(logPath, WellKnownSidType.NetworkServiceSid, FileSystemRights.FullControl);
                }
            }
        }

        static void ReplacePermissions(string filepath, WellKnownSidType sidType, FileSystemRights allow)
        {
            FileSecurity sec = File.GetAccessControl(filepath);
            var sid = new SecurityIdentifier(sidType, null);
            sec.PurgeAccessRules(sid); //remove existing
            sec.AddAccessRule(new FileSystemAccessRule(sid, allow, AccessControlType.Allow));
            File.SetAccessControl(filepath, sec);
        }
    }
}
