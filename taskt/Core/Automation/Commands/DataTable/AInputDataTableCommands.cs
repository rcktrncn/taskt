using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Input DataTable commands
    /// </summary>
    public abstract class AInputDataTableCommands : ScriptCommand, ILDataTableProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyParameterOrder(5000)]
        public string v_DataTable { get; set; }
    }
}
