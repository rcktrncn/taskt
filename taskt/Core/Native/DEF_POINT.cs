using System.Runtime.InteropServices;

namespace taskt.Core.Native
{
    public class DEF_POINT
    {
        /// <summary>
        /// point struct for some APIs
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
    }
}
