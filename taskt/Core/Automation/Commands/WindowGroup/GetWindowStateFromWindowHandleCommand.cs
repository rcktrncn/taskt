using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Native.Windows;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window State From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns a state of window name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a window state.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowStateFromWindowHandleCommand : AWindowHandleCommands, IWindowStateProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State")]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        [PropertyParameterOrder(5500)]
        public string v_WindowState { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State Text", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State Text")]
        [PropertyParameterOrder(5501)]
        public string v_WindowStateText { get; set; }

        public GetWindowStateFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowHandleAction(engine, new Action<IntPtr>((whnd) =>
            {
                (var value, var text) = WindowAPI.GetWindowState(whnd);

                if (!string.IsNullOrEmpty(v_WindowState))
                {
                    value.StoreInUserVariable(engine, v_WindowState);
                }
                if (!string.IsNullOrEmpty(v_WindowStateText))
                {
                    text.StoreInUserVariable(engine, v_WindowStateText);
                }
            }));
        }
    }
}