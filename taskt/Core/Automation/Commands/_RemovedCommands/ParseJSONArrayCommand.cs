﻿//using System;
//using System.Collections.Generic;
//using System.Xml.Serialization;
//using taskt.Core.Automation.Attributes.PropertyAttributes;

//namespace taskt.Core.Automation.Commands
//{
//    [Serializable]
//    [Attributes.ClassAttributes.Group("JSON")]
//    [Attributes.ClassAttributes.SubGruop("Convert")]
//    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON Array into a list.")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
//    [Attributes.ClassAttributes.EnableAutomateRender(true)]
//    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
//    public sealed class ParseJSONArrayCommand : ScriptCommand, ICanHandleList
//    {
//        [XmlAttribute]
//        [PropertyDescription("Supply the JSON Array or Variable")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Select or provide a variable or json array value")]
//        [SampleUsage("**[1,2,3]** or **[{obj1},{obj2}]** or **{{{vArrayVariable}}}**")]
//        [Remarks("")]
//        [PropertyShowSampleUsageInDescription(true)]
//        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
//        [PropertyDisplayText(true, "JSON")]
//        public string v_Json { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please select the variable to receive the List")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Select or provide a variable from the variable list")]
//        [SampleUsage("**vSomeVariable**")]
//        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
//        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [PropertyIsVariablesList(true)]
//        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
//        [PropertyDisplayText(true, "List")]
//        public string v_Result { get; set; }

//        public ParseJSONArrayCommand()
//        {
//            this.CommandName = "ParseJSONArrayCommand";
//            this.SelectionName = "Parse JSON Array";
//            this.CommandEnabled = true;
//            this.CustomRendering = true; 
//        }

//        public override void RunCommand(Engine.AutomationEngineInstance engine)
//        {
//            //get variablized input
//            var variableInput = v_Json.ExpandValueOrUserVariable(engine);

//            //create objects
//            Newtonsoft.Json.Linq.JArray arr;
//            List<string> resultList = new List<string>();

//            //parse json
//            try
//            {
//                arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
//            }
 
//            //add results to result list since list<string> is supported
//            foreach (var result in arr)
//            {
//                resultList.Add(result.ToString());
//            }

//            //resultList.StoreInUserVariable(engine, v_applyToVariableName);
//            this.StoreListInUserVariable(resultList, nameof(v_Result), engine);
//        }
//    }
//}