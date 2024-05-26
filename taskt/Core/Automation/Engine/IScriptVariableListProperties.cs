using System.Collections.Generic;
using taskt.Core.Script;

namespace taskt.Core.Automation.Engine
{
    public interface IScriptVariableListProperties
    {
        /// <summary>
        /// Variable List
        /// </summary>
        List<ScriptVariable> VariableList { get; set; }
    }
}
