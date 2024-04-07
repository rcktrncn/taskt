using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Add Dictionary Items or Create Dictionary commands
    /// </summary>
    public abstract class ADictionaryAddCreateCommands : AInputDictionaryCommands, IDictionaryAddCreateProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_KeyAndValue))]
        [PropertyParameterOrder(6000)]
        public DataTable v_ColumnNameDataTable { get; set; }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        }
    }
}
