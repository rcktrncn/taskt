using System;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Select Chart By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to select Chart by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select Chart by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSelectChartByNameCommand : AExcelChartActionCommands
    {
        public ExcelSelectChartByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                chart.Select();
            }));
        }
    }
}