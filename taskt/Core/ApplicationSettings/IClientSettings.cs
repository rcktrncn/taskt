namespace taskt.Core
{
    public interface IClientSettings
    {
        bool AntiIdleWhileOpen { get; }
        string RootFolder { get; }
        bool MinimizeToTray { get; }
        string AttendedTasksFolder { get; }
        string StartupMode { get; }
        bool PreloadBuilderCommands { get; }
        bool UseSlimActionBar { get; }
        bool InsertCommandsInline { get; }
        bool EnableSequenceDragDrop { get; }
        bool InsertVariableAtCursor { get; }
        bool InsertElseAutomatically { get; }
        bool InsertCommentIfLoopAbove { get; }
        bool GroupingBySubgroup { get; }
        bool DontShowValidationMessage { get; }
        bool ShowPoliteTextInDescription { get; }
        bool ShowSampleUsageInDescription { get; }
        bool ShowDefaultValueInDescription { get; }
        bool ShowIndentLine { get; }
        bool ShowScriptMiniMap { get; }

        int IndentWidth{ get; }
        string DefaultBrowserInstanceName { get; }
        string DefaultStopWatchInstanceName { get; }
        string DefaultExcelInstanceName { get; }
        string DefaultWordInstanceName { get; }
        string DefaultDBInstanceName { get; }
        string DefaultNLGInstanceName { get; }

        string InstanceNameOrder { get; }

        bool DontShowDefaultInstanceWhenMultipleItemsExists { get; }

        bool SearchTargetGroupName { get; }
        bool SearchTargetSubGroupName { get; }
        bool SearchGreedlyGroupName { get; }
        bool SearchGreedlySubGroupName { get; }

        bool ShowCommandSearchBar { get; }

        bool HideNotifyAutomatically { get; }

        bool RememberCommandEditorSizeAndPosition { get; }

        bool RememberSupplementFormsForCommandEditorPosition { get; }

        bool CheckForUpdateAtStartup { get; }
        bool SkipBetaVersionUpdate { get; }

        bool EnabledAutoSave { get; }

        int AutoSaveInterval { get; }

        int RemoveAutoSaveFileDays { get; }

        int RemoveRunWithtoutSavingFileDays { get; }

        int RemoveBeforeConvertedFileDays { get; }

        bool SupportIECommand { get; }

        bool ChangeItemsWithWheelWhenNotForcused { get; }

        bool DisplayNumberBeforeParameterDescription { get; }
    }
}
