using System;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJContainerExtentionMethods
    {
        public enum JSONType
        {
            NotJSON,
            Object,
            Array
        };

        /// <summary>
        /// Detect JSON Type
        /// </summary>
        /// <param name="json"></param>
        /// <returns>(jsonText, jsonType)</returns>
        public static (string, JSONType) DetectJSONType(string json, Engine.AutomationEngineInstance engine)
        {
            if (EM_CanHandleJSONObjectExtensionMethods.IsJSONObject(json, engine, out (string str, JObject json) to))
            {
                return (to.str, JSONType.Object);
            }
            else if (EM_CanHandleJSONArrayExtentionMethods.IsJSONArray(json, engine, out (string str, JArray json) ta))
            {
                return (ta.str, JSONType.Array);
            }
            else
            {
                return ("", JSONType.NotJSON);
            }
        }

        /// <summary>
        /// Expand Value or User Variable As JSON
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns>(jsonText, jsonType)</returns>
        public static (string, string) ExpandValueOrUserVariableAsJSON(this ICanHandleJSON command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var text = ((ScriptCommand)command).ExpandValueOrUserVariable(parameterName, "JSON", engine);
            (var jsonText, var jsonType) = DetectJSONType(text, engine);
            switch (jsonType)
            {
                case JSONType.Object:
                case JSONType.Array:
                    return (jsonText, jsonType.ToString().ToLower());

                default:
                    throw new Exception($"Specified Text is Not JSON.");
            }
        }
    }
}
