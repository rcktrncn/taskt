using System;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Select Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to select Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSelectShapeByNameCommand : AExcelShapeActionCommands
    {
        public ExcelSelectShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                shape.Select();
            }));
        }
    }
}