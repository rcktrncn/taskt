namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Get something from DataTable properties
    /// </summary>
    public interface IDataTableGetFromDataTable : ILDataTableProperties
    {
        /// <summary>
        /// Variable Name to Store Result
        /// </summary>
        string v_Result { get; set; }
    }
}
