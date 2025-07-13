using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * tihs file has bottom Notify tray events and processes
         */

        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (appSettings == null)
            {
                return;
            }

            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                if ((notificationManager.ExpireNotifaciton < DateTime.Now) && (notificationManager.IsVisible))
                {
                    notificationManager.HideNotification();
                }

                //if ((notificationExpires < DateTime.Now) && (isDisplaying))
                //{
                //    HideNotification();
                //}

                if ((appSettings.ClientSettings.AntiIdleWhileOpen) && (DateTime.Now > lastAntiIdleEvent.AddMinutes(1)))
                {
                    PerformAntiIdle();
                }

                /*
                // check if notification is required
                if ((notificationList.Count > 0) && (notificationExpires < DateTime.Now))
                {
                    var itemToDisplay = notificationList[0];
                    notificationList.RemoveAt(0);
                    notificationExpires = DateTime.Now.AddSeconds(2);
                    ShowNotification(itemToDisplay);
                }
                */
            }
        }

        //private void pnlStatus_Paint(object sender, PaintEventArgs e)
        //{
        //    e.Graphics.Clear(pnlStatus.BackColor);
        //    e.Graphics.DrawString(notificationText, pnlStatus.Font, Brushes.White, 30, 4);
        //    e.Graphics.DrawImage(Properties.Resources.message, 5, 3, 24, 24);
        //}

        private void pnlStatus_DoubleClick(object sender, EventArgs e)
        {
            //using (var fm = new General.frmDialog(notificationText, "Status Message", General.frmDialog.DialogType.OkOnly, 0))
            using (var fm = new General.frmDialog(notificationManager.NotificationText, "Status Message", General.frmDialog.DialogType.OkOnly, 0))
            {
                fm.ShowDialog();
            }
            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyTray.Visible = false;
            }
        }

        /// <summary>
        /// show footer message
        /// </summary>
        /// <param name="notificationText"></param>
        public void Notify(string notificationText)
        {
            notificationManager.ShowMessage(notificationText);

            // DBG
            //MessageBox.Show(notificationText);

            /*
            if (appSettings.ClientSettings.HideNotifyAutomatically)
            {
                notificationList.Add(notificationText);
            }
            else
            {
                this.notificationText = notificationText;
                pnlStatus_Paint(pnlStatus, new PaintEventArgs(pnlStatus.CreateGraphics(), pnlStatus.Bounds));    // force draw
            }
            */

            //if (!isDisplaying)
            //{
            //    ShowNotificationRow();
            //}

            //this.notificationText = notificationText;
            //pnlStatus_Paint(pnlStatus, new PaintEventArgs(pnlStatus.CreateGraphics(), pnlStatus.Bounds));   // force draw
        }

        ///// <summary>
        ///// show notification controls
        ///// </summary>
        ///// <param name="textToDisplay"></param>
        //private void ShowNotification(string textToDisplay)
        //{
        //    notificationText = textToDisplay;

        //    pnlStatus.SuspendLayout();

        //    ShowNotificationRow();
        //    pnlStatus.ResumeLayout();

        //    isDisplaying = true;
        //}

        ///// <summary>
        ///// hide notification controls
        ///// </summary>
        //private void HideNotification()
        //{
        //    pnlStatus.SuspendLayout();
        //    HideNotificationRow();
        //    pnlStatus.ResumeLayout();

        //    isDisplaying = false;
        //}

        ///// <summary>
        ///// hide notification tray row
        ///// </summary>
        //private void HideNotificationRow()
        //{
        //    tlpControls.RowStyles[5].Height = 0;
        //}

        ///// <summary>
        ///// show notificaiton tray row
        ///// </summary>
        //private void ShowNotificationRow()
        //{
        //    tlpControls.RowStyles[5].Height = 30;
        //}
    }
}
