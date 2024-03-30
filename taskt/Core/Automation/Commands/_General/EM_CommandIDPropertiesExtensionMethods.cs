using System;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CommandIDPropertiesExtensionMethods
    {
        /// <summary>
        /// Generate CommandID
        /// </summary>
        /// <param name="command"></param>
        public static void GenerateID(this ICommandIDProperties command)
        {
            var id = Guid.NewGuid();
            command.CommandID = id.ToString();
        }
    }
}
