﻿using Newtonsoft.Json.Linq;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Value From JSON Array")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Value From JSON Array")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get JSON Value From JSON Array")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValueFromJSONArrayCommand : AJSONGetFromJContainerCommands
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_ArrayIndex))]
        [PropertyIsOptional(false, "")]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(7000)]
        public string v_ArrayIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);
            if (jCon is JArray ary)
            {
                var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_ArrayIndex), "Index", engine);
                if (index < 0)
                {
                    index += ary.Count;
                }
                if (index >= 0 && index < ary.Count)
                {
                    ary[index].ToString().StoreInUserVariable(engine, v_Result);
                }
                else
                {
                    throw new Exception($"Array Index is Out of Range. Index; '{v_ArrayIndex}', Expand: '{index}'");
                }
            }
            else
            {
                throw new Exception($"Extraction Result is NOT Supported Type. Result: '{jCon}', JSONPath: '{v_JsonExtractor}'");
            }
        }
    }
}
