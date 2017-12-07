using System;
using System.Windows.Forms;

namespace PrintForegroundWindow
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void LogLine(string line = "")
        {
            OutputTextBox.AppendText(line);
            OutputTextBox.AppendText(Environment.NewLine);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set hook
            var foregroundWindowChangedHandler = new WinEventHandler(
                (handle, type, windowHandle, objectId, childId, thread, timeMs) =>
                {
                    // Ignore events from non-windows
                    if (objectId != 0)
                        return;

                    // Get foreground window text and class name
                    var text = NativeMethods.GetWindowText(windowHandle);
                    var className = NativeMethods.GetWindowClassName(windowHandle);

                    // Output to console
                    LogLine($"Handle: {windowHandle}");
                    LogLine($"Text: {text}");
                    LogLine($"Class: {className}");
                    LogLine();
                });
            NativeMethods.SetWinEventHook(0x0003, foregroundWindowChangedHandler);
        }
    }
}