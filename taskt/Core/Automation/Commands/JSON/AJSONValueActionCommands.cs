﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.JSON
{
    /// <summary>
    /// for JSON Value Action commands
    /// </summary>
    public abstract class AJSONValueActionCommands : AJSONGetFromJContainerCommands, IJSONValueActionProperties
    {
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueToAdd))]
        [PropertyParameterOrder(10000)]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ValueType))]
        [PropertyParameterOrder(11000)]
        public string v_ValueType { get; set; }
    }
}
