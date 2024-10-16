﻿namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// List commands properties
    /// </summary>
    public interface ILListProperties : ICanHandleList, ILExpandableProperties
    {
        /// <summary>
        /// List variable name
        /// </summary>
        string v_List { get; set; }
    }
}
