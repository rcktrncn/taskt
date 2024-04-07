using System.Data;

namespace taskt.Core.Automation.Commands
{
    public interface IDictionaryAddCreateProperties : ILDictionaryProperties
    {
        /// <summary>
        /// Dictionary Keys & Values
        /// </summary>
        DataTable v_ColumnNameDataTable { get; set; }
    }
}
