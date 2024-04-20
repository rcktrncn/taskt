namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Dictionary & Key properties
    /// </summary>
    public interface IDictionaryKeyProperties : ILDictionaryProperties
    {
        /// <summary>
        /// Dictionary key name
        /// </summary>
        string v_Key { get; set; }
    }
}
