﻿namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// row Range Get commands properties
    /// </summary>
    public interface IExcelRowRangeGetProperties : IExcelRowRangeProperties
    {
        /// <summary>
        /// variable name to store result
        /// </summary>
        string v_Result { get; set; }
    }
}
