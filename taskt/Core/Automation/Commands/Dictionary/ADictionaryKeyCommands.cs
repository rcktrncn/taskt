using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class ADictionaryKeyCommands : AInputDictionaryCommands, IDictionaryKeyProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        [PropertyParameterOrder(6000)]
        public virtual string v_Key { get; set; }
    }
}
