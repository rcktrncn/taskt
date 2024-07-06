using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
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

        public List<Core.Script.ScriptVariable> variablesList { private set; get; }

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

        public async void OK()
        {
            //Todo: figure out why return DialogResult not working for some reason

            var t = GetVariablesFromHTML();
            t.Wait();

            Result = DialogResult.OK;
            this.Close();
        }

        public void Cancel()
        {
            //Todo: figure out why return DialogResult not working for some reason

            Result = DialogResult.Cancel;
            this.Close();
        }

        public async Task GetVariablesFromHTML()
        {
            this.variablesList = new List<Core.Script.ScriptVariable>();

            await GetVariablesFromInput();
        }

        private async Task GetVariablesFromInput()
        {   
            // func id
            var rnd = new Random();
            var func_id = rnd.Next();

            // js code
            var inputJS = @"
function getInputValues_" + func_id + @"() {
    let ret = '[';
    const elems = document.querySelectorAll('input');
    for (let i = 0; i < elems.length; i++) {
        const elem = elems[i];
        const name = elem.getAttribute('v_applyToVariable');
        if (name != '') {
            const attr = elem.getAttribute('type');
            if (attr == 'checkbox') {
                 const c = elem.getAttribute('checked');
                 ret += '{ ""' + name + '"": ""' + c + '"" },';
            }
            else {
                 const v = elem.value;
                 ret += '{ ""' + name + '"": ""' + v + '"" },';
            }
        }
    }
    ret = ret.substring(0, ret.length - 1) + ']';
    return ret;
}" + @"
getInputValues_" + func_id + "()";

            
            // parse result
            var jsonText = await webBrowserHTML.ExecuteScriptAsync(inputJS);
            var ary = JArray.Parse(jsonText);

            Console.WriteLine($"!!! {jsonText}");

            //foreach(JObject item in ary)
            //{
            //    this.variablesList.Add(new Core.Script.ScriptVariable()
            //    {
            //        VariableName = item["name"].ToString(),
            //        VariableValue = item["value"].ToString(),
            //    });
            //}

            AddVariablesList(ary);

            //HtmlElementCollection collection = webBrowserHTML.Document.GetElementsByTagName(tagSearch);
            //for (int i = 0; i < collection.Count; i++)
            //{
            //    var variableName = collection[i].GetAttribute("v_applyToVariable");

            //    if (!string.IsNullOrEmpty(variableName))
            //    {
            //        var parentElement = collection[i];
            //        if (tagSearch == "select")
            //        {
            //            foreach (HtmlElement item in parentElement.Children)
            //            {
            //                if (item.GetAttribute("selected") == "True")
            //                {
            //                    varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = item.InnerText });
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (parentElement.GetAttribute("type") == "checkbox")
            //            {
            //                var inputValue = collection[i].GetAttribute("checked");
            //                varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = inputValue });
            //            }
            //            else
            //            {
            //                var inputValue = collection[i].GetAttribute("value");
            //                varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = inputValue });
            //            }
            //        }
            //    }
            //}
            //return varList;
        }

        private void AddVariablesList(JArray ary)
        {
            foreach(JObject item in ary)
            {
                var name = item["name"].ToString();

                ScriptVariable existsVar = null;
                foreach(var v in variablesList)
                {
                    if (v.VariableName == name)
                    {
                        existsVar = v;
                        break;
                    }
                }
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

        private void webBrowserHTML_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            webBrowserHTML.Enabled = false;
        }

        private void webBrowserHTML_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            webBrowserHTML.Enabled = true;
        }
    }
}
