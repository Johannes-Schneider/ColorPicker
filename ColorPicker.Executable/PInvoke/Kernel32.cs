using System;
using System.Runtime.InteropServices;

namespace ColorPicker.Executable.PInvoke
{
    internal static class Kernel32
    {
        [DllImport(nameof(Kernel32))]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}