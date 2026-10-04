using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using taskt.Core.AutomationElement_UIElement;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_GetUIElementsInformationPropertiesExtentionMethods
    {
        /// <summary>
        /// store UIElements Information in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="elems"></param>
        /// <param name="engine"></param>
        public static void StoreUIElementsInformationInUserVariable(this IGetUIElementsInformationProperties command, List<AutomationElement> elems, Engine.AutomationEngineInstance engine)
        {
            //string result = "";

            var result = new StringBuilder();

            int counts = elems.Count;
            for (int i = 0; i < counts; i++)
            {
                var elem = elems[i];
                result.Append($"Index: {i}, Name: {elem.Current.Name}, LocalizedControlType: {elem.Current.LocalizedControlType}, ControlType: {UIElementInspector.GetControlTypeText(elem)}\n");
            }
            result.ToString().Trim().StoreInUserVariable(engine, command.v_Result);
        }
    }
}
