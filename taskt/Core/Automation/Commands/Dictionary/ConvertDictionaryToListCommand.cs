﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert Dictionary To List")]
    [Attributes.ClassAttributes.Description("This command allows you to get List from Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get List from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDictionaryToListCommand : ADictionaryGetFromDictionaryCommands, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        public ConvertDictionaryToListCommand()
        {
            //this.CommandName = "ConvertDictionaryToListCommand";
            //this.SelectionName = "Convert Dictionary To List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var dic = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            var dic = this.ExpandUserVariableAsDictionary(engine);

            //dic.Values.ToList().StoreInUserVariable(engine, v_Result);
            this.StoreListInUserVariable(dic.Values.ToList(), engine);
        }
    }
}