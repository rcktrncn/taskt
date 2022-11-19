﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableRowValuesByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name to be setted a row")]
        [InputSpecification("Enter a existing DataTable Variable Name")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable to setted", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to setted")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row index to setted values")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
        [PropertyDisplayText(true, "Row to setted")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the DataTable Variable Name to set to the DataTable")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable to set", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to set")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row index to set values")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
        [PropertyDisplayText(true, "Row to set")]
        public string v_SrcRowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the if DataTable column does not exists")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_NotExistsKey { get; set; }

        public SetDataTableRowValuesByDataTableCommand()
        {
            this.CommandName = "SetDataTableRowValuesByDataTableCommand";
            this.SelectionName = "Set DataTable Row Values By DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable myDT = v_DataTableName.GetDataTableVariable(engine);
            //int rowIndex = DataTableControls.GetRowIndex(v_DataTableName, v_RowIndex, engine);
            (var myDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_RowIndex), engine);

            //DataTable addDT = v_RowName.GetDataTableVariable(engine);
            //int srcRowIndex = DataTableControls.GetRowIndex(v_RowName, v_SrcRowIndex, engine);
            (var addDT, var srcRowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_RowName), nameof(v_SrcRowIndex), engine);

            //string notExistsKey = v_NotExistsKey.GetUISelectionValue("v_NotExistsKey", this, engine);
            string ifNotColumnExists = this.GetUISelectionValue(nameof(v_NotExistsKey), "Column not exists", engine);

            // get columns list
            //List<string> columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            new GetDataTableColumnListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_OutputList = ExtensionMethods.GetInnerVariableName(0, engine)
            }.RunCommand(engine);
            var columns = (List<string>)ExtensionMethods.GetInnerVariable(0, engine).VariableValue;

            if (ifNotColumnExists == "error")
            {
                for (int i = 0; i < addDT.Columns.Count; i++)
                {
                    if (!columns.Contains(addDT.Columns[i].ColumnName))
                    {
                        throw new Exception("Column name " + addDT.Columns[i].ColumnName + " does not exists");
                    }
                }
            }
            for (int i = 0; i < addDT.Columns.Count; i++)
            {
                if (columns.Contains(addDT.Columns[i].ColumnName))
                {
                    myDT.Rows[rowIndex][addDT.Columns[i].ColumnName] = addDT.Rows[srcRowIndex][addDT.Columns[i].ColumnName];
                }
            }
        }
    }
}