using System;
using System.Runtime.InteropServices;

namespace ColorPicker.Executable.PInvoke
{
    internal static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X { get; }

            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rectangle
        {
            public int Left { get; }

            public int Top { get; }

            public int Right { get; }

            public int Bottom { get; }

            public Rectangle(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }
    }
}