using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {
        /*
         * this file has frmScriptBuilder window title processes
         */

        /// <summary>
        /// update window title
        /// </summary>
        private void UpdateWindowTitle()
        {
            if (ScriptFilePath != null)
            {
                System.IO.FileInfo scriptFileInfo = new System.IO.FileInfo(ScriptFilePath);
                this.Text = "taskt - (" + scriptFileInfo.Name + ")";
            }
            else
            {
                this.Text = "taskt";
            }

            if (this.dontSaveFlag)
            {
                this.Text += " *";
            }
        }

        /// <summary>
        /// change save state and update window title
        /// </summary>
        /// <param name="dontSaveNow"></param>
        private void ChangeSaveState(bool dontSaveNow)
        {
            this.dontSaveFlag = dontSaveNow;
            UpdateWindowTitle();
            SetAutoSaveState();
        }
    }
}
