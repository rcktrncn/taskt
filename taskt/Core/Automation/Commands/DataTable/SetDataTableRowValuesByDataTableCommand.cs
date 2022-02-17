﻿using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
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
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row index to setted values")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
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
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row index to set values")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
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

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);
            DataTable addDT = v_RowName.GetDataTableVariable(engine);

            //string vRowIndex = v_RowIndex.ConvertToUserVariable(engine);
            //int rowIndex = int.Parse(vRowIndex);
            //if ((rowIndex < 0) || (rowIndex >= myDT.Rows.Count))
            //{
            //    throw new Exception("Row Index is less than 0 or exceeds the number of rows in the DataTable");
            //}
            int rowIndex = DataTableControl.GetRowIndex(v_DataTableName, v_RowIndex, engine);

            //string vSrcRowIndex = v_SrcRowIndex.ConvertToUserVariable(engine);
            //int srcRowIndex = int.Parse(vSrcRowIndex);
            //if ((srcRowIndex < 0) || (srcRowIndex >= addDT.Rows.Count))
            //{
            //    throw new Exception("Row Index is less than 0 or exceeds the number of rows in the DataTable");
            //}
            int srcRowIndex = DataTableControl.GetRowIndex(v_RowName, v_SrcRowIndex, engine);

            //string notExistsKey;
            //if (String.IsNullOrEmpty(v_NotExistsKey))
            //{
            //    notExistsKey = "Ignore";
            //}
            //else
            //{
            //    notExistsKey = v_NotExistsKey.ConvertToUserVariable(engine);
            //}
            //notExistsKey = notExistsKey.ToLower();
            //switch (notExistsKey)
            //{
            //    case "ignore":
            //    case "error":
            //        break;
            //    default:
            //        throw new Exception("Strange value in if Dictionary key does not exists " + v_NotExistsKey);
            //}
            string notExistsKey = v_NotExistsKey.GetUISelectionValue("v_NotExistsKey", this, engine);

            // get columns list
            List<string> columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            if (notExistsKey == "error")
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set DataTable '" + v_DataTableName + "' Row '" + v_RowIndex + "' By DataTable '" + v_RowName + "' Row '" + v_SrcRowIndex + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DataTableName))
        //    {
        //        this.validationResult += "DataTable Name to setted is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_RowIndex))
        //    {
        //        this.validationResult += "Row index to setted is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int index;
        //        if (int.TryParse(this.v_RowIndex, out index))
        //        {
        //            if (index < 0)
        //            {
        //                this.validationResult += "Row index to setted is less than 0.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }
        //    if (String.IsNullOrEmpty(this.v_RowName))
        //    {
        //        this.validationResult += "DataTable name to set is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SrcRowIndex))
        //    {
        //        this.validationResult += "Row Index to set is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int index;
        //        if (int.TryParse(this.v_SrcRowIndex, out index))
        //        {
        //            if (index < 0)
        //            {
        //                this.validationResult += "Row index to setted is less than 0.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}