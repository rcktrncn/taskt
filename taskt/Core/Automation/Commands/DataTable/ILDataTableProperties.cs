namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DataTable properties
    /// </summary>
    public interface ILDataTableProperties : ICanHandleDataTable, ILExpandableProperties
    {
        /// <summary>
        /// DataTable variabe name
        /// </summary>
        string v_DataTable { get; set; }
    }
}
