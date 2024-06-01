using System;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To List")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON Array into a List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON Array into a List")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertJSONToListCommand : AJSONGetFromJSONCommands, IListResultProperties, ICanHandleJContainer
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        //public string v_Json { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        public ConvertJSONToListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JObject> objFunc = new Action<JObject>((obj) =>
            //{
            //    List<string> resultList = new List<string>();
            //    foreach (var result in obj)
            //    {
            //        resultList.Add(result.Value.ToString());
            //    }
            //    //resultList.StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreListInUserVariable(resultList, nameof(v_Result), engine);
            //});
            //Action<JArray> aryFunc = new Action<JArray>((ary) =>
            //{
            //    List<string> resultList = new List<string>();
            //    foreach (var result in ary)
            //    {
            //        resultList.Add(result.ToString());
            //    }
            //    //resultList.StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreListInUserVariable(resultList, nameof(v_Result), engine);
            //});
            //this.JSONProcess(nameof(v_Json), objFunc, aryFunc, engine);

            (_, var jCon, _) = this.ExpandValueOrUserVariableAsJSON(nameof(v_Json), engine);
            var res = this.CreateEmptyList();
            if (jCon is JObject obj)
            {
                foreach(var item in obj)
                {
                    res.Add(item.Value.ToString());
                }
            }
            else if (jCon is JArray ary)
            {
                foreach(var item in ary)
                {
                    res.Add(item.ToString());
                }
            }
            this.StoreListInUserVariable(res, engine);
        }
    }
}