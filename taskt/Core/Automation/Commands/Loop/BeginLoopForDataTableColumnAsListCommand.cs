﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop For DataTable Column As List")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by DataTable. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForDataTableColumnAsListCommand : ADataTableGetFromDataTableCommands, IHaveLoopAdditionalCommands
    {
        //[XmlAttribute]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Column Index")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Column Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Column Index")]
        [PropertyParameterOrder(6002)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Column Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Column Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Column Name")]
        [PropertyParameterOrder(6003)]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Reverse Loop")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6005)]
        public string v_ReverseLoop { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Current Loop Times (First Time Value is '1')")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Loop Current Times Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Current Times")]
        [PropertyParameterOrder(6007)]
        public string v_CurrentTimes { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        [PropertyParameterOrder(6008)]
        public string v_NumberOfLoops { get; set; }

        public BeginLoopForDataTableColumnAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForDataTableColumnAsListCommand)parentCommand.ScriptCommand;

            //var rawVariable = v_DataTable.GetRawVariable(engine);
            var tableToLoop = this.ExpandUserVariableAsDataTable(engine);
            var loopTimes = tableToLoop.Columns.Count;

            Action<int> tableColumnIndexAction;
            if (string.IsNullOrEmpty(v_ColumnIndex))
            {
                tableColumnIndexAction = new Action<int>(idx => { });   // nothing
            }
            else
            {
                tableColumnIndexAction = new Action<int>(idx => { idx.StoreInUserVariable(engine, v_ColumnIndex); });
            }

            Action<string> tableColumnNameAction;
            if (string.IsNullOrEmpty(v_ColumnIndex))
            {
                tableColumnNameAction = new Action<string>(n => { });   // nothing
            }
            else
            {
                tableColumnNameAction = new Action<string>(n => { n.StoreInUserVariable(engine, v_ColumnName); });
            }

            Action<int> loopTimesAction;
            if (string.IsNullOrEmpty(v_CurrentTimes))
            {
                loopTimesAction = new Action<int>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }
            else
            {
                loopTimesAction = new Action<int>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                    num.StoreInUserVariable(engine, v_CurrentTimes);
                });
            }

            Action<int> loopNumAction;
            if (string.IsNullOrEmpty(v_NumberOfLoops))
            {
                loopNumAction = new Action<int>(num => { });    // nothing
            }
            else
            {
                loopNumAction = new Action<int>(num => { num.StoreInUserVariable(engine, v_NumberOfLoops); });
            }

            int count = 0;  // loop counter
            var loopBodyProcess = new Action<int>((column) =>
            {
                engine.ReportProgress($"Starting Loop Number {(count + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    // store variables value
                    var convColumnList = new ConvertDataTableColumnToListCommand()
                    {
                        v_DataTable = this.v_DataTable,
                        v_ColumnIndex = column.ToString(),
                        v_ColumnType = "index",
                        v_Result = this.v_Result,
                    };
                    convColumnList.RunCommand(engine);

                    tableColumnIndexAction(column);
                    tableColumnNameAction(tableToLoop.Columns[column].ColumnName);
                    loopTimesAction(count + 1);
                    loopNumAction(count);

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress($"Exiting Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress($"Continuing Next Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }

                engine.ReportProgress($"Finished Loop From Line {loopCommand.LineNumber}");
            });

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReverseLoop), engine))
            {
                for (int i = tableToLoop.Columns.Count - 1; i >= 0; i--)
                {
                    loopBodyProcess(i);
                }
            }
            else
            {
                for (int i = 0; i < tableToLoop.Columns.Count; i++)
                {
                    loopBodyProcess(i);
                }
            }
        }
    }
}