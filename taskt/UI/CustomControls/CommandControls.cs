﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using taskt.UI.Forms;
using taskt.Core;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using static taskt.Core.Automation.Commands.PropertyControls;

namespace taskt.UI.CustomControls
{
    public static class CommandControls
    {
        public static Forms.frmCommandEditor CurrentEditor { get; set; }

        public enum CommandControlType
        {
            Body,
            Label,
            Helpers,
            CunstomHelpers,
            SecondLabel
        }

        #region create multi group for
        /// <summary>
        /// create Controls for Render. This method automatically creates all properties controls except "v_Comment". This method supports all attributes.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            //var controlList = command.GetType().GetProperties().Where(
            //        prop => (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
            //    ).Select(prop => prop.Name).ToList();
            var controlList = command.GetParameterProperties().Select(prop => prop.Name).ToList();

            return MultiCreateInferenceDefaultControlGroupFor(controlList, command, editor);
        }

        /// <summary>
        /// create Controls for Render specified by List&lt;string&gt;. This method supports all attributes.
        /// </summary>
        /// <param name="propartiesName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> MultiCreateInferenceDefaultControlGroupFor(List<string> propartiesName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            var controlList = new List<Control>();

            foreach (var propertyName in propartiesName)
            {
                controlList.AddRange(CreateInferenceDefaultControlGroupFor(propertyName, command, editor));
            }

