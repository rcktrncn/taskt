using Newtonsoft.Json.Linq;
using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_JSONJSONPathPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value or User Variable as JSON by JSONPath
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns>(JToken, type)</returns>
        /// <exception cref="Exception"></exception>
        public static (JToken, string) ExpandValueOrUserVariableAsJSONByJSONPath(this IJSONJSONPathProperties command, Engine.AutomationEngineInstance engine)
        {
            (_, var json, _) = command.ExpandValueOrUserVariableAsJSON(engine);
            var path = ((ScriptCommand)command).ExpandValueOrUserVariable(nameof(command.v_JsonExtractor), "JSONPath", engine);

            var token = json.SelectToken(path);
            if (token is JObject obj)
            {
                return (obj, "object");
            }
            else if (token is JArray ary)
            {
                return (ary, "array");
            }
            else if (token is JValue v)
            {
                return (v, "value");
            }
            else
            {
                throw new Exception($"Nothing found in JSONPath. Path: '{command.v_JsonExtractor}', ExtractPath: '{path}'");
            }
        }
    }
}
