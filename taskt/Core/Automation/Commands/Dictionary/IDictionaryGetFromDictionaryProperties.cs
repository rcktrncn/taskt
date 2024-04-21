namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Get something from Dictionary properties
    /// </summary>
    public interface IDictionaryGetFromDictionaryProperties : ILDictionaryProperties
    {
        /// <summary>
        /// Variable name to store the result obtained from Dictionary
        /// </summary>
        string v_Result { get; set; }
    }
}
