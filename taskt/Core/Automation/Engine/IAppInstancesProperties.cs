using System.Collections.Generic;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// App Instances properties
    /// </summary>
    public interface IAppInstancesProperties
    {
        /// <summary>
        /// App Instances
        /// </summary>
        Dictionary<string, object> AppInstances { get; set; }
    }
}
