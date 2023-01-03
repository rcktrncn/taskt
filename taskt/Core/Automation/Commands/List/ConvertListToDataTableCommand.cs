﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.Description("This command convert a List to a DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertListToDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to convert")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ColumnType))]
        [PropertyDescription("DataTable Columns Type")]
        [PropertyUISelectionOption("Column Prefix")]
        [PropertyIsOptional(true, "Column Prefix")]
        [PropertyDisplayText(true, "DataTable Columns Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable Columns Name List")]
        [InputSpecification("")]
        [SampleUsage("**vColumns** or **{{{vColumns}}}**")]
        [Remarks("If Columns is empty, DataTable column is column0, column1, ...")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyDisplayText(true, "Columns Name List")]
        public string v_Columns { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ANotEnough))]
        [PropertyDescription("When the number of items in the List is greater than the number of Columns")]
        [PropertyUISelectionOption("Try Create Columns")]
        public string v_ColumnsNotEnough { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListItemNotEnough))]
        [PropertyDescription("When the number of Columns is greater than the number of items in the List")]
        public string v_ListItemNotEnough { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable Variable Name")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_applyToVariableName { get; set; }

        public ConvertListToDataTableCommand()
        {
            this.CommandName = "ConvertListToDataTableCommand";
            this.SelectionName = "Convert List To DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_InputList.GetListVariable(engine);

            DataTable myDT = new DataTable();

            Action<List<string>> dtUseColumns = new Action<List<string>>((targetColumns) =>
            {
                string columnsNotEnough = this.GetUISelectionValue(nameof(v_ColumnsNotEnough), "Columns Not Enough", engine);
                string listItemNotEnough = this.GetUISelectionValue(nameof(v_ListItemNotEnough), "List Item Not Enough", engine);

                if ((columnsNotEnough == "error") && (targetList.Count > targetColumns.Count))
                {
                    throw new Exception("The number of keys in " + v_Columns + " is not enough");
                }
                if ((listItemNotEnough == "error") && (targetColumns.Count > targetList.Count))
                {
                    throw new Exception("The number of List items in " + v_InputList + " is not enough");
                }

                if (targetList.Count == targetColumns.Count)
                {
                    myDT.Rows.Add();
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        myDT.Columns.Add(targetColumns[i]);
                        myDT.Rows[0][i] = targetList[i];
                    }
                }
                else if (targetList.Count > targetColumns.Count)
                {
                    switch (columnsNotEnough)
                    {
                        case "ignore":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;

                        case "try create columns":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetColumns.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            for (int i = targetColumns.Count; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add("column" + i.ToString());
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;
                    }
                }
                else
                {
                    switch (listItemNotEnough)
                    {
                        case "ignore":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;

                        case "insert empty value":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            for (int i = targetList.Count; i < targetColumns.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = "";
                            }
                            break;
                    }
                }
            });

            List<string> columnsList;

            string columnsType = this.GetUISelectionValue(nameof(v_ColumnType), "Columns Type", engine);
            switch (columnsType)
            {
                case "list":
                    columnsList = v_Columns.GetListVariable(engine);
                    dtUseColumns(columnsList);
                    break;
                case "comma separated":
                    columnsList = v_Columns.ConvertToUserVariable(engine).Split(',').ToList();
                    dtUseColumns(columnsList);
                    break;
                case "space separated":
                    columnsList = v_Columns.ConvertToUserVariable(engine).Split(' ').ToList();
                    dtUseColumns(columnsList);
                    break;
                case "tab separated":
                    columnsList = v_Columns.ConvertToUserVariable(engine).Split('\t').ToList();
                    dtUseColumns(columnsList);
                    break;
                case "newline separated":
                    columnsList = v_Columns.ConvertToUserVariable(engine).Replace("\r\n", "\n").Replace("\r", "\n").Split('\n').ToList();
                    dtUseColumns(columnsList);
                    break;
                case "column prefix":
                    string columnPrefix;
                    if (String.IsNullOrEmpty(v_Columns))
                    {
                        columnPrefix = "column";
                    }
                    else
                    {
                        columnPrefix = v_Columns.ConvertToUserVariable(engine);
                    }

                    myDT.Rows.Add();
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        myDT.Columns.Add(columnPrefix + i.ToString());
                        myDT.Rows[0][i] = targetList[i];
                    }
                    break;
            }
            myDT.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}