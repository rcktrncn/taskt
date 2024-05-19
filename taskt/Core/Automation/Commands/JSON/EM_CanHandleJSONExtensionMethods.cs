using System;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJSONExtensionMethods
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
        public static (string, JSONType) DetectJSONType(string json)
        {
            json = json.Trim();
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                try
                {
                    JObject.Parse(json);
                    return (json, JSONType.Object);
                }
                catch
                {
                    return ("", JSONType.NotJSON);
                }
            }
            else if (json.StartsWith("[") && json.EndsWith("]"))
            {
                try
                {
                    JArray.Parse(json);
                    return (json, JSONType.Array);
                }
                catch
                {
                    return ("", JSONType.NotJSON);
                }
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
            (var jsonText, var jsonType) = DetectJSONType(text);
            switch (jsonType)
            {
                case JSONType.Object:
                case JSONType.Array:
                    return (jsonText, jsonType.ToString().ToLower());

                default:
                    throw new Exception($"Specified Text is Not JSON.");
            }
        }

        /// <summary>
        /// Store JSON in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="list"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONInUserVariable(this ICanHandleJSON command, string json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "JSON Variable");
            ExtensionMethods.StoreInUserVariable(variableName, json, engine);
        }
    }
}
