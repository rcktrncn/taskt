using System;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Script
{
    /// <summary>
    /// Inner Script Variable
    /// </summary>
    public class InnerScriptVariable : ScriptVariable, IDisposable
    {
        private AutomationEngineInstance engine;

        public InnerScriptVariable(AutomationEngineInstance engine)
        {
            this.engine = engine;

            var variableList = engine.VariableList;
            int i = 0;
            while (true)
            {
                if (variableList.Exists(v => (v.VariableName == VariableNameControls.INNER_VARIABLE_PREFIX + i.ToString())))
                {
                    i++;
                }
                else
                {
                    break;
                }
            }
            this.VariableName = VariableNameControls.INNER_VARIABLE_PREFIX + i.ToString();
            variableList.Add(this);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            var variableLists = engine.VariableList;

            var idx = variableLists.IndexOf(this);
            if (idx >= 0)
            {
                variableLists.RemoveAt(idx);
            }
        }

        ~InnerScriptVariable()
        {
            Dispose(false);
        }
    }
}
