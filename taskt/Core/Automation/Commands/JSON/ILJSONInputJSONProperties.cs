namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Json input properties
    /// </summary>
    public interface ILJSONInputJSONProperties : ICanHandleJSON, ILExpandableProperties
    {
        /// <summary>
        /// JSON Value or Variable Name
        /// </summary>
        string v_Json { get; set; }
    }
}
