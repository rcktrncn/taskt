﻿using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using taskt.Core.IO;
using taskt.Core.Script;


namespace taskt.UI.Forms.ScriptEngine.Supplemental
{
    /* This attribute is required to call C# code from WebView2 */
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public partial class frmHTMLDisplayForm : Form
    {
        public DialogResult Result { get; set; }
        public string TemplateHTML { get; set; }

        public List<ScriptVariable> variablesList { private set; get; }

        public frmHTMLDisplayForm()
        {
            InitializeComponent();
        }

        private async void frmHTMLDisplayForm_Load(object sender, EventArgs e)
        {
            //webBrowserHTML.ScriptErrorsSuppressed = true;
            //webBrowserHTML.ObjectForScripting = this;
            //webBrowserHTML.DocumentText = TemplateHTML;

            var udfPath = Path.Combine(Folders.GetSettingsFolderPath(), "webview2");
            var webView2Environment = await CoreWebView2Environment.CreateAsync(userDataFolder: udfPath);
            await webBrowserHTML.EnsureCoreWebView2Async(webView2Environment);

            webBrowserHTML.CoreWebView2.AddHostObjectToScript("fm", this);

            webBrowserHTML.NavigateToString(TemplateHTML);

            this.TopMost = true;
        }

        private void webBrowserHTML_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            webBrowserHTML.Enabled = false;
        }

        private void webBrowserHTML_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            webBrowserHTML.Enabled = true;
        }

        /// <summary>
        /// Call from WebView2, OK button
        /// </summary>
        public async void OK()
        {
            //Todo: figure out why return DialogResult not working for some reason

            this.variablesList = new List<ScriptVariable>();

            // input tags
            await GetValueAndVariableNameFromInputElements();

            Result = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Call from WebView2, Cancel button
        /// </summary>
        public void Cancel()
        {
            //Todo: figure out why return DialogResult not working for some reason

            Result = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// get value and variable name from Input Elements
        /// </summary>
        /// <returns></returns>
        private async Task GetValueAndVariableNameFromInputElements()
        {
            // func id
            var rnd = new Random();
            var func_id = rnd.Next();

            // js code
            var inputJS = @"
function getInputValues_" + func_id + @"() {
    let ret = [];
    const elems = document.querySelectorAll('input, textarea, select');

    const inputFunc = (elem, name, ary) => {
        const attr = elem.getAttribute('type');
        if (attr == 'checkbox')
        {
            ary.push({ name: name, value: elem.checked });
        }
        else
        {
            ary.push({ name: name, value: elem.value });
        }
    }

    for (let i = 0; i < elems.length; i++)
    {
        const elem = elems[i];
        if (elem.tagName  = 'input') {
            const applyVar = elem.getAttribute('v_applyToVariable');
            if (applyVar != null)
            {
                inputFunc(elem, applyVar, ret);
            }
            const dataVar = elem.getAttribute('data-variable');
            if (dataVar != null) {
                inputFunc(elem, dataVar, ret);
            }
        }
        else {
            const applyVar = elem.getAttribute('v_applyToVariable');
            if (applyVar != null) {
                ret.push({ name: applyVar, value: elem.value });
            }
            const dataVar = elem.getAttribute('data-variable');
            if (dataVar != null) {
                ret.push({ name: dataVar, value: elem.value });
            }
        }
    }
    return JSON.stringify(ret);
}" + @"
getInputValues_" + func_id + "();";

            Console.WriteLine(inputJS);

            var jsonText = await webBrowserHTML.ExecuteScriptAsync(inputJS);

            var parsedJsonText = jsonText.Replace("\\\"", "\"");
            parsedJsonText = parsedJsonText.Substring(1, parsedJsonText.Length - 2);

            var ary = JArray.Parse(parsedJsonText);
            AddVariablesList(ary);
        }

        private void AddVariablesList(JArray ary)
        {
            foreach(JObject item in ary.Cast<JObject>())
            {
                var name = item["name"].ToString();

                var existsVar = variablesList.FirstOrDefault(v => v.VariableName == name);
                if (existsVar != null)
                {
                    existsVar.VariableValue = item["value"].ToString();
                }
                else
                {
                    variablesList.Add(new ScriptVariable()
                    {
                        VariableName = name,
                        VariableValue = item["value"].ToString(),
                    });
                }
            }
        }
    }
}
