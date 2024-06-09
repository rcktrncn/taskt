using Newtonsoft.Json.Linq;
using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_JSONValueActionPropertiesExtentionMethods
    {
        /// <summary>
        /// Expand Value or Variable Value In JSON Value
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string, JContainer) ExpandValueOrVariableValueInJSONValue(this IJSONValueActionProperties command, Engine.AutomationEngineInstance engine)
        {
            (var str, var json, _) = command.ExpandValueOrUserVariableAsJSON(nameof(command.v_Value), engine);
            var t = ((ScriptCommand)command).ExpandValueOrUserVariableAsSelectionItem(nameof(command.v_ValueType), engine);
            switch (t)
            {
                case "auto":
                    return (str, json);
                    
                case "array":
                    if (json is JArray)
                    {
                        return (str, json);
                    }
                    else
                    {
                        throw new Exception($"Spceified Value is NOT JSON Array. Value: '{str}'");
                    }

                case "object":
                    if (json is JObject)
                    {
                        return (str, json);
                    }
                    else
                    {
                        throw new Exception($"Spceified Value is NOT JSON Object. Value: '{str}'");
                    }

                default:
                    throw new Exception($"Spceified Value is NOT JSON. Value: '{str}'");
            }
        }
    }
}
