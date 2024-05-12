﻿using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for List Index specified commands
    /// </summary>
    public abstract class AListIndexCommands : AListInputListCommands, IListIndexProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        public string v_Index { get; set; }
    }
}
