﻿using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableColumnToDataTableCommand : ADataTableGetFromDataTableColumnCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //[PropertyDescription("DataTable Variable Name to Converted")]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        public override string v_Result { get; set; }

        public ConvertDataTableColumnToDataTableCommand()
        {
            //this.CommandName = "ConvertDataTableColumnToDataTableCommand";
            //this.SelectionName = "Convert DataTable Column To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var srcDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            (var srcDT, var colIndex, var colName) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);
            
            DataTable myDT = new DataTable();
            myDT.Columns.Add(colName);
            for (int i = 0; i < srcDT.Rows.Count; i++)
            {
                myDT.Rows.Add();
                myDT.Rows[i][0] = srcDT.Rows[i][colIndex]?.ToString() ?? "";
            }

            //myDT.StoreInUserVariable(engine, v_Result);
            //this.StoreDataTableInUserVariable(myDT, nameof(v_Result), engine);
            this.StoreDataTableInUserVariable(myDT, engine);
        }
    }
}