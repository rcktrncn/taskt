using System.Windows.Forms;

namespace taskt.UI.Forms
{
    public partial class DialogLikeThemedForm : ThemedForm
    {
        public DialogLikeThemedForm()
        {
            InitializeComponent();
        }

        private void DialogLikeThemedForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
