﻿using System;
using Newtonsoft.Json.Linq;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleJSONObjectExtensionMethods
    {
        /// <summary>
        /// Check object is JSONObject
        /// </summary>
        /// <param name="value"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsJSONObject(object value, out (string str, JObject json) r)
        {
            r = ("", default);
            if (value is string str)
            {
                str = str.Trim();
                if (str.StartsWith("{") && str.EndsWith("}"))
                {
                    try
                    {
                        r.json = JObject.Parse(str.Trim());
                        r.str = str;
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Expand Value or User Variable as JSONObject
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (string, JObject) ExpandValueOrUserVariableAsJSONObject(ScriptVariable variable)
        {
            if (IsJSONObject(variable.VariableValue, out (string, JObject) r))
            {
                return r;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not JSONObject.");
            }
        }

        /// <summary>
        /// Expand Value or User Variable As JSONObject
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static (string, JObject) ExpandValueOrUserVariableAsJSONObject(this ICanHandleJSONObject command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "JSON");
            try
            {
                return ExpandValueOrUserVariableAsJSONObject(variableName.GetRawVariable(engine));
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not JSON.");
            }
        }

        /// <summary>
        /// Store JSONObject in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONObjectInUserVariable(this ICanHandleJSONObject command, JObject json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            command.StoreJSONInUserVariable(json, parameterName, engine);
        }

        /// <summary>
        /// Store JSONObject in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="json"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreJSONObjectInUserVariable(this ICanHandleJSONObject command, string json, string parameterName, Engine.AutomationEngineInstance engine)
        {
            command.StoreJSONInUserVariable(json, parameterName, engine);
        }
    }
}
