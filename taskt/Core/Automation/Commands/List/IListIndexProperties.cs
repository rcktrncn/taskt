namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// List Index properties
    /// </summary>
    public interface IListIndexProperties : ILListProperties
    {
        /// <summary>
        /// List Index
        /// </summary>
        string v_Index { get; set; }
    }
}
