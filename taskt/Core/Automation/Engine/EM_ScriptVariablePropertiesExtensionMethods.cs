using System;
using System.Linq;
using taskt.Core.Script;

namespace taskt.Core.Automation.Engine
{
    public static class EM_ScriptVariablePropertiesExtensionMethods
    {
        /// <summary>
        /// Try Get Script Variable
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="variableName"></param>
        /// <returns>ScritpVariable or Null</returns>
        public static ScriptVariable TryGetScriptVariable(this IScriptVariableListProperties engine, string variableName)
        {
            return engine.VariableList.Where(var => var.VariableName == variableName).FirstOrDefault();
        }

        /// <summary>
        /// Check ScriptVariable Exists
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="variableName">no wrapped variable marker</param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool CheckScriptVariableExists(this IScriptVariableListProperties engine, string variableName, out ScriptVariable v)
        {
            v = engine.TryGetScriptVariable(variableName);
            return (v != null);
        }

        /// <summary>
        /// Add ScriptVariable
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="variable"></param>
        /// <param name="allowOverride"></param>
        public static void AddScriptVariable(this IScriptVariableListProperties engine, ScriptVariable variable, bool allowOverride = true)
        {
            if (engine.CheckScriptVariableExists(variable.VariableName, out ScriptVariable v))
            {
                if (allowOverride)
                {
                    engine.RemoveScriptVariable(v.VariableName);
                    engine.VariableList.Add(variable);
                }
                else
                {
                    throw new Exception($"ScriptVariable '{v.VariableName}' is already Exists.");
                }
            }
            else
            {
                engine.VariableList.Add(variable);
            }
        }

        /// <summary>
        /// Remove ScriptVariable
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="variableName"></param>
        public static void RemoveScriptVariable(this IScriptVariableListProperties engine, string variableName)
        {
            if (engine.CheckScriptVariableExists(variableName, out ScriptVariable v))
            {
                engine.VariableList.Remove(v);
            }
            else
            {
                throw new Exception($"ScriptVariable '{variableName}' does not Exsits.");
            }
        }
    }
}
