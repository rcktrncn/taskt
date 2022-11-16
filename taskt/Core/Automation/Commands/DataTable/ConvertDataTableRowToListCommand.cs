﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableRowToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable to fet rows from.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the index of the Row")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid DataRow index value")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vRowIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Current Row")]
        [PropertyDisplayText(true, "Row")]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign The List")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableRowToListCommand()
        {
            this.CommandName = "ConvertDataTableRowToListCommand";
            this.SelectionName = "Convert DataTable Row To List";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            //int index = DataTableControls.GetRowIndex(v_DataTableName, v_DataRowIndex, engine);
            (var srcDT, var index) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_DataRowIndex), engine);

            List<string> myList = new List<string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                //if (srcDT.Rows[index][i] != null)
                //{
                //    myList.Add(srcDT.Rows[index][i].ToString());
                //}
                //else
                //{
                //    myList.Add("");
                //}
                myList.Add(srcDT.Rows[index][i]?.ToString() ?? "");
            }

            myList.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}