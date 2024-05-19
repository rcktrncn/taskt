﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Math")]
    [Attributes.ClassAttributes.CommandSettings("Get Sum From List")]
    [Attributes.ClassAttributes.Description("This command allows you to get sum value from a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get sum value from a list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetSumFromListCommand : AListGetMathResultFromListCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_WhenValueIsNotNumeric))]
        //public string v_WhenValueIsNotNumeric { get; set; }

        public GetSumFromListCommand()
        {
            //this.CommandName = "GetSumFromListCommand";
            //this.SelectionName = "Get Sum From List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //ListControls.MathProcess(this, nameof(v_WhenValueIsNotNumeric), v_List, engine,
            //    new Func<List<decimal>, decimal>((lst) =>
            //    {
            //        return lst.Sum();
            //    })
            //).StoreInUserVariable(engine, v_Result);

            this.MathProcess(
                new Func<List<decimal>, decimal>(lst => lst.Sum()),
                engine
            ).StoreInUserVariable(engine, v_Result);
        }
    }
}