using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Native.Windows;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Set Window State By Window Handle")]
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetWindowStateByWindowHandleCommand : AWindowHandleActionCommands, IWindowStateProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("State of the Window")]
        //[PropertyUISelectionOption("Maximize")]
        //[PropertyUISelectionOption("Minimize")]
        //[PropertyUISelectionOption("Restore")]
        //[InputSpecification("", true)]
        //[PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "State")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowState))]
        [PropertyParameterOrder(5500)]
        public string v_WindowState { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        //public string v_WaitTimeBetweenFindAndAction

        public SetWindowStateByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowHandleActionBeforeWaitActivate(engine, new Action<IntPtr>((whnd) =>
            {
                var state = WindowAPI.WindowState.MINIMIZE;
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WindowState), engine))
                {
                    case "maximize":
                        state = WindowAPI.WindowState.MINIMIZE;
                        break;
                    case "minimize":
                        state = WindowAPI.WindowState.MINIMIZE;
                        break;
                    case "restore":
                        state = WindowAPI.WindowState.RESTORE;
                        break;
                    case "3":
                        state = WindowAPI.WindowState.MINIMIZE;
                        break;
                    case "2":
                        state = WindowAPI.WindowState.MINIMIZE;
                        break;
                    case "1":
                        state = WindowAPI.WindowState.RESTORE;
                        break;
                }

                WindowAPI.SetWindowState(whnd, state);
            }));
        }
    }
}