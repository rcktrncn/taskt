using System;

namespace taskt.Core.Script
{
    /// <summary>
    /// Script Information
    /// </summary>
    [Serializable]
    public class ScriptInformation
    {
        public string TasktVersion { get; set; }
        public string Author { get; set; }
        public DateTime LastRunTime { get; set; }
        public uint RunTimes { get; set; }
        public uint Revision { get; set; }
        public string ScriptVersion { get; set; }
        public string Description { get; set; }
        public ScriptInformation()
        {
            this.TasktVersion = "0.0.0.0";
            this.Author = "";
            this.LastRunTime = DateTime.Parse("1990-01-01T00:00:00");
            this.RunTimes = 0;
            this.Revision = 0;
            this.ScriptVersion = "0.0.0";
            this.Description = "";
        }

        public ScriptInformation(ScriptInformation info)
        {
            this.TasktVersion = info.TasktVersion;
            this.Author= info.Author;
            this.LastRunTime = info.LastRunTime;
            this.RunTimes = info.RunTimes;
            this.Revision = info.Revision;
            this.ScriptVersion = info.ScriptVersion;
            this.Description = info.Description;
        }
    }
}
