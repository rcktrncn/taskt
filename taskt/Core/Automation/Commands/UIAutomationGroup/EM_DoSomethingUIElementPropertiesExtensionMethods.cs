using System;
using System.Windows.Automation;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_DoSomethingUIElementPropertiesExtensionMethods
    {
        /// <summary>
        /// UIElement action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        public static void UIElementAction(this IDoSomethingUIElementProperties command, AutomationEngineInstance engine, Action<AutomationElement> actionFunc)
        {
            var targetElement = command.ExpandUserVariableAsUIElement(engine);

            // core process
            actionFunc(targetElement);

            command.StoreWindowNameAndWindowHandleInUserVariablesFromUIElement(targetElement, engine);
        }
    }
}
