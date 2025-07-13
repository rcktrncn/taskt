using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder
{
    public partial class frmScriptBuilder
    {

        private void txtCommandSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                SearchForItemInListView();
            }
        }

        private void HideSearchInfo()
        {
            lblCurrentlyViewing.Hide();
            lblTotalResults.Hide();
        }

        private void SearchForItemInListView()
        {
            var searchCriteria = txtCommandSearch.Text;

            if (searchCriteria == "")
            {
                searchCriteria = tsSearchBox.Text;
            }

            if (searchCriteria.Length > 0)
            {
                if ((string)picCommandSearch.Tag != searchCriteria)
                {
                    picCommandSearch.Tag = searchCriteria;
                    AdvancedSearchItemInCommands(searchCriteria, false, false, false, false, true, false, "");
                }
                else
                {
                    MoveMostNearMatchedLine(true);
                }
            }
            else
            {
                ClearHighlightListViewItem();
            }
        }
    }
}
