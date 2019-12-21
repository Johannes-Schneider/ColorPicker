using System;
using System.Runtime.InteropServices;

namespace ColorPicker.Executable.PInvoke
{
    internal static class Magnification
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct Transform
        {
            private const int OneDimensionLength = 3;

            private fixed float v[OneDimensionLength * OneDimensionLength];

            public float this[int x, int y]
            {
                get => v[y * OneDimensionLength + x];
                set => v[y * OneDimensionLength + x] = value;
            }

            public static Transform Magnify(float factor)
            {
                return new Transform() {[0, 0] = factor, [1, 1] = factor, [2, 2] = 1.0f};
            }
        }

        [DllImport(nameof(Magnification))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MagInitialize();

        [DllImport(nameof(Magnification))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MagUninitialize();

        [DllImport(nameof(Magnification))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MagShowSystemCursor([MarshalAs(UnmanagedType.Bool)] bool fShowCursor);

        [DllImport(nameof(Magnification))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MagSetWindowTransform(IntPtr hWnd, ref Transform pTransform);
        
        [DllImport(nameof(Magnification))]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MagSetWindowSource(IntPtr hWnd, Win32.Rectangle rect);
    }
}