            return controlList;
        }
        #endregion

        #region create inference default control group for
        /// <summary>
        /// create control group. this method use PropertyRecommendedUIControl attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<Control> CreateInferenceDefaultControlGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor)
        {
            var propInfo = command.GetProperty(propertyName);
            //var virtualPropInfo = GetViratulProperty(propInfo);
            var virtualPropInfo = propInfo.GetVirtualProperty();

            var attrRecommended = GetCustomAttributeWithVirtual<PropertyRecommendedUIControl>(propInfo, virtualPropInfo);
            if (attrRecommended != null)
            {
                switch (attrRecommended.recommendedControl)
                {
                    case PropertyRecommendedUIControl.RecommendeUIControlType.TextBox:
                    case PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox:
                        return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);
                        
                    case PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox:
                        return CreateDefaultDropdownGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);

                    case PropertyRecommendedUIControl.RecommendeUIControlType.CheckBox:
                        return CreateDefaultCheckBoxGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);

                    case PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView:
                        return CreateDataGridViewGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);

                    default:
                        return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);
                }
            }
            else
            {
                // check combobox
                var attrUIOpt = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
                var attrIsWin = GetCustomAttributeWithVirtual<PropertyIsWindowNamesList>(propInfo, virtualPropInfo);
                var attrIsVar = GetCustomAttributeWithVirtual<PropertyIsVariablesList>(propInfo, virtualPropInfo);
                var attrInstance = GetCustomAttributeWithVirtual<PropertyInstanceType>(propInfo, virtualPropInfo);
                if ((attrUIOpt.Count > 0) || (attrIsWin != null) || (attrIsVar != null) || (attrInstance != null))
                {
                    return CreateDefaultDropdownGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);
                }
                else
                {
                    return CreateDefaultInputGroupFor(propertyName, command, editor, propInfo, virtualPropInfo);
                }
            }
        }
        #endregion

        #region create default control group for

        /// <summary>
        /// create input control group. this method try use PropertyVirtualProperty attribute, and finally use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultInputGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            if (virtualPropInfo== null)
            {
                virtualPropInfo = propInfo.GetVirtualProperty();
            }
            
            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() =>
            {
                return CreateDefaultInputFor(propertyName, command, editor, propInfo, virtualPropInfo);
            }), editor, propInfo, virtualPropInfo);
        }

        /// <summary>
        /// create combobox control group. this method try use PropertyVirtualProperty attribute, and finally use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultDropdownGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            if (virtualPropInfo == null)
            {
                virtualPropInfo = propInfo.GetVirtualProperty();
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultDropdownFor(propertyName, command, editor, propInfo, virtualPropInfo);
            }), editor, propInfo, virtualPropInfo);
        }

        /// <summary>
        /// create checkbox control group. this method try use PropertyVirtualProperty attribute, and finally use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDefaultCheckBoxGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            if (virtualPropInfo == null)
            {
                virtualPropInfo = propInfo.GetVirtualProperty();
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultCheckBoxFor(propertyName, command, editor, propInfo, virtualPropInfo);
            }), editor, propInfo, virtualPropInfo);
        }

        /// <summary>
        /// create datagridview control group. this method try use PropertyVirtualProperty attribute, and finally use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo"></param>
        /// <returns></returns>
        public static List<Control> CreateDataGridViewGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            if (virtualPropInfo == null)
            {
                virtualPropInfo = propInfo.GetVirtualProperty();
            }

            return CreateDefaultControlGroupFor(propertyName, command, new Func<Control>(() => {
                return CreateDefaultDataGridViewFor(propertyName, command, editor, propInfo, virtualPropInfo);
            }), editor, propInfo, virtualPropInfo);
        }

        /// <summary>
        /// create control group. this method control body is created by method as argument. this method use several attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="createFunc"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property</param>
        /// <returns></returns>
        private static List<Control> CreateDefaultControlGroupFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Func<Control> createFunc, Forms.frmCommandEditor editor, PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        {
            var controlList = new List<Control>();

            // label
            var label = CreateDefaultLabelFor(propertyName, command, editor, propInfo, virtualPropInfo);
            controlList.Add(label);

            // 2nd label
            var attr2ndLabel = GetCustomAttributeWithVirtual<PropertySecondaryLabel>(propInfo, virtualPropInfo);
            if (attr2ndLabel?.useSecondaryLabel ?? false)
            {
                var label2 = CreateSimpleLabel();
                label2.Name = "lbl2_" + propertyName;
                controlList.Add(label2);
            }

            var createdInput = createFunc();

            // ui helper
            controlList.AddRange(CreateDefaultUIHelpersFor(propertyName, command, createdInput, editor, propInfo, virtualPropInfo));

            // custom ui helper
            controlList.AddRange(CreateCustomUIHelpersFor(propertyName, command, createdInput, editor, propInfo, virtualPropInfo));

            // body
            controlList.Add(createdInput);

            return controlList;
        }
        #endregion

        #region create default controls

        #region label

        /// <summary>
        /// create Label from seveal attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use viratul property</param>
        /// <returns></returns>
        public static Control CreateDefaultLabelFor(string propertyName, Core.Automation.Commands.ScriptCommand command, frmCommandEditor editor = null, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var setting = editor?.appSettings ?? CurrentEditor.appSettings;

            var labelText = GetLabelText(propertyName, propInfo, setting, virtualPropInfo);

            // get addtional parameter info
            var attrAdditionalParams = GetCustomAttributesWithVirtual<PropertyAddtionalParameterInfo>(propInfo, virtualPropInfo);
            Dictionary<string, string> addParams = null;
            if (attrAdditionalParams.Count > 0)
            {
                Func<string, string> convFunc;
                if (editor == null)
                {
                    convFunc = new Func<string, string>((str) =>
                    {
                        return str;
                    });
                }
                else
                {
                    convFunc = new Func<string, string>((str) =>
                    {
                        return GetSampleUsageTextForLabel(str, editor.appSettings);
                    });
                }

                addParams = new Dictionary<string, string>();
                foreach (var p in attrAdditionalParams)
                {
                    addParams.Add(p.searchKey, convFunc(p.description));
                }
            }

            return CreateDefaultLabelFor(propertyName, command, labelText, addParams);
        }

        /// <summary>
        /// create Label. This method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="labelText"></param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        public static Control CreateDefaultLabelFor(string propertyName, Core.Automation.Commands.ScriptCommand command, string labelText, Dictionary<string, string> additionalParams = null)
        {
            var inputLabel = CreateSimpleLabel();

            inputLabel.Text = labelText;
            inputLabel.Name = "lbl_" + propertyName;

            if (additionalParams != null)
            {
                inputLabel.Tag = additionalParams;
            }

            return inputLabel;
        }
        #endregion

        #region textbox
        /// <summary>
        /// create TextBox from PropertyTextBoxSetting, PropertyRecommendedUIControl, PropertyFirstValue attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property info</param>
        /// <returns></returns>
        public static Control CreateDefaultInputFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var textBoxSetting = GetCustomAttributeWithVirtual<PropertyTextBoxSetting>(propInfo, virtualPropInfo) ?? new PropertyTextBoxSetting();
            var recommendedControl = GetCustomAttributeWithVirtual<PropertyRecommendedUIControl>(propInfo, virtualPropInfo) ?? new PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextLink);
            var firstValue = GetCustomAttributeWithVirtual<PropertyFirstValue>(propInfo, virtualPropInfo);

            TextBox newTextBox;
            
            int height = textBoxSetting.height;
            if (recommendedControl.recommendedControl == PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)
            {
                height = (height < 2) ? 3 : height; // multi line fix
            }
            newTextBox = (TextBox)CreateDefaultInputFor(propertyName, command, height * 30, 300, textBoxSetting.allowNewLine, (firstValue?.firstValue ?? ""), editor, propInfo);

            return newTextBox;
        }

        /// <summary>
        /// create TextBox and binding property, this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="allowNewLine"></param>
        /// <param name="firstValue">if editor and propInfo is null, firtValue not work.</param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static Control CreateDefaultInputFor(string propertyName, Core.Automation.Commands.ScriptCommand command, int height = 30, int width = 300, bool allowNewLine = true, string firstValue = "", Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null)
        {
            var inputBox = CreateStandardTextBoxFor(propertyName, command);

            // new line setting
            if (!allowNewLine)
            {
                inputBox.KeyDown += (sender, e) => TextBoxKeyDown_DenyEnterNewLine(sender, e);
                inputBox.ScrollBars = ScrollBars.None;
            }
            else
            {
                inputBox.Multiline = true;
                inputBox.ScrollBars = ScrollBars.Vertical;
            }

            inputBox.Height = height;
            inputBox.Width = width;

            // first value
            if ((editor?.creationMode ?? frmCommandEditor.CreationMode.Edit) == frmCommandEditor.CreationMode.Add)
            {
                propInfo?.SetValue(command, editor.appSettings.replaceApplicationKeyword(firstValue));
            }

            return inputBox;
        }
        #endregion

        #region checkbox
        /// <summary>
        /// create CheckBox and binding property, this method try to use PropertyDescription attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property info</param>
        /// <returns></returns>
        public static Control CreateDefaultCheckBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var desc = GetCustomAttributeWithVirtual<PropertyDescription>(propInfo, virtualPropInfo);
            var firstValue = GetCustomAttributeWithVirtual<PropertyFirstValue>(propInfo, virtualPropInfo);
            return CreateDefaultCheckBoxFor(propertyName, command, desc?.propertyDescription ?? "", firstValue?.firstValue ?? "", editor, propInfo);
        }

        /// <summary>
        /// create CheckBox and binding property, this method does not use attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="description"></param>
        /// <param name="firstValue">if editor and propInfo is null, firtValue not work.</param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static Control CreateDefaultCheckBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command, string description, string firstValue = "", Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null)
        {
            var inputBox = CreateStandardCheckboxFor(propertyName, command);
            inputBox.Text = description;

            if ((editor?.creationMode ?? frmCommandEditor.CreationMode.Edit) == frmCommandEditor.CreationMode.Add)
            {
                string convValue = editor.appSettings.replaceApplicationKeyword(firstValue);
                if ((convValue != "") && (bool.TryParse(convValue, out bool b)))
                {
                    propInfo?.SetValue(command, b);
                }
            }

            return inputBox;
        }
        #endregion

        #region combobox
        /// <summary>
        /// create ComboBox and binding property, some events, selection items. this method use PropertyIsWindowNamesList, PropertyIsVariableList, PropertyInstanceType, PropertyUISelectionOption, PropertySelectionChangeEvent attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor">if editor is null, does not support variable, window, instance selection items</param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property info</param>
        /// <returns></returns>
        public static Control CreateDefaultDropdownFor(string propertyName, Core.Automation.Commands.ScriptCommand command, frmCommandEditor editor = null, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var uiOptions = new List<string>();

            // window names list
            var attrIsWin = GetCustomAttributeWithVirtual<PropertyIsWindowNamesList>(propInfo, virtualPropInfo);
            if (attrIsWin?.isWindowNamesList ?? false)
            {
                uiOptions.AddRange(GetWindowNames(editor, attrIsWin.allowCurrentWindow, attrIsWin.allowCurrentWindow, attrIsWin.allowDesktop));
            }

            // variable names list
            var attrIsVar = GetCustomAttributeWithVirtual<PropertyIsVariablesList>(propInfo, virtualPropInfo);
            if (attrIsVar?.isVariablesList ?? false)
            {
                uiOptions.AddRange(GetVariableNames(editor));
            }

            // instance name list
            var attrIsInstance = GetCustomAttributeWithVirtual<PropertyInstanceType>(propInfo, virtualPropInfo);
            if ((attrIsInstance?.instanceType ?? PropertyInstanceType.InstanceType.none) != PropertyInstanceType.InstanceType.none)
            {
                uiOptions.AddRange(GetInstanceNames(editor, attrIsInstance.instanceType));
            }

            // ui options
            var opts = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
            if (opts.Count > 0)
            {
                uiOptions.AddRange(opts.Select(opt => opt.uiOption).ToList());
            }

            var changeEvent = GetCustomAttributeWithVirtual<PropertySelectionChangeEvent>(propInfo, virtualPropInfo);
            var firstValue = GetCustomAttributeWithVirtual<PropertyFirstValue>(propInfo, virtualPropInfo);

            // instance first value
            if ((attrIsInstance?.instanceType ?? PropertyInstanceType.InstanceType.none) != PropertyInstanceType.InstanceType.none)
            {
                if ((uiOptions.Count > 1) && (editor.appSettings.ClientSettings.DontShowDefaultInstanceWhenMultipleItemsExists))
                {
                    firstValue = null;
                }
            }

            return CreateDefaultDropdownFor(propertyName, command, uiOptions, changeEvent?.methodName ?? "", firstValue?.firstValue ?? "", editor, propInfo);
        }

        /// <summary>
        /// create ComboBox and binding property, some events, selection items. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="uiOptions"></param>
        /// <param name="selectionChangeEventName"></param>
        /// <param name="firstValue">if editor and propInfo is null, firtValue not work.</param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <returns></returns>
        public static Control CreateDefaultDropdownFor(string propertyName, Core.Automation.Commands.ScriptCommand command, List<string> uiOptions, string selectionChangeEventName = "", string firstValue = "", Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null)
        {
            var inputBox = CreateStandardComboboxFor(propertyName, command);
            inputBox.Items.AddRange(uiOptions.ToArray());

            if (selectionChangeEventName != "")
            {
                (var trgMethod, var useOuterClassEvent) = GetMethodInfo(selectionChangeEventName, command);

                inputBox.SelectionChangeCommitted += useOuterClassEvent ?
                    (EventHandler)trgMethod.CreateDelegate(typeof(EventHandler)) :
                    (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), command, trgMethod);
            }

            if ((editor?.creationMode ?? frmCommandEditor.CreationMode.Edit) == frmCommandEditor.CreationMode.Add)
            {
                propInfo?.SetValue(command, editor.appSettings.replaceApplicationKeyword(firstValue));
            }

            return inputBox;
        }
        #endregion

        #region DataGridView
        /// <summary>
        /// create DataGridView and binding property, this method use PropertyDataGridViewSetting, PropertyDataGridViewColumnSettings, PropertyDataGridViewCellEditEvent attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropertyInfo">if not null, try use virtual property</param>
        /// <returns></returns>
        public static DataGridView CreateDefaultDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Forms.frmCommandEditor editor = null, PropertyInfo propInfo = null, PropertyInfo virtualPropertyInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            // DataGridView setting
            var dgvSetting = GetCustomAttributeWithVirtual<PropertyDataGridViewSetting>(propInfo, virtualPropertyInfo) ?? new PropertyDataGridViewSetting();
            // columns setting
            var columnSetting = GetCustomAttributesWithVirtual<PropertyDataGridViewColumnSettings>(propInfo, virtualPropertyInfo);
            // events
            var events = GetCustomAttributesWithVirtual<PropertyDataGridViewCellEditEvent>(propInfo, virtualPropertyInfo);

            return CreateDefaultDataGridViewFor(propertyName, command, dgvSetting.allowAddRow, dgvSetting.allowDeleteRow, dgvSetting.allowResizeRow,
                    dgvSetting.width, dgvSetting.height,
                    dgvSetting.autoGenerateColumns, dgvSetting.headerRowHeight,
                    false, columnSetting, events);
        }


        /// <summary>
        /// create DataGridView and binding property, some events, selection items. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="allowAddRows"></param>
        /// <param name="allowDeleteRows"></param>
        /// <param name="allowResizeRows"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="autoGenerateColumns"></param>
        /// <param name="headerRowHeight"></param>
        /// <param name="allowSort"></param>
        /// <param name="columns"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public static DataGridView CreateDefaultDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command, bool allowAddRows = true, bool allowDeleteRows = true, bool allowResizeRows = false, int width = 400, int height = 250, bool autoGenerateColumns = true, int headerRowHeight = 1, bool allowSort = false, List<PropertyDataGridViewColumnSettings> columns = null, List<PropertyDataGridViewCellEditEvent> events = null)
        {
            var propInfo = command.GetProperty(propertyName);

            var dgv = CreateStandardDataGridViewFor(propertyName, command);

            // create DataTable
            if ((columns?.Count ?? 0) > 0)
            {
                var table = CreateDataTable(propertyName, command, dgv, columns);
                propInfo.SetValue(command, table);
            }

            // behavior
            dgv.AllowUserToAddRows = allowAddRows;
            dgv.AllowUserToDeleteRows = allowDeleteRows;
            dgv.AllowUserToResizeRows = allowResizeRows;
            dgv.AutoGenerateColumns = autoGenerateColumns;

            // looks
            if (width < 100)
            {
                width = 400;
            }
            if (height < 100)
            {
                height = 250;
            }

            dgv.Size = new Size(width, height);

            if (headerRowHeight > 1)
            {
                dgv.ColumnHeadersHeight = ((Convert.ToInt32(CurrentEditor.Theme.Datagridview.FontSize) + 15) * headerRowHeight);
            }

            // sort mode
            if ((dgv.Columns.Count > 0) && !allowSort)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            // set events
            if (events != null)
            {
                foreach (var ev in events)
                {
                    (var trgMethod, var useOuterClass) = GetMethodInfo(ev.methodName, command);

                    switch (ev.eventRaise)
                    {
                        case PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick:
                            DataGridViewCellEventHandler clickMethod = (useOuterClass) ?
                                (DataGridViewCellEventHandler)trgMethod.CreateDelegate(typeof(DataGridViewCellEventHandler)) :
                                (DataGridViewCellEventHandler)Delegate.CreateDelegate(typeof(DataGridViewCellEventHandler), command, trgMethod);
                            dgv.CellClick += clickMethod;
                            break;
                        case PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit:
                            DataGridViewCellCancelEventHandler beginEditMethod = (useOuterClass) ?
                                (DataGridViewCellCancelEventHandler)trgMethod.CreateDelegate(typeof(DataGridViewCellCancelEventHandler)) :
                                (DataGridViewCellCancelEventHandler)Delegate.CreateDelegate(typeof(DataGridViewCellCancelEventHandler), command, trgMethod);
                            dgv.CellBeginEdit += beginEditMethod;
                            break;
                    }
                }
            }

            return dgv;
        }
        #endregion

        #region  CustomItemControl(UIHelper)

        /// <summary>
        /// create UIHelpers(CommandItemControl) and binding a event. this method use PropertyUIHelper attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="targetControl"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property</param>
        /// <returns></returns>
        public static List<Control> CreateDefaultUIHelpersFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Control targetControl, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }

            var propertyUIHelpers = GetCustomAttributesWithVirtual<PropertyUIHelper>(propInfo, virtualPropInfo);

            var controlList = new List<Control>();
            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            int count = 0;
            foreach (PropertyUIHelper uiHelper in propertyUIHelpers)
            {
                controlList.Add(CreateDefaultUIHelperFor(propertyName, uiHelper, count, targetControl, editor));

                count++;
            }

            return controlList;
        }

        /// <summary>
        /// create CustomUIHelpers(CommandItemControl) and binding a event. this method use PropertyCustomUIHelper attribute.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="targetControl"></param>
        /// <param name="editor"></param>
        /// <param name="propInfo"></param>
        /// <param name="virtualPropertyInfo">if not null, try use virtual property info</param>
        /// <returns></returns>
        public static List<Control> CreateCustomUIHelpersFor(string propertyName, Core.Automation.Commands.ScriptCommand command, Control targetControl, Forms.frmCommandEditor editor, PropertyInfo propInfo = null, PropertyInfo virtualPropertyInfo = null)
        {
            if (propInfo == null)
            {
                propInfo = command.GetProperty(propertyName);
            }
            List<Control> ctrls = new List<Control>();

            var uiHelpers = GetCustomAttributesWithVirtual<PropertyCustomUIHelper>(propInfo, virtualPropertyInfo);

            if (uiHelpers.Count == 0)
            {
                return ctrls;
            }

            int counter = 0;
            foreach (var uiHelper in uiHelpers)
            {
                ctrls.Add(CreateDefaultCustomUIHelperFor(propertyName, command, uiHelper, counter, targetControl, editor));

                counter++;
            }

            return ctrls;
        }

        /// <summary>
        /// create UIHelper(CommandItemControl) and binding a event. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="setting"></param>
        /// <param name="num"></param>
        /// <param name="targetControl"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static CommandItemControl CreateDefaultUIHelperFor(string propertyName, PropertyUIHelper setting, int num, Control targetControl, Forms.frmCommandEditor editor)
        {
            var uiHelper = CreateSimpleUIHelper(propertyName + "_helper_" + num, targetControl);
            uiHelper.HelperType = setting.additionalHelper;
            switch (setting.additionalHelper)
            {
                case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                    //show variable selector
                    uiHelper.CommandImage = Images.GetUIImage("VariableCommand");
                    uiHelper.CommandDisplay = "Insert Variable";
                    uiHelper.DrawIcon = Properties.Resources.taskt_variable_helper;
                    uiHelper.Click += (sender, e) => ShowVariableSelector(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                    //show file selector
                    uiHelper.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                    uiHelper.CommandDisplay = "Select a File";
                    uiHelper.DrawIcon = Properties.Resources.taskt_file_helper;
                    uiHelper.Click += (sender, e) => ShowFileSelector(sender, e, editor);
                    break;

                case PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper:
                    //show folder selector
                    uiHelper.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                    uiHelper.CommandDisplay = "Select a Folder";
                    uiHelper.DrawIcon = Properties.Resources.taskt_folder_helper;
                    uiHelper.Click += (sender, e) => ShowFolderSelector(sender, e, editor);
                    break;
            }
            return uiHelper;
        }

        /// <summary>
        /// create CustomUIHelper(CommandItemControl) and binding a event. this method does not support attributes. only specify arguments.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="setting"></param>
        /// <param name="num"></param>
        /// <param name="targetControl"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static CommandItemControl CreateDefaultCustomUIHelperFor(string propertyName, Core.Automation.Commands.ScriptCommand command, PropertyCustomUIHelper setting, int num, Control targetControl, Forms.frmCommandEditor editor)
        {
            var uiHelper = CreateSimpleUIHelper(propertyName + "_customhelper_" + (setting.nameKey == "" ? num.ToString() : setting.nameKey), targetControl);
            uiHelper.CommandDisplay = setting.labelText;
            (var trgMethod, var isOuterClass) = GetMethodInfo(setting.methodName, command);

            if (isOuterClass)
            {
                uiHelper.Click += (EventHandler)trgMethod.CreateDelegate(typeof(EventHandler));
            }
            else
            {
                uiHelper.Click += (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), command, trgMethod);
            }

            return uiHelper;
        }
        #endregion

        #endregion

        #region create standard controls

        /// <summary>
        /// create ComboBox and binding property. This methods does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static TextBox CreateStandardTextBoxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var inputBox = new TextBox();
            var theme = CurrentEditor.Theme.Input;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Name = propertyName;
            return inputBox;
        }

        /// <summary>
        /// create ComboBox and binding property and binding some events for cursor position. This methods does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ComboBox CreateStandardComboboxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var inputBox = new ComboBox();
            var theme = CurrentEditor.Theme.Combobox;
            inputBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            inputBox.ForeColor = theme.FontColor;
            inputBox.BackColor = theme.BackColor;
            inputBox.DataBindings.Add("Text", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = propertyName;

            // cursor position events
            inputBox.KeyUp += (sender, e) => ComboBoxKeyUp_SaveCursorPosition(sender, e);
            inputBox.Click += (sender, e) => ComboBoxClick_SaveCursorPosition(sender, e);

            return inputBox;
        }

        /// <summary>
        /// create CheckBox and binding property. This method does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static CheckBox CreateStandardCheckboxFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var checkBox = new CheckBox();

            var theme = CurrentEditor.Theme.Checkbox;
            checkBox.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            checkBox.ForeColor = theme.FontColor;
            checkBox.BackColor = theme.BackColor;
            checkBox.DataBindings.Add("Checked", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);
            checkBox.Name = propertyName;

            return checkBox;
        }

        /// <summary>
        ///  create DataGridView and binding property. This method does not use attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static DataGridView CreateStandardDataGridViewFor(string propertyName, Core.Automation.Commands.ScriptCommand command)
        {
            var theme = CurrentEditor.Theme.Datagridview;
            var dgv = new DataGridView();
            dgv.Font = new Font(theme.Font, theme.FontSize, theme.Style);
            dgv.ForeColor = theme.FontColor;
            dgv.ColumnHeadersHeight = Convert.ToInt32(theme.FontSize) + 20;
            dgv.RowTemplate.Height = Convert.ToInt32(theme.FontSize) + 20;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DataBindings.Add("DataSource", command, propertyName, false, DataSourceUpdateMode.OnPropertyChanged);

            dgv.Name = propertyName;

            return dgv;
        }

        /// <summary>
        /// create CommandItemControl. This method does not use attributes.
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="targetControl"></param>
        /// <returns></returns>
        public static CommandItemControl CreateSimpleUIHelper(string controlName, Control targetControl)
        {
            var theme = CurrentEditor.Theme.UIHelper;
            CommandItemControl helperControl = new CommandItemControl
            {
                Padding = new Padding(10, 0, 0, 0),
                Font = new Font(theme.Font, theme.FontSize, theme.Style),
                ForeColor = theme.FontColor,
                BackColor = theme.BackColor,
                Name = controlName,
                Tag = targetControl
            };

            return helperControl;
        }

        /// <summary>
        /// create Label, this method does not use attributes.
        /// </summary>
        /// <returns></returns>
        public static Label CreateSimpleLabel()
        {
            var theme = CurrentEditor.Theme.Label;
            Label newLabel = new Label
            {
                AutoSize = true,
                Font = new Font(theme.Font, theme.FontSize, theme.Style),
                ForeColor = theme.FontColor,
                BackColor = theme.BackColor
            };
            return newLabel;
        }
        #endregion

        #region create Control support methods

        #region DataGridView methods
        /// <summary>
        /// create/init DataTable and DataGridView columns to specified arguments
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="command"></param>
        /// <param name="dgv"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private static DataTable CreateDataTable(string propertyName, Core.Automation.Commands.ScriptCommand command, DataGridView dgv, List<PropertyDataGridViewColumnSettings> columns = null)
        {
            var table = new DataTable
            {
                TableName = propertyName.Replace("v_", "") + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            // add column
            foreach (var colSetting in columns)
            {
                // DataTable Column
                table.Columns.Add(colSetting.columnName);
                table.Columns[table.Columns.Count - 1].DefaultValue = colSetting.defaultValue;

                // DataGridView Column
                DataGridViewColumn newDGVColumn;
                switch (colSetting.type)
                {
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox:
                        newDGVColumn = new DataGridViewTextBoxColumn();
                        break;
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox:
                        newDGVColumn = new DataGridViewComboBoxColumn();
                        var so = colSetting.comboBoxItems.Split('\n');
                        ((DataGridViewComboBoxColumn) newDGVColumn).Items.AddRange(so);
                        break;
                    case PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox:
                        newDGVColumn = new DataGridViewCheckBoxColumn();
                        break;
                    default:
                        newDGVColumn = new DataGridViewTextBoxColumn();
                        break;
                }
                newDGVColumn.HeaderText = colSetting.headerText;
                newDGVColumn.DataPropertyName = colSetting.columnName;
                newDGVColumn.ReadOnly = colSetting.readOnly;
                dgv.Columns.Add(newDGVColumn);
            }

            return table;
        }
        #endregion

        #region Label methods
        /// <summary>
        /// get text for Label. This method use PropertyDescription, PropertyShowSampleUsageInDescription, PropertyIsOptional attributes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propInfo"></param>
        /// <param name="setting"></param>
        /// <param name="virtualPropertyInfo">if not null, try use virtual property info</param>
        /// <returns></returns>
        public static string GetLabelText(string propertyName, PropertyInfo propInfo, ApplicationSettings setting, PropertyInfo virtualPropertyInfo = null)
        {
            var attrDescription = GetCustomAttributeWithVirtual<PropertyDescription>(propInfo, virtualPropertyInfo) ?? new PropertyDescription(propertyName);

            string labelText = setting.replaceApplicationKeyword(attrDescription.propertyDescription);

            // polite text
            if (setting.ClientSettings.ShowPoliteTextInDescription)
            {
                var lowText = labelText.ToLower();

                if (!Regex.IsMatch(lowText, "^please (select|specify|enter|indicate|input)"))
                {
                    // check "Select" or not
                    var controlType = GetCustomAttributeWithVirtual<PropertyRecommendedUIControl>(propInfo, virtualPropertyInfo)?.recommendedControl ?? PropertyRecommendedUIControl.RecommendeUIControlType.TextBox;
                    bool isSelect = ((controlType == PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox) ||
                                        (controlType == PropertyRecommendedUIControl.RecommendeUIControlType.CheckBox) ||
                                        (controlType == PropertyRecommendedUIControl.RecommendeUIControlType.RadioButton));
                    if (!isSelect)
                    {
                        var uiOpts = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropertyInfo);
                        var isWin = GetCustomAttributeWithVirtual<PropertyIsWindowNamesList>(propInfo, virtualPropertyInfo);
                        var isVar = GetCustomAttributeWithVirtual<PropertyIsVariablesList>(propInfo, virtualPropertyInfo);
                        var ins = GetCustomAttributeWithVirtual<PropertyInstanceType>(propInfo, virtualPropertyInfo);

                        isSelect = (uiOpts.Count > 0) ||
                                    (isWin?.isWindowNamesList ?? false) ||
                                    (isVar?.isVariablesList ?? false) ||
                                    ((ins?.instanceType ?? PropertyInstanceType.InstanceType.none) != PropertyInstanceType.InstanceType.none);
                    }

                    if (Regex.IsMatch(lowText, "^(the|a|an) "))
                    {
                        labelText = "Please " + (isSelect ? "Select" : "Specify") + " " + labelText;
                    }
                    else
                    {
                        labelText = "Please " + (isSelect ? "Select" : "Specify") + " the " + labelText;
                    }
                }
            }

            // show sample usage
            if (setting.ClientSettings.ShowSampleUsageInDescription)
            {
                var attrShowSample = GetCustomAttributeWithVirtual<PropertyShowSampleUsageInDescription>(propInfo, virtualPropertyInfo);
                if (attrShowSample?.showSampleUsage ?? false)
                {
                    if (!labelText.Contains("(ex."))
                    {
                        var sampleText = GetSampleUsageText(propInfo, setting, virtualPropertyInfo);
                        if (sampleText != "")
                        {
                            labelText += " (ex. " + sampleText + ")";
                        }
                    }
                }
            }

            // show optional
            var attrIsOpt = GetCustomAttributeWithVirtual<PropertyIsOptional>(propInfo, virtualPropertyInfo);
            if (attrIsOpt?.isOptional ?? false)
            {
                if (!labelText.Contains("Optional"))
                {
                    labelText = "Optional - " + labelText;
                }

                if ((attrIsOpt.setBlankToValue != "") && (!labelText.Contains("Default is") && (setting.ClientSettings.ShowDefaultValueInDescription)))
                {
                    labelText += " (Default is " + attrIsOpt.setBlankToValue + ")";
                }
            }

            return labelText;
        }

        /// <summary>
        /// get sample usage text to Label. This method use PropertyShowSampleUsageInDescription, PropertyDetailSampleUsage, SampleUsage attributes.
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="setting"></param>
        /// <param name="virtualPropInfo">if not null, try use virtual property info</param>
        /// <param name="planeText">if sample usege text written in MarkDown, return value is plane text.</param>
        /// <returns></returns>
        public static string GetSampleUsageText(PropertyInfo propInfo, ApplicationSettings setting, PropertyInfo virtualPropInfo = null, bool planeText = true)
        {
            var sampleText = "";
            var attrShowSample = GetCustomAttributeWithVirtual<PropertyShowSampleUsageInDescription>(propInfo, virtualPropInfo);
            if (attrShowSample?.showSampleUsage ?? false)
            {
                var attrDetailSamples = propInfo.GetCustomAttributes<PropertyDetailSampleUsage>()
                                            .Where(v => (v.showInDescription))
                                            .ToList();
                // try virtual prop
                if ((attrDetailSamples.Count == 0) && (virtualPropInfo != null))
                {
                    attrDetailSamples = virtualPropInfo.GetCustomAttributes<PropertyDetailSampleUsage>()
                                            .Where(v => (v.showInDescription))
                                            .ToList();
                }

                if (attrDetailSamples.Count > 0)
                {
                    foreach (var d in attrDetailSamples)
                    {
                        sampleText += d.sampleUsage + " or ";
                    }
                    sampleText = sampleText.Trim();
                    sampleText = sampleText.Substring(0, sampleText.Length - 2);
                }

                if (sampleText == "")
                {
                    var attrSample = GetCustomAttributeWithVirtual<SampleUsage>(propInfo, virtualPropInfo);
                    sampleText = attrSample?.sampleUsage ?? "";
                }
            }

            if (planeText)
            {
                return GetSampleUsageTextForLabel(sampleText, setting);
            }
            else
            {
                return setting.replaceApplicationKeyword(sampleText);
            }
        }

        #region keyword md format

        /// <summary>
        /// get SampleUsage text for Label (Description)
        /// </summary>
        /// <param name="sample"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        private static string GetSampleUsageTextForLabel(string sample, ApplicationSettings setting)
        {
            return setting.replaceApplicationKeyword(Markdig.Markdown.ToPlainText(sample).Trim()).Replace(" or ", ", ");
        }
        #endregion
        #endregion

        #region ComboBox methods
        
        /// <summary>
        /// add windows names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static ComboBox AddWindowNames(this ComboBox cbo, Forms.frmCommandEditor editor = null, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            if (cbo == null)
                return null;

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetWindowNames(editor, addCurrentWindow, addAllWindows, addDesktop).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        /// <summary>
        /// add variable names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static ComboBox AddVariableNames(this ComboBox cbo, Forms.frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetVariableNames(editor).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        
        /// <summary>
        /// add Instace names to specified ComboBox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="editor"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static ComboBox AddInstanceNames(this ComboBox cbo, Forms.frmCommandEditor editor, PropertyInstanceType.InstanceType tp)
        {
            if ((cbo == null) || (editor == null))
            {
                return null;
            }

            cbo.BeginUpdate();

            cbo.Items.Clear();

            cbo.Items.AddRange(GetInstanceNames(editor, tp).ToArray());

            cbo.EndUpdate();

            return cbo;
        }

        /// <summary>
        /// get Window Names list
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="addCurrentWindow"></param>
        /// <param name="addAllWindows"></param>
        /// <param name="addDesktop"></param>
        /// <returns></returns>
        public static List<string> GetWindowNames(Forms.frmCommandEditor editor = null, bool addCurrentWindow = true, bool addAllWindows = false, bool addDesktop = false)
        {
            var lst = new List<string>();

            if (addCurrentWindow)
            {
                lst.Add(editor?.appSettings.EngineSettings.CurrentWindowKeyword ?? "Current Window");
            }

            if (addAllWindows)
            {
                lst.Add(editor?.appSettings.EngineSettings.AllWindowsKeyword ?? "All Windows");
            }
            if (addDesktop)
            {
                lst.Add(editor.appSettings.EngineSettings.DesktopKeyword ?? "Desktop");
            }
            lst.AddRange(WindowNameControls.GetAllWindowTitles());

            return lst;
        }

        #region create ComboBox items list
        /// <summary>
        /// get variable names list
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static List<string> GetVariableNames(Forms.frmCommandEditor editor)
        {
            return editor?.scriptVariables.Select(v => v.VariableName).ToList() ?? new List<string>();
        }

        /// <summary>
        /// get instance names list
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static List<string> GetInstanceNames(Forms.frmCommandEditor editor, PropertyInstanceType.InstanceType tp)
        {
            if (editor == null)
            {
                return new List<string>();
            }

            string sortOrder = editor.appSettings.ClientSettings.InstanceNameOrder.ToLower();

            Dictionary<string, int> instanceList = editor.instanceList.getInstanceClone(tp, (sortOrder == "frequency of use"));

            string defInstanceName = "";
            switch (tp)
            {
                case PropertyInstanceType.InstanceType.DataBase:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultDBInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.Excel:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.IE:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.NLG:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultNLGInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.WebBrowser:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.StopWatch:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultStopWatchInstanceName;
                    break;
                case PropertyInstanceType.InstanceType.Word:
                    defInstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
                    break;
            }
            if ((defInstanceName != "") && !instanceList.ContainsKey(defInstanceName))
            {
                instanceList.Add(defInstanceName, 0);
            }

            List<string> sortedInstance;
            switch (editor.appSettings.ClientSettings.InstanceNameOrder.ToLower())
            {
                case "no sorting":
                    sortedInstance = instanceList.Keys.ToList();
                    break;
                case "by name":
                    sortedInstance = instanceList.OrderBy(t => t.Key).Select(v => v.Key).ToList();
                    break;
                case "creation frequently":
                case "frequency of use":
                default:
                    sortedInstance = instanceList.OrderByDescending(t => t.Value).Select(v => v.Key).ToList();
                    break;
            }

            return sortedInstance;
        }

        #endregion

        /// <summary>
        /// get MethodInfo from name. If methodName contains "+", it means use outer class method.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private static (MethodInfo, bool) GetMethodInfo(string methodName, ScriptCommand command)
        {
            bool useOuterClassEvent = methodName.Contains("+");
            MethodInfo trgMethod;
            if (useOuterClassEvent)
            {
                int idx = methodName.IndexOf("+");
                string className = methodName.Substring(0, idx);
                string shortMethodName = methodName.Substring(idx + 1);
                var tp = Type.GetType("taskt.Core.Automation.Commands." + className);
                trgMethod = tp.GetMethod(shortMethodName, BindingFlags.Public | BindingFlags.Static);
            }
            else
            {
                trgMethod = command.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            if (trgMethod == null)
            {
                throw new Exception("Method '" + methodName + "' does not exists. Command: " + command.CommandName);
            }

            return (trgMethod, useOuterClassEvent);
        }

        //private static PropertyInfo GetViratulProperty(PropertyInfo propInfo)
        //{
        //    var attrVP = propInfo.GetCustomAttribute<PropertyVirtualProperty>();
        //    if (attrVP == null)
        //    {
        //        return null;
        //    }
        //    var tp = Type.GetType("taskt.Core.Automation.Commands." + attrVP.className);
        //    return tp.GetProperty(attrVP.propertyName, BindingFlags.Public | BindingFlags.Static);
        //}

        //private static T GetCustomAttributeWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        //    where T : System.Attribute
        //{
        //    return propInfo.GetCustomAttribute<T>() ?? virtualPropInfo?.GetCustomAttribute<T>() ?? null;
        //}

        //private static List<T> GetCustomAttributesWithVirtual<T>(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        //    where T : System.Attribute
        //{
        //    var a = propInfo.GetCustomAttributes<T>().ToList();
        //    if (a.Count == 0)
        //    {
        //        return virtualPropInfo?.GetCustomAttributes<T>().ToList() ?? new List<T>();
        //    }
        //    else
        //    {
        //        return a;
        //    }
        //}
        #endregion

        #region Control event handlers

        /// <summary>
        /// deny create new line when type Enter in TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TextBoxKeyDown_DenyEnterNewLine(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// remember cursor position in ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ComboBoxKeyUp_SaveCursorPosition(object sender, KeyEventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
        }
        /// <summary>
        /// remember cursor position in ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ComboBoxClick_SaveCursorPosition(object sender, EventArgs e)
        {
            ComboBox trg = (ComboBox)sender;
            trg.Tag = trg.SelectionStart;
        }
        #endregion

        #endregion

        #region UIHelper events

        /// <summary>
        /// show Variable Selector form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="editor"></param>
        public static void ShowVariableSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            //get copy of user variables and append system variables, then load to combobox
            var variableList = CurrentEditor.scriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Common.GenerateSystemVariables().Select(f => f.VariableName));

            using (Forms.Supplemental.frmItemSelector newVariableSelector = new Forms.Supplemental.frmItemSelector(variableList))
            {
                if (newVariableSelector.ShowDialog() == DialogResult.OK)
                {
                    //ensure that a variable was actually selected
                    if (newVariableSelector.selectedItem == null)
                    {
                        //return out as nothing was selected
                        MessageBox.Show("There were no variables selected!");
                        return;
                    }

                    //grab the referenced input assigned to the 'insert variable' button instance
                    CommandItemControl inputBox = (CommandItemControl)sender;

                    //load settings
                    var settings = CurrentEditor.appSettings.EngineSettings;

                    if (inputBox.Tag is TextBox targetTextbox)
                    {
                        if (editor.appSettings.ClientSettings.InsertVariableAtCursor)
                        {
                            string str = targetTextbox.Text;
                            int cursorPos = targetTextbox.SelectionStart;
                            string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                            targetTextbox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                            targetTextbox.Focus();
                            targetTextbox.SelectionStart = cursorPos + ins.Length;
                            targetTextbox.SelectionLength = 0;
                        }
                        else
                        {
                            targetTextbox.Text += string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                            targetTextbox.Focus();
                            targetTextbox.SelectionStart = targetTextbox.Text.Length;
                            targetTextbox.SelectionLength = 0;
                        }
                    }
                    else if (inputBox.Tag is ComboBox targetCombobox)
                    {
                        if (editor.appSettings.ClientSettings.InsertVariableAtCursor)
                        {
                            string str = targetCombobox.Text;
                            int cursorPos;
                            if (targetCombobox.Tag == null)
                            {
                                targetCombobox.Tag = 0;
                            }
                            if (!int.TryParse(targetCombobox.Tag.ToString(), out cursorPos))
                            {
                                cursorPos = str.Length;
                            }
                            string ins = string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                            targetCombobox.Text = str.Substring(0, cursorPos) + ins + str.Substring(cursorPos);
                            targetCombobox.Focus();
                            targetCombobox.SelectionStart = cursorPos + ins.Length;
                            targetCombobox.SelectionLength = 0;
                        }
                        else
                        {
                            targetCombobox.Text += string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                            targetCombobox.Focus();
                            targetCombobox.SelectionStart = targetCombobox.Text.Length;
                            targetCombobox.SelectionLength = 0;
                        }
                    }
                    else if (inputBox.Tag is DataGridView targetDGV)
                    {
                        if (targetDGV.SelectedCells.Count == 0)
                        {
                            MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!(targetDGV.SelectedCells[0] is DataGridViewTextBoxCell))
                        {
                            MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                        {
                            if (targetDGV.Tag == null)
                            {
                                MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else if (targetDGV.Tag.ToString() != "column-a-editable")
                            {
                                MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        var source = (DataTable)targetDGV.DataSource;
                        var rowIndex = targetDGV.SelectedCells[0].RowIndex;
                        var colIndex = targetDGV.SelectedCells[0].ColumnIndex;
                        if (source.Rows.Count == targetDGV.SelectedCells[0].RowIndex)
                        {
                            source.Rows.Add(source.NewRow());
                        }
                        var targetCell = targetDGV.Rows[rowIndex].Cells[colIndex];
                        targetCell.Value += string.Concat(settings.VariableStartMarker, newVariableSelector.selectedItem.ToString(), settings.VariableEndMarker);
                    }
                }
            }
        }

        /// <summary>
        /// show File Selector form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="editor"></param>
        private static void ShowFileSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetControlValue((Control)((CommandItemControl)sender).Tag, ofd.FileName);
                }
            }
        }

        /// <summary>
        /// show Folder Selector form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="editor"></param>
        private static void ShowFolderSelector(object sender, EventArgs e, Forms.frmCommandEditor editor)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    SetControlValue((Control)((CommandItemControl)sender).Tag, fbd.SelectedPath);
                }
            }
        }

        private static void SetControlValue(Control targetControl, string newValue)
        {
            if (targetControl is TextBox targetText)
            {
                targetText.Text = newValue;
            }
            else if (targetControl is ComboBox targetCombo)
            {
                targetCombo.Text = newValue;
            }
            else if (targetControl is DataGridView targetDGV)
            {
                targetDGV.CurrentCell.Value = newValue;
            }
        }

        #endregion

        public static List<AutomationCommand> GenerateCommandsandControls()
        {
            var commandList = new List<AutomationCommand>();

            var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                                 .Where(t => t.Namespace == "taskt.Core.Automation.Commands")
                                 .Where(t => t.Name != "ScriptCommand")
                                 .Where(t => t.IsAbstract == false)
                                 .Where(t => t.BaseType.Name == "ScriptCommand")
                                 .ToList();


            var userPrefs = new ApplicationSettings().GetOrCreateApplicationSettings();

            //Loop through each class
            foreach (var commandClass in commandClasses)
            {
                var groupingAttribute = commandClass.GetCustomAttributes(typeof(Core.Automation.Attributes.ClassAttributes.Group), true);
                string groupAttribute = "";
                if (groupingAttribute.Length > 0)
                {
                    var attributeFound = (Core.Automation.Attributes.ClassAttributes.Group)groupingAttribute[0];
                    groupAttribute = attributeFound.groupName;
                }
                var subGroupAttr = (Core.Automation.Attributes.ClassAttributes.SubGruop)commandClass.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.SubGruop));
                string subGroupName = (subGroupAttr != null) ? subGroupAttr.subGruopName : "";

                //Instantiate Class
                Core.Automation.Commands.ScriptCommand newCommand = (Core.Automation.Commands.ScriptCommand)Activator.CreateInstance(commandClass);

                //If command is enabled, pull for display and configuration
                if (newCommand.CommandEnabled)
                {
                    var newAutomationCommand = new AutomationCommand();
                    newAutomationCommand.CommandClass = commandClass;
                    newAutomationCommand.Command = newCommand;
                    newAutomationCommand.DisplayGroup = groupAttribute;
                    newAutomationCommand.DisplaySubGroup = subGroupName;
                    newAutomationCommand.FullName = string.Join(" - ", groupAttribute, newCommand.SelectionName);
                    newAutomationCommand.ShortName = newCommand.SelectionName;

                    if (userPrefs.ClientSettings.PreloadBuilderCommands)
                    {
                        //newAutomationCommand.RenderUIComponents();
                    }

                    //call RenderUIComponents to render UI controls              
                    commandList.Add(newAutomationCommand);

                }
            }

            return commandList;

        }

        #region search control methods
        public static List<Control> GetControlsByName(this List<Control> ctrls, string parameterName, CommandControlType t = CommandControlType.Body)
        {
            List<Control> ret = new List<Control>();

            switch (t)
            {
                case CommandControlType.Body:
                    ret.Add(ctrls.Where(c => (c.Name == parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.Label:
                    ret.Add(ctrls.Where(c => (c.Name == "lbl_" + parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.SecondLabel:
                    ret.Add(ctrls.Where(c => (c.Name == "lbl2_" + parameterName)).FirstOrDefault());
                    break;

                case CommandControlType.Helpers:
                    ret.AddRange(ctrls.Where(c => (c.Name.StartsWith(parameterName + "_helper_"))).ToArray());
                    break;

                case CommandControlType.CunstomHelpers:
                    ret.AddRange(ctrls.Where(c => (c.Name.StartsWith(parameterName + "_customhelper_"))).ToArray());
                    break;
            }

            return ret;
        }

        public static List<Control> GetControlGroup(this List<Control> ctrls, string parameterName, string nextParameterName = "")
        {
            List<Control> ret = new List<Control>();

            int index = ctrls.FindIndex(t => (t.Name == "lbl_" + parameterName));
            int last = (nextParameterName == "") ? ctrls.Count : ctrls.FindIndex(t => (t.Name == "lbl_" + nextParameterName));

            for (int i = index; i < last; i++)
            {
                ret.Add(ctrls[i]);
            }

            return ret;
        }

        public static Control GetPropertyControl(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey(propertyName))
            {
                return controls[propertyName];
            }
            else
            {
                throw new Exception("Control '" + propertyName + "' does not exists.");
            }
        }
        public static Label GetPropertyControlLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey("lbl_" + propertyName))
            {
                return (Label)controls["lbl_" + propertyName];
            }
            else
            {
                throw new Exception("Label 'lbl_" + propertyName + "' does not exists.");
            }
        }
        public static Label GetPropertyControl2ndLabel(this Dictionary<string, Control> controls, string propertyName)
        {
            if (controls.ContainsKey("lbl2_" + propertyName))
            {
                return (Label)controls["lbl2_" + propertyName];
            }
            else
            {
                throw new Exception("2nd Label 'lbl2_" + propertyName + "' does not exists.");
            }
        }
        public static (Control body, Label label, Label label2nd) GetAllPropertyControl(this Dictionary<string, Control> controls, string propertyName, bool throwWhenLabelNotExists = true, bool throwWhen2ndLabelNotExists = false)
        {
            Control body = controls.GetPropertyControl(propertyName);

            Label label;
            try
            {
                label = controls.GetPropertyControlLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhenLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label = null;
                }
            }
            Label label2nd;
            try
            {
                label2nd = controls.GetPropertyControl2ndLabel(propertyName);
            }
            catch (Exception ex)
            {
                if (throwWhen2ndLabelNotExists)
                {
                    throw ex;
                }
                else
                {
                    label2nd = null;
                }
            }
            return (body, label, label2nd);
        }
        #endregion
    }



    public class AutomationCommand
    {
        public Type CommandClass { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string DisplayGroup { get; set; }
        public string DisplaySubGroup { get; set; }
        public Core.Automation.Commands.ScriptCommand Command { get; set; }
        public List<Control> UIControls { get; set; }
        public void RenderUIComponents(Forms.frmCommandEditor editorForm)
        {
            if (Command == null)
            {
                throw new InvalidOperationException("Command cannot be null!");
            }

            UIControls = new List<Control>();
            if (Command.CustomRendering)
            {

                var renderedControls = Command.Render(editorForm);

                if (renderedControls.Count == 0)
                {
                    var label = new Label();
                    var theme = editorForm.Theme.ErrorLabel;
                    //label.ForeColor = Color.Red;
                    //label.AutoSize = true;
                    //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                    label.AutoSize = true;
                    label.ForeColor = theme.FontColor;
                    label.BackColor = theme.BackColor;
                    label.Text = "No Controls are defined for rendering!  If you intend to override with custom controls, you must handle the Render() method of this command!  If you do not wish to override with your own custom controls then set 'CustomRendering' to False.";
                    UIControls.Add(label);
                }
                else
                {
                    foreach (var ctrl in renderedControls)
                    {
                        UIControls.Add(ctrl);
                    }

                    //generate comment command if user did not generate it
                    var commentControlExists = renderedControls.Any(f => f.Name == "v_Comment");

                    if (!commentControlExists)
                    {
                        UIControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", Command));
                        UIControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", Command, 100, 300));
                    }

                }


            }
            else
            {

                var label = new Label();
                var theme = editorForm.Theme.ErrorLabel;
                //label.ForeColor = Color.Red;
                //label.AutoSize = true;
                //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                label.AutoSize = true;
                label.ForeColor = theme.FontColor;
                label.BackColor = theme.BackColor;
                label.Text = "Command not enabled for custom rendering!";
                UIControls.Add(label);
            }
        }
        public void Bind(Forms.frmCommandEditor editor)
        {
            //preference to preload is false
            //if (UIControls is null)
            //{
            this.RenderUIComponents(editor);
            //}

            foreach (var ctrl in UIControls)
            {

                if (ctrl.DataBindings.Count > 0)
                {
                    var newBindingList = new List<Binding>();
                    foreach (Binding binding in ctrl.DataBindings)
                    {
                        newBindingList.Add(new Binding(binding.PropertyName, Command, binding.BindingMemberInfo.BindingField, false, DataSourceUpdateMode.OnPropertyChanged));
                    }

                    ctrl.DataBindings.Clear();

                    foreach (var newBinding in newBindingList)
                    {
                        ctrl.DataBindings.Add(newBinding);
                    }
                }

                if (ctrl is CommandItemControl)
                {
                    var control = (CommandItemControl)ctrl;
                    switch (control.HelperType)
                    {
                        case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                            control.DataSource = editor.scriptVariables;
                            break;
                        default:
                            break;
                    }
                }

                //if (ctrl is UIPictureBox)
                //{

                //    var typedControl = (UIPictureBox)InputControl;

                //}

                //Todo: helper for loading variables, move to attribute
                if ((ctrl.Name == "v_userVariableName") && (ctrl is ComboBox))
                {
                    var variableCbo = (ComboBox)ctrl;
                    variableCbo.Items.Clear();
                    foreach (var var in editor.scriptVariables)
                    {
                        variableCbo.Items.Add(var.VariableName);
                    }
                }

            }
        }
    }
}
