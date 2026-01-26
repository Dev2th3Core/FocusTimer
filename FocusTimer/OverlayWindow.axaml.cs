using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace FocusTimer.Avalonia
{
    public partial class OverlayWindow : Window
    {
        private DispatcherTimer? timer;
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
            
            // Avalonia equivalent of SourceInitialized
            this.Opened += OverlayWindow_Opened;
        }

        // Constructor for XAML previewer
        public OverlayWindow() : this(30) { }

        private void OverlayWindow_Opened(object? sender, EventArgs e)
        {
            SetWindowToWorkArea();
            WindowTransparencyHelper.MakeTransparent(this);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds <= 0)
            {
                timer?.Stop();
                var timerText = this.FindControl<TextBlock>("TimerText");
                if (timerText != null) timerText.Text = "TIME UP";
                return;
            }

            timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var timerText = this.FindControl<TextBlock>("TimerText");
            if (timerText == null) return;

            if (timeLeft.TotalHours >= 1)
                timerText.Text = timeLeft.ToString(@"hh\:mm\:ss");
            else
                timerText.Text = timeLeft.ToString(@"mm\:ss");
        }

        public void SetRemainingMinutes(int minutes)
        {
            if (minutes < 0) minutes = 0;
            timeLeft = TimeSpan.FromMinutes(minutes);
            if (timer != null && !timer.IsEnabled && !isPaused)
            {
                timer.Start();
            }
            UpdateDisplay();
        }

        public void PauseTimer()
        {
            if (timer == null) return;
            if (!isPaused)
            {
                timer.Stop();
                isPaused = true;
            }
        }

        public void ResumeTimer()
        {
            if (timer == null) return;
            if (isPaused)
            {
                if (timeLeft.TotalSeconds > 0)
                    timer.Start();
                isPaused = false;
            }
        }

        public bool IsPaused => isPaused;

        public void SetBackgroundColor(IBrush brush)
        {
            var bgRect = this.FindControl<Border>("BackgroundRect");
            if (bgRect != null) bgRect.Background = brush;
        }

        public void SetTimerColor(IBrush brush)
        {
            var timerText = this.FindControl<TextBlock>("TimerText");
            if (timerText != null) timerText.Foreground = brush;
        }

        public void SetTimerPosition(string position)
        {
            var timerText = this.FindControl<TextBlock>("TimerText");
            if (timerText == null) return;

            // Reset alignment
            timerText.HorizontalAlignment = HorizontalAlignment.Center;
            timerText.VerticalAlignment = VerticalAlignment.Center;

            switch ((position ?? "").ToLowerInvariant())
            {
                case "top": timerText.VerticalAlignment = VerticalAlignment.Top; break;
                case "bottom": timerText.VerticalAlignment = VerticalAlignment.Bottom; break;
                case "left": timerText.HorizontalAlignment = HorizontalAlignment.Left; break;
                case "right": timerText.HorizontalAlignment = HorizontalAlignment.Right; break;
                case "top-left": timerText.HorizontalAlignment = HorizontalAlignment.Left; timerText.VerticalAlignment = VerticalAlignment.Top; break;
                case "top-right": timerText.HorizontalAlignment = HorizontalAlignment.Right; timerText.VerticalAlignment = VerticalAlignment.Top; break;
                case "bottom-left": timerText.HorizontalAlignment = HorizontalAlignment.Left; timerText.VerticalAlignment = VerticalAlignment.Bottom; break;
                case "bottom-right": timerText.HorizontalAlignment = HorizontalAlignment.Right; timerText.VerticalAlignment = VerticalAlignment.Bottom; break;
            }
        }

        public void SetTimerFontSize(double size)
        {
            var timerText = this.FindControl<TextBlock>("TimerText");
            if (timerText != null && size > 0) timerText.FontSize = size;
        }

        private void SetWindowToWorkArea()
        {
            if (Screens.Primary != null)
            {
                var workArea = Screens.Primary.WorkingArea;
                Position = new PixelPoint(workArea.X, workArea.Y);
                Width = workArea.Width;
                Height = workArea.Height;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            timer?.Stop();
            base.OnClosed(e);
        }
    }
}