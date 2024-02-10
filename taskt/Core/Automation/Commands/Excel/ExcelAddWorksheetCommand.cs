﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Add Worksheet")]
    [Attributes.ClassAttributes.Description("This command adds a new Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelAddWorksheetCommand : AExcelInstanceCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("New Worksheet Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**mySheet**", PropertyDetailSampleUsage.ValueType.Value, "Worksheet Name")]
        [PropertyDetailSampleUsage("**{{{vSheet}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Worksheet Name")]
        [PropertyParameterOrder(6000)]
        public string v_NewSheetName { get; set; }

        public ExcelAddWorksheetCommand()
        {
            //this.CommandName = "ExcelAddWorksheetCommand";
            //this.SelectionName = "Add Worksheet";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var excelInstance = v_InstanceName.ExpandValueOrUserVariableAsExcelInstance(engine);
            excelInstance.Worksheets.Add();

            var sheetName = v_NewSheetName.ExpandValueOrUserVariable(engine);
            if (!string.IsNullOrEmpty(sheetName))
            {
                ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name = sheetName;
            }
        }
    }
}