using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.JSON
{
    /// <summary>
    /// for Get something from JSON commands
    /// </summary>
    public abstract class AJSONGetFromJSONCommands : AJSONInputJSONCommands, ILResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(10000)]
        public abstract string v_Result { get; set; }
    }
}
