using System;
using System.Runtime.InteropServices;
using System.Text;

namespace PrintForegroundWindow
{
    public delegate void WinEventHandler(
        IntPtr hookHandle, uint eventType, IntPtr windowHandle,
        int objectId, int childId, uint eventThread,
        uint eventTimeMs);

    public static class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "SetWinEventHook", SetLastError = true)]
        private static extern IntPtr SetWinEventHookInternal(
            uint eventMin, uint eventMax,
            IntPtr eventHandlerAssemblyHandle, WinEventHandler eventHandler,
            uint processId, uint threadId, uint flags);

        [DllImport("user32.dll", EntryPoint = "UnhookWinEvent", SetLastError = true)]
        private static extern bool UnsetWinEventHookInternal(IntPtr hookHandle);

        [DllImport("user32.dll", EntryPoint = "GetClassName", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetWindowClassNameInternal(IntPtr windowHandle, StringBuilder buffer, int maxLength);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextInternal(IntPtr windowHandle, StringBuilder buffer, int maxLength);

        public static IntPtr SetWinEventHook(uint eventId, WinEventHandler handler)
        {
            return SetWinEventHookInternal(eventId, eventId, IntPtr.Zero, handler, 0, 0, 0);
        }

        public static void UnsetWinEventHook(IntPtr handle)
        {
            UnsetWinEventHookInternal(handle);
        }

        public static string GetWindowClassName(IntPtr handle)
        {
            const int length = 512;
            var sb = new StringBuilder(length);
            GetWindowClassNameInternal(handle, sb, length);
            return sb.ToString();
        }

        public static string GetWindowText(IntPtr handle)
        {
            const int length = 512;
            var sb = new StringBuilder(length);
            GetWindowTextInternal(handle, sb, length);
            return sb.ToString();
        }
    }
}