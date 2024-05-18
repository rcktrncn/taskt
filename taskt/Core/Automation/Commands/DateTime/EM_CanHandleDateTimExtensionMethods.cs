﻿using System;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleDateTimExtensionMethods
    {
        /// <summary>
        /// Expand User Variable As DateTime
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ExpandValueOrVariableAsDateTime(this ICanHandleDateTime command, ScriptVariable variable)
        {
            if (variable.VariableValue is DateTime time)
            {
                return time;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not DateTime");
            }
        }

        /// <summary>
        /// expand value or variable as DateTime
        /// </summary>
        /// <param name="command"></param>
        /// <param name="paramterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ExpandValueOrVariableAsDateTime(this ICanHandleDateTime command, string paramterName, AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(paramterName, "DateTime");
            //var v = variableName.GetRawVariable(engine);
            //if (v.VariableValue is DateTime time)
            //{
            //    return time;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not DateTime");
            //}
            try
            {
                return command.ExpandValueOrVariableAsDateTime(variableName.GetRawVariable(engine));
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not DateTime");
            }
        }

        /// <summary>
        /// store DateTime in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="c"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDateTimeInUserVariable(this ICanHandleDateTime command, DateTime c, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DateTime Variable");
            ExtensionMethods.StoreInUserVariable(variableName, c, engine);
        }
    }
}
