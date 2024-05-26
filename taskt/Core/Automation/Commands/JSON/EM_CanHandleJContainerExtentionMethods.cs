using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJContainerExtentionMethods
    {
        /// <summary>
        /// Store JSON in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONInUserVariable(this ICanHandleJContainer command, JContainer json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            //var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "JSON Variable");
            //ExtensionMethods.StoreInUserVariable(variableName, json.ToString(), engine);
            StoreJSONInUserVariable(command, json.ToString(), parameterName, engine);
        }

        /// <summary>
        /// Store JSON in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONInUserVariable(this ICanHandleJContainer command, string json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "JSON Variable");
            ExtensionMethods.StoreInUserVariable(variableName, json.ToString(), engine);
        }
    }
}
