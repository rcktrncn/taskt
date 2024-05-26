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
            if (EM_CanHandleJSONObjectExtensionMethods.IsJSONObject(json, out (string str, JObject json) to))
            {
                return (to.str, JSONType.Object);
            }
            else if (EM_CanHandleJSONArrayExtentionMethods.IsJSONArray(json, out (string str, JArray json) ta))
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

        ///// <summary>
        ///// Store JSON in User Variable
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="list"></param>
        ///// <param name="parameterName"></param>
        ///// <param name="engine"></param>
        //public static void StoreJSONInUserVariable(this ICanHandleJSON command, string json, string parameterName, Engine.AutomationEngineInstance engine)
        //{
        //    var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "JSON Variable");
        //    ExtensionMethods.StoreInUserVariable(variableName, json, engine);
        //}
    }
}
