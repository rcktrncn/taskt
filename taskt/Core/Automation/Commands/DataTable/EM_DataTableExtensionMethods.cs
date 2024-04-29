using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    public static class EM_DataTableExtensionMethods
    {
        /// <summary>
        /// Get DataTable Column Name List
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<string> GetColumnNameList(this DataTable table)
        {
            return table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
        }
    }
}
