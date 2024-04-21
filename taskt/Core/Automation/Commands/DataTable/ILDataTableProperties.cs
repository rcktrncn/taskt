namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DataTable properties
    /// </summary>
    public interface ILDataTableProperties : ICanHandleDataTable
    {
        /// <summary>
        /// DataTable variabe name
        /// </summary>
        string v_DataTable { get; set; }
    }
}
