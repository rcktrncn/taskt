using System;
using System.Windows.Forms;

/*
 * This form is called primarily by frmScriptBuilder, so the namespace looks like this
 */
namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmAddVariable : ThemedForm
    {
        //private Core.ApplicationSettings appSettings;
        private Core.SafeApplicationSettings appSettings;

        public frmAddVariablesEditMode editMode { get; }
        
        public string VariableName { get; private set; }
        
        public string VariableValue { get; private set; }

        public enum frmAddVariablesEditMode
        {
            Add,
            Edit
        }

        public frmAddVariable()
        {
            InitializeComponent();
            this.editMode = frmAddVariablesEditMode.Add;
            this.appSettings = App.Taskt_Settings;
        }

        public frmAddVariable(string VariableName, string variableValue)
        {
            InitializeComponent();
            this.Text = "edit variable";
            lblHeader.Text = "edit variable";
            txtVariableName.Text = VariableName;
            txtDefaultValue.Text = variableValue;
            this.editMode = frmAddVariablesEditMode.Edit;
            this.appSettings = App.Taskt_Settings;
        }

        private void frmAddVariable_Load(object sender, EventArgs e)
        {
            lblDefineNameDescription.Text = Core.Automation.Commands.InternalKeywordsControls.ReplaceKeywordsToSystemVariableAndInstanceName(lblDefineNameDescription.Tag.ToString(), appSettings);
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (txtVariableName.Text.Trim() == string.Empty)
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.VariableName = txtVariableName.Text;
            this.VariableValue = txtDefaultValue.Text;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAddVariable_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
