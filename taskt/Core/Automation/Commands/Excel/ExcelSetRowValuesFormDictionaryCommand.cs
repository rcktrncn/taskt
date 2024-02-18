﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Set Row Values From Dictionary")]
    [Attributes.ClassAttributes.Description("This command set Row values from Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromDictionaryCommand : AExcelInstanceCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        [PropertyParameterOrder(6000)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        [PropertyParameterOrder(6001)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        [PropertyParameterOrder(6002)]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        [PropertyParameterOrder(6003)]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyParameterOrder(6004)]
        public string v_DictionaryVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        [PropertyParameterOrder(6005)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When Dictionary Items Not Enough")]
        [PropertyParameterOrder(6006)]
        public string v_WhenItemNotEnough { get; set; }

        public ExcelSetRowValuesFromDictionaryCommand()
        {
            //this.CommandName = "ExcelSetRowValuesFromDictionaryCommand";
            //this.SelectionName = "Set Row Values From Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);

            Dictionary<string, string> myDic = v_DictionaryVariable.ExpandUserVariableAsDictinary(engine);

            (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
                ExcelControls.GetRangeIndeiesRowDirection(
                    nameof(v_RowIndex), nameof(v_ColumnType),
                    nameof(v_ColumnStart), nameof(v_ColumnEnd),
                    nameof(v_ValueType), engine, excelSheet, this,
                    myDic
                );

            //string ifListNotEnough = v_IfDictionaryNotEnough.GetUISelectionValue("v_IfDictionaryNotEnough", this, engine);
            string ifListNotEnough = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenItemNotEnough), "If Dictionary Not Enough", engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifListNotEnough == "error")
            {
                if (range > myDic.Count)
                {
                    throw new Exception("Dictionary Items not enough");
                }
            }

            int max = range;
            if (range > myDic.Count)
            {
                max = myDic.Count;
            }

            //Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.ExpandValueOrUserVariableAsSelectionItem("v_ValueType", this, engine));
            var setFunc = ExcelControls.SetCellValueFunction(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), engine));

            string[] keys = new string[myDic.Count];
            myDic.Keys.CopyTo(keys, 0);

            for (int i = 0; i < max; i++)
            {
                setFunc(myDic[keys[i]], excelSheet, columnStartIndex + i, rowIndex);
            }
        }
    }
}