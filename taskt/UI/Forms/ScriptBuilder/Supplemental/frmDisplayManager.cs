﻿using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmDisplayManager : UIForm
    {
        BindingList<MachineConfiguration> Machines = new BindingList<MachineConfiguration>();

        public frmDisplayManager()
        {
            InitializeComponent();
        }

        private void frmDisplayManager_Load(object sender, EventArgs e)
        {
            dgvMachines.DataSource = Machines;
        }

        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            var newMachine = new MachineConfiguration();
            newMachine.MachineName = "HostName";
            newMachine.UserName = "Administrator";
            newMachine.Password = "12345";
            newMachine.SupportCredSsp = "Yes";
            newMachine.NextConnectionDue = DateTime.Now;
            newMachine.LastKnownStatus = "Just Added";
            Machines.Add(newMachine);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Machines.Count == 0)
            {
                AddLogEvent("No machines were found!");
                return;
            }

            AddLogEvent("Enabling Remote Desktop Polling");
            dgvMachines.ReadOnly = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            tmrCheck.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tmrCheck.Enabled = false;
            AddLogEvent("Disabling Remote Desktop Polling");
            dgvMachines.ReadOnly = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void tmrCheck_Tick(object sender, EventArgs e)
        {
            dgvMachines.Refresh();

            foreach (var machine in Machines)
            {
                if ((string.IsNullOrEmpty(machine.MachineName)) || (string.IsNullOrEmpty(machine.UserName)) || (string.IsNullOrEmpty(machine.Password)))
                {
                    continue;
                }

                if (machine.NextConnectionDue <= DateTime.Now)
                {
                    //int windowWidth, windowHeight;

                    if (!int.TryParse(txtWidth.Text, out int windowWidth))
                    {
                        windowWidth = 1024;
                    }

                    if (!int.TryParse(txtHeight.Text, out int windowHeight))
                    {
                        windowHeight = 768;
                    }

                    string credSSp = machine.SupportCredSsp.Trim().ToLower();
                    switch (credSSp)
                    {
                        case "yes":
                            credSSp = "true";
                            break;  
                        case "no":
                            credSSp = "false";
                            break;
                    }
                    if (!bool.TryParse(credSSp, out bool supportCredSsp))
                    {
                        supportCredSsp = true;
                    }

                    AddLogEvent($"Machine '{machine.MachineName}' is due for desktop login");          
                    machine.LastKnownStatus = "Attempting to login";
                    machine.NextConnectionDue = DateTime.Now.AddMinutes(2);

                    AddLogEvent($"Next Connection for Machine '{machine.MachineName}' due at '{machine.NextConnectionDue}'");
                    
                    var viewer = new ScriptEngine.Supplemental.frmRemoteDesktopViewer(machine.MachineName, machine.UserName, machine.Password, supportCredSsp, windowWidth, windowHeight, chkHideScreen.Checked, chkStartMinimized.Checked);
                    viewer.LoginUpdateEvent += Viewer_LoginUpdateEvent;
                    viewer.Show();
                }
            }
        }

        private void Viewer_LoginUpdateEvent(object sender, ScriptEngine.Supplemental.LoginResultArgs e)
        {
            //var frmViewer = (Supplement_Forms.frmRemoteDesktopViewer)sender;
            var connResult = e.Result.ToString();
            AddLogEvent($"Machine '{e.MachineName}' login attempt was '{connResult}' {e.AdditionalDetail}");

            var machine = Machines.Where(f => f.MachineName == e.MachineName).FirstOrDefault();

            var status = $"Connection Result: '{connResult}'";
            if (!string.IsNullOrEmpty(e.AdditionalDetail))
            {
                status += $" ({e.AdditionalDetail})";
            }

            machine.LastKnownStatus = status;

            if (e.Result == ScriptEngine.Supplemental.LoginResultArgs.LoginResultCode.Failed)
            {
                var frmSender = (ScriptEngine.Supplemental.frmRemoteDesktopViewer)sender;
                frmSender.Close();
            }
        }

        private void AddLogEvent(string log)
        {
            lstEventLogs.Items.Add($"{DateTime.Now} - {log}");
            lstEventLogs.SelectedIndex = lstEventLogs.Items.Count - 1;
        }

        public class MachineConfiguration
        {
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string SupportCredSsp { get; set; }
            public DateTime NextConnectionDue { get; set; }
            public string LastKnownStatus { get; set; }
        }
    }
}

