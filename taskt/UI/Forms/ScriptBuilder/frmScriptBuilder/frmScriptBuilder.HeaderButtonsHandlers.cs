using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has header menu buttons event handlers
         *
         */

        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            BeginNewScriptProcess();
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            BeginOpenScriptProcess();
        }

        private void uiBtnImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(false);
        }

        private void uiBtnSaveAs_Click(object sender, EventArgs e)
        {
            BeginSaveScriptProcess(true);
        }

        private void uiBtnAddVariable_Click(object sender, EventArgs e)
        {
            ShowVariableManager();
        }

        private void uiBtnSettings_Click(object sender, EventArgs e)
        {
            ShowSettingForm();
        }

        private void uiBtnClearAll_Click(object sender, EventArgs e)
        {
            HideSearchInfo();
            lstScriptActions.Items.Clear();
        }

        private void uiBtnRecordSequence_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var sequenceRecorder = new Supplemental.frmSequenceRecorder())
            {
                sequenceRecorder.callBackForm = this;
                sequenceRecorder.ShowDialog();
            }

            pnlCommandHelper.Hide();

            this.Show();
            this.BringToFront();
        }

        private void uiBtnScheduleManagement_Click(object sender, EventArgs e)
        {
            using (var scheduleManager = new Supplemental.frmScheduleManagement())
            {
                scheduleManager.Show();
            }
        }

        private void uiBtnRunScript_Click(object sender, EventArgs e)
        {
            BeginRunScriptProcess();
        }

        private void picCommandSearch_Click(object sender, EventArgs e)
        {
            SearchForItemInListView();
        }

        private void picCommandSearch_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void picCommandSearch_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void uiBtnKeep_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSequenceImport_Click(object sender, EventArgs e)
        {
            BeginImportProcess();
        }

        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            //if (lstScriptActions.Items.Count == 0)
            //    return;

            //reqdIndex = 0;

            //if (txtCommandSearch.Text == "")
            //{
            //    //hide info
            //    HideSearchInfo();

            //    //clear indexes
            //    matchingSearchIndex.Clear();
            //    currentIndexInMatchItems = -1;

            //    //repaint
            //    lstScriptActions.Invalidate();
            //}
            //else
            //{
            //    lblCurrentlyViewing.Show();
            //    lblTotalResults.Show();
            //    SearchForItemInListView();

            //    //repaint
            //    lstScriptActions.Invalidate();
            //}
        }
    }
}
