﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command set Row values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Row")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Column Type")]
        [InputSpecification("")]
        [SampleUsage("**Range** or **RC**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Range")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertyValueSensitive(false)]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Start Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "A or 1")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Start Column")]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the End Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "End of List")]
        [PropertyDisplayText(true, "End Column")]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the List Variable Name to set")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_ListVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Value type to set")]
        [InputSpecification("")]
        [SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertyValueSensitive(false)]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify If List Items not enough")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyValueSensitive(false)]
        public string v_IfListNotEnough { get; set; }

        public ExcelSetRowValuesFromListCommand()
        {
            this.CommandName = "ExcelSetRowValuesFromListCommand";
            this.SelectionName = "Set Row Values From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            // get list
            List<string> myList = v_ListVariable.GetListVariable(engine);

            (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
                ExcelControls.GetRangeIndeiesRowDirection(
                    nameof(v_RowIndex), nameof(v_ColumnType),
                    nameof(v_ColumnStart), nameof(v_ColumnEnd),
                    nameof(v_ValueType), engine, excelSheet, this,
                    myList
                );

            //string ifListNotEnough = v_IfListNotEnough.GetUISelectionValue("v_IfListNotEnough", this, engine);
            string ifListNotEnough = this.GetUISelectionValue(nameof(v_IfListNotEnough), "If List Not Enough", engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifListNotEnough == "error")
            {
                if (range > myList.Count)
                {
                    throw new Exception("List Items not enough");
                }
            }

            int max = range;
            if (range > myList.Count)
            {
                max = myList.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnStartIndex + i, rowIndex);
            }
        }
    }
}