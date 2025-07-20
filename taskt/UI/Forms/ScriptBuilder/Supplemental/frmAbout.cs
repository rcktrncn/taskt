//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Diagnostics;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmAbout : DialogLikeThemedForm
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        #region form events
        private void frmAbout_Load(object sender, EventArgs e)
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var info = FileVersionInfo.GetVersionInfo(location);
            lblProjectName.Text = info.ProductName;
            lblAppVersion.Text = "version: " + info.ProductVersion;
            lblBuildDate.Text = "build date: " + System.IO.File.GetLastWriteTime(location).ToString("MM.dd.yy hh.mm.ss");
        }
        #endregion
    }
}