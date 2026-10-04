using System;
using System.Windows.Automation;
using taskt.Core.AutomationElement_UIElement;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_CanHandleUIElementScrollBarExtensionMethods
    {
        public static ScrollPattern GetScrollPattern(AutomationElement targetElement, Action notScrollBarAction)
        {
            if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
            {
                if (targetElement.Current.ControlType == ControlType.ScrollBar)
                {
                    var parentElement = UIElementInspector.GetParentUIElement(targetElement);
                    if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                    {
                        notScrollBarAction();
                        return null;
                    }
                }
                else
                {
                    notScrollBarAction();
                    return null;
                }
            }
            return (ScrollPattern)scrollPtn;
        }
    }
}
