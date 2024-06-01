﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.JSON
{
    /// <summary>
    /// for Input JSON commands
    /// </summary>
    public abstract class AJSONInputJSONCommands : ScriptCommand, ILJSONInputJSONProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        [PropertyParameterOrder(5000)]
        public string v_Json { get; set; }
    }
}
