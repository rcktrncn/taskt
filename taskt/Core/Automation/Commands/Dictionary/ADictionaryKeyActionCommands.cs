using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class ADictionaryKeyActionCommands : ADictionaryKeyCommands, IDictionaryKeyActionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        public string v_WhenKeyDoesNotExists { get; set; }
    }
}
