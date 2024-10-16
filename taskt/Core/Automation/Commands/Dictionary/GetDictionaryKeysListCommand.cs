﻿using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.CommandSettings("Get Dictionary Keys List")]
    [Attributes.ClassAttributes.Description("This command allows you to get Keys List in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Keys List in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetDictionaryKeysListCommand : ADictionaryGetFromDictionaryCommands, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        public GetDictionaryKeysListCommand()
        {
            //this.CommandName = "GetDictionaryKeysListCommand";
            //this.SelectionName = "Get Dictionary Keys List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var dic = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            var dic = this.ExpandUserVariableAsDictionary(engine);

            //dic.Keys.ToList().StoreInUserVariable(engine, v_Result);
            this.StoreListInUserVariable(dic.Keys.ToList(), engine);
        }
    }
}