using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertJSONToDictionaryCommand : ScriptCommand, ICanHandleDictionary
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        public string v_Json { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_Result { get; set; }

        public ConvertJSONToDictionaryCommand()
        {
            //this.CommandName = "ConvertJSONToDictionaryCommand";
            //this.SelectionName = "Convert JSON To Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            Action<JObject> objFunc = new Action<JObject>((obj) =>
            {
                var resultDic = new Dictionary<string, string>();
                foreach (var result in obj)
                {
                    resultDic.Add(result.Key, result.Value.ToString());
                }
                //resultDic.StoreInUserVariable(engine, v_applyToVariableName);
                this.StoreDictionaryInUserVariable(resultDic, nameof(v_Result), engine);
            });
            Action<JArray> aryFunc = new Action<JArray>((ary) =>
            {
                var resultDic = new Dictionary<string, string>();
                for (int i = 0; i < ary.Count; i++)
                {
                    resultDic.Add("key" + i.ToString(), ary[i].ToString());
                }
                //resultDic.StoreInUserVariable(engine, v_applyToVariableName);
                this.StoreDictionaryInUserVariable(resultDic, nameof(v_Result), engine);
            });
            this.JSONProcess(nameof(v_Json), objFunc, aryFunc, engine);
        }
    }
}