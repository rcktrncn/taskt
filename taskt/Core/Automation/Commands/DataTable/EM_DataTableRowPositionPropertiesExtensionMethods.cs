﻿using System;
using System.Data;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableRowPositionPropertiesExtensionMethods
    {
        /// <summary>
        /// Expand Value or User Variable as DataTable Row
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ExpandValueOrUserVariableAsDataTableRow(this ILDataTableRowPositionProperties command, DataTable table, Engine.AutomationEngineInstance engine)
        {
            var index = ((ScriptCommand)command).ExpandValueOrUserVariableAsInteger(nameof(command.v_RowIndex), "Row Index", engine);
            if (index < 0)
            {
                index += table.Rows.Count;
            }
            if (index >= 0 && index < table.Rows.Count)
            {
                return index;
            }
            else
            {
                throw new Exception($"Strange DataTable Row Index. Value: '{command.v_RowIndex}', Expand Value: '{index}'");
            }
        }
    }
}
