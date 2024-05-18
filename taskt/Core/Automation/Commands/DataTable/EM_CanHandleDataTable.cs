using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleDataTable
    {
        /// <summary>
        /// Expand User Variable As DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ExpandUserVariableAsDataTable(ScriptVariable variable)
        {
            // TODO: it's ok?
            if (variable.VariableValue is DataTable table)
            {
                return table;
            }
            else
            {
                throw new Exception($"Variable '{variable.VariableName}' is not DataTable");
            }
        }

        /// <summary>
        /// expand user variable as DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DataTable ExpandUserVariableAsDataTable(this ICanHandleDataTable command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DataTable Variable");
            //var v = variableName.GetRawVariable(engine);
            //if (v.VariableValue is DataTable table)
            //{
            //    return table;
            //}
            //else
            //{
            //    throw new Exception($"Variable '{variableName}' is not DataTable");
            //}
            try
            {
                return ExpandUserVariableAsDataTable(variableName.GetRawVariable(engine));
            }
            catch
            {
                throw new Exception($"Variable '{variableName}' is not DataTable");
            }
        }

        /// <summary>
        /// store DataTable in User Variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        public static void StoreDataTableInUserVariable(this ICanHandleDataTable command, DataTable table, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var variableName = ((ScriptCommand)command).GetRawPropertyValueAsString(parameterName, "DataTable Variable");
            ExtensionMethods.StoreInUserVariable(variableName, table, engine);
        }
    }
}
