namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// DataTable Row Position Properties
    /// </summary>
    public interface ILDataTableRowPositionProperties : ILExpandableProperties
    {
        /// <summary>
        /// DataTable Row Index
        /// </summary>
        string v_RowIndex { get; set; }
    }
}
