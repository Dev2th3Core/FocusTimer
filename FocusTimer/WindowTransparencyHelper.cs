using Avalonia;
using Avalonia.Controls;
using System;
using System.Runtime.InteropServices;

namespace FocusTimer.Avalonia
{
    public static class WindowTransparencyHelper
    {
        public static void MakeTransparent(Window window)
        {
            var handle = window.TryGetPlatformHandle()?.Handle ?? IntPtr.Zero;
            if (handle == IntPtr.Zero) return;

            if (OperatingSystem.IsWindows())
            {
                SetWindowsClickThrough(handle);
            }
            // Future: Add macOS/Linux specific logic here
        }

        // --- Windows Specific Implementation ---
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_LAYERED = 0x80000;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        private static void SetWindowsClickThrough(IntPtr hwnd)
        {
            int style = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        }
    }
}