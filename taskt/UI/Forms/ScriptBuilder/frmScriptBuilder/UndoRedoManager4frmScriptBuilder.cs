using System.Collections.Generic;
using taskt.Core.Script;

namespace taskt.UI.Forms
{
    // aliace for Instances Counter dictionary, too long!
    using InstanceCounterData = Dictionary<string, Dictionary<string, Dictionary<string, int>>>;

    /// <summary>
    /// Undo, Redo manager for frmScriptBuilder
    /// </summary>
    internal class UndoRedoManager4frmScriptBuilder
    {
        /// <summary>
        /// undo stack
        /// </summary>
        private readonly Stack<(Script, InstanceCounterData)> undo = new Stack<(Script, InstanceCounterData)>();
        /// <summary>
        /// redo stack
        /// </summary>
        private readonly Stack<(Script, InstanceCounterData)> redo = new Stack<(Script, InstanceCounterData)>();
        
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
        public void AddUndoSnapshot(Script script, InstanceCounterData counter)
        {
            undo.Push((script, counter));
        }

        /// <summary>
        /// add redo snapshot
        /// </summary>
        /// <param name="script"></param>
        /// <param name="counter"></param>
        public void AddRedoSnapshot(Script script, InstanceCounterData counter)
        {
            redo.Push((script, counter));
        }

        /// <summary>
        /// undo
        /// </summary>
        /// <returns></returns>
        public (Script, InstanceCounterData) Undo()
        {
            if (CanUndo)
            {
                var ret = undo.Pop();
                return ret;
            }
            else
            {
                return (null, null);
            }
        }

        /// <summary>
        /// redo
        /// </summary>
        /// <returns></returns>
        public (Script, InstanceCounterData) Redo()
        {
            if (CanRedo)
            {
                var ret = redo.Pop();
                return ret;
            }
            else
            {
                return (null, null);
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
