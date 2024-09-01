﻿namespace taskt.Core
{
    public interface IEngineSettings
    {
        /// <summary>
        /// Show Debug Window
        /// </summary>
        bool ShowDebugWindow { get; }

        /// <summary>
        /// Close Debug Window Automatically
        /// </summary>
        bool AutoCloseDebugWindow { get; }

        /// <summary>
        /// Enable DiagnosticLogging
        /// </summary>
        bool EnableDiagnosticLogging { get; }

        /// <summary>
        /// Show Advanced Debug text Output
        /// </summary>
        bool ShowAdvancedDebugOutput { get; }

        /// <summary>
        /// Create Missing Variables
        /// </summary>
        bool CreateMissingVariablesDuringExecution { get; }

        /// <summary>
        /// Track Execution Metrics
        /// </summary>
        bool TrackExecutionMetrics { get; }

        /// <summary>
        /// Variable start marker, Value can be changed during script execution
        /// </summary>
        string VariableStartMarker { get; set; }

        /// <summary>
        /// Variable end marker, Value can be changed during script execution
        /// </summary>
        string VariableEndMarker { get; set; }

        /// <summary>
        /// key to cancel script execution
        /// </summary>
        System.Windows.Forms.Keys CancellationKey { get; }

        /// <summary>
        /// Delay between commands
        /// </summary>
        int DelayBetweenCommands { get; }

        /// <summary>
        /// override existing app instances
        /// </summary>
        bool OverrideExistingAppInstances { get; }

        /// <summary>
        /// auto close messages on server execution
        /// </summary>
        bool AutoCloseMessagesOnServerExecution { get; }

        /// <summary>
        /// auto close debug window on server execution
        /// </summary>
        bool AutoCloseDebugWindowOnServerExecution { get; }

        /// <summary>
        /// auto calculation values, Value can be changed during script execution
        /// </summary>
        bool AutoCalcVariables { get; set; }

        /// <summary>
        /// export intermediate script xml
        /// </summary>
        bool ExportIntermediateXML { get; }

        /// <summary>
        /// use new variable perser
        /// </summary>
        bool UseNewParser { get; }

        /// <summary>
        /// ignore first variable marker in output parameters
        /// </summary>
        bool IgnoreFirstVariableMarkerInOutputParameter { get; }

        /// <summary>
        /// max file counter
        /// </summary>
        int MaxFileCounter { get; }

        /// <summary>
        /// max UIElement inspect depth
        /// </summary>
        int MaxUIElementInpectDepth { get; }

        /// <summary>
        /// max UIElement inspect sibling nodes
        /// </summary>
        int MaxUIElementInspectSiblingNodes { get; }
    }
}
