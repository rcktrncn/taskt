using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    /// <summary>
    /// notification manager for frmScriptBuilder
    /// </summary>
    internal class NotificationManager4frmScriptBuilder
    {
        /// <summary>
        /// notification text
        /// </summary>
        public string NotificationText { get; private set; }

        /// <summary>
        /// expire(hide) notification datetime
        /// </summary>
        public DateTime ExpireNotifaciton { get; private set; }

        /// <summary>
        /// notifacation panel visible state
        /// </summary>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// parent control of Notification panel
        /// </summary>
        private RowStyle parentControl;
        /// <summary>
        /// notification panel
        /// </summary>
        private Panel myPanel;

        /// <summary>
        /// parent contrl height
        /// </summary>
        const int parentHeight = 30;

        /// <summary>
        /// expire time of Notification
        /// </summary>
        private int expireSeconds;

        /// <summary>
        /// messages
        /// </summary>
        private List<string> notificationMessages = new List<string>();

        public NotificationManager4frmScriptBuilder(RowStyle parent, Panel panel, int expireSeconds)
        {
            parentControl = parent;
            myPanel = panel;
            this.expireSeconds = expireSeconds;
        }

        /// <summary>
        /// hide notification controls
        /// </summary>
        public void HideNotification()
        {
            parentControl.Height = 0;
            IsVisible = false;
        }

        /// <summary>
        /// show notification controls
        /// </summary>
        public void ShowNotification()
        {
            parentControl.Height = parentHeight;
            IsVisible = true;
        }

        /// <summary>
        /// show notification message
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            this.NotificationText = message;
            this.notificationMessages.Add(message);

            if (!IsVisible)
            {
                ShowNotification();
            }

            this.ExpireNotifaciton = DateTime.Now.AddSeconds(expireSeconds);

            var g = myPanel.CreateGraphics();

            myPanel.SuspendLayout();
            g.Clear(myPanel.BackColor);
            g.DrawString(NotificationText, myPanel.Font, Brushes.White, 30, 4);
            g.DrawImage(Properties.Resources.message, 5, 3, 24, 24);
            myPanel.ResumeLayout();
        }
    }
}
