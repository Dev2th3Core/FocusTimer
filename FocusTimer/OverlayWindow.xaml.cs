using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media;

namespace FocusTimer
{
    public partial class OverlayWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private bool isPaused;

        public OverlayWindow(int minutes)
        {
            InitializeComponent();

            timeLeft = TimeSpan.FromMinutes(minutes);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDisplay();
            SetWindowToWorkArea();

            // Apply click-through style after the window source (HWND) is available
            SourceInitialized += OverlayWindow_SourceInitialized;
        }

        private void OverlayWindow_SourceInitialized(object? sender, EventArgs e)
        {
            EnableClickThrough();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds <= 0)
            {
                timer.Stop();
                TimerText.Text = "TIME UP";
                return;
            }

            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            // Show hours when the remaining time is one hour or more
            if (timeLeft.TotalHours >= 1)
                TimerText.Text = timeLeft.ToString(@"hh\:mm\:ss");
            else
                TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        // Allow external callers to set the remaining minutes
        public void SetRemainingMinutes(int minutes)
        {
            if (minutes < 0) minutes = 0;
            timeLeft = TimeSpan.FromMinutes(minutes);
            // Restart timer if it was stopped and not paused
            if (timer != null && !timer.IsEnabled && !isPaused)
            {
                timer.Start();
            }
            UpdateDisplay();
        }

        // Pause the countdown
        public void PauseTimer()
        {
            if (timer == null) return;
            if (!isPaused)
            {
                timer.Stop();
                isPaused = true;
            }
        }

        // Resume the countdown
        public void ResumeTimer()
        {
            if (timer == null) return;
            if (isPaused)
            {
                // If time already elapsed, do not restart
                if (timeLeft.TotalSeconds > 0)
                    timer.Start();
                isPaused = false;
            }
        }

        // Expose paused state
        public bool IsPaused => isPaused;

        // Allow changing the overlay background brush
        public void SetBackgroundColor(Brush brush)
        {
            if (BackgroundRect != null)
                BackgroundRect.Fill = brush;
        }

        // Allow changing the timer text brush
        public void SetTimerColor(Brush brush)
        {
            if (TimerText != null)
                TimerText.Foreground = brush;
        }

        // Position the TimerText based on selection
        public void SetTimerPosition(string position)
        {
            switch ((position ?? "").ToLowerInvariant())
            {
                case "top":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Center;
                    TimerText.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case "bottom":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Center;
                    TimerText.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case "left":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Left;
                    TimerText.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case "right":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Right;
                    TimerText.VerticalAlignment = VerticalAlignment.Center;
                    break;
                case "top-left":
                case "topleft":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Left;
                    TimerText.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case "top-right":
                case "topright":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Right;
                    TimerText.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case "bottom-left":
                case "bottomleft":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Left;
                    TimerText.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case "bottom-right":
                case "bottomright":
                    TimerText.HorizontalAlignment = HorizontalAlignment.Right;
                    TimerText.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                default:
                    TimerText.HorizontalAlignment = HorizontalAlignment.Center;
                    TimerText.VerticalAlignment = VerticalAlignment.Center;
                    break;
            }
        }

        // Allow changing the timer font size
        public void SetTimerFontSize(double size)
        {
            if (TimerText != null && size > 0)
                TimerText.FontSize = size;
        }

        private void SetWindowToWorkArea()
        {
            // Use the primary screen work area which excludes the taskbar
            var workArea = SystemParameters.WorkArea;
            Left = workArea.Left;
            Top = workArea.Top;
            Width = workArea.Width;
            Height = workArea.Height;
        }

        private void EnableClickThrough()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero)
                return;

            int style = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_LAYERED = 0x80000;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    }
}
