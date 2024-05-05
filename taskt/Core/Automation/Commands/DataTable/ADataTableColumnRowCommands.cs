using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.DataTable;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for DataTable Column Row specified commands
    /// </summary>
    public abstract class ADataTableColumnRowCommands : ADataTableInputDataTableCommands, IDataTableColumnRowPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        [PropertyParameterOrder(7000)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        [PropertyParameterOrder(8000)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyParameterOrder(9000)]
        public string v_RowIndex { get; set; }
    }
}
