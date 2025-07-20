using System.Collections.Generic;
using taskt.Core.Script;

namespace taskt.UI.Forms
{
    // aliace for Instances Counter dictionary, too long!
    using InstanceCounterData = Dictionary<string, Dictionary<string, Dictionary<string, int>>>;

    // aliace lstScriptActions line state (isNewLine, isDontSave)
    using LineStatesData = List<(bool IsNewInsertedLine, bool IsDontSaveLine)>;

    /// <summary>
    /// Undo, Redo manager for frmScriptBuilder
    /// </summary>
    internal class UndoRedoManager4frmScriptBuilder
    {
        /// <summary>
        /// undo stack
        /// </summary>
        private readonly Stack<(Script, InstanceCounterData, LineStatesData)> undo = new Stack<(Script, InstanceCounterData, LineStatesData)>();
        /// <summary>
        /// redo stack
        /// </summary>
        private readonly Stack<(Script, InstanceCounterData, LineStatesData)> redo = new Stack<(Script, InstanceCounterData, LineStatesData)>();
        
        /// <summary>
        /// can undo?
        /// </summary>
        public bool CanUndo { get => (this.undo.Count > 0); }
        /// <summary>
        /// can redo?
        /// </summary>
        public bool CanRedo { get => (this.redo.Count > 0); }

        /// <summary>
        /// DBG: undo, redo size
        /// </summary>
        private const int max_size = 256;

        public UndoRedoManager4frmScriptBuilder() { }

        /// <summary>
        /// add undo snapshot
        /// </summary>
        /// <param name="script"></param>
        public void AddUndoSnapshot(Script script, InstanceCounterData counter, LineStatesData lineState)
        {
            undo.Push((script, counter, lineState));
        }

        /// <summary>
        /// add redo snapshot
        /// </summary>
        /// <param name="script"></param>
        /// <param name="counter"></param>
        public void AddRedoSnapshot(Script script, InstanceCounterData counter, LineStatesData lineState)
        {
            redo.Push((script, counter, lineState));
        }

        /// <summary>
        /// undo
        /// </summary>
        /// <returns></returns>
        public (Script, InstanceCounterData, LineStatesData) Undo()
        {
            if (CanUndo)
            {
                var ret = undo.Pop();
                return ret;
            }
            else
            {
                return (null, null, null);
            }
        }

        /// <summary>
        /// redo
        /// </summary>
        /// <returns></returns>
        public (Script, InstanceCounterData, LineStatesData) Redo()
        {
            if (CanRedo)
            {
                var ret = redo.Pop();
                return ret;
            }
            else
            {
                return (null, null, null);
            }
        }

        /// <summary>
        /// clear undo, redo data
        /// </summary>
        public void Clear() 
        { 
            undo.Clear();
            redo.Clear(); 
        } 
    }
}
