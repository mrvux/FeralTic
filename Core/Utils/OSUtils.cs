using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.Utils
{
    public static class OSUtils
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeLibrary(IntPtr hModule);

        private static bool isCompiler47Available;
        private static bool dllCheck47 = false;

        public static bool IsWindows8
        {
            get
            {
                Version win8version = new Version(6, 2, 9200, 0);

                return (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                    Environment.OSVersion.Version >= win8version);
            }
        }

        public static bool IsCompiler47Available
        {
            get
            {
                if (dllCheck47 == false)
                {
                    IntPtr ptr = LoadLibrary("d3dcompiler_47.dll");
                    isCompiler47Available = ptr != IntPtr.Zero;
                    if (ptr != IntPtr.Zero)
                    {
                        FreeLibrary(ptr);
                    }
                    dllCheck47 = true;
                }
                return isCompiler47Available;
            }
        }
    }
}
