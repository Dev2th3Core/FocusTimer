using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading.Tasks;

namespace FocusTimer
{
    public partial class MainWindow : Window
    {
        private int defaultDuration = 30; // default 30 minutes
        private int currentDuration;

        // Keep a reference to the overlay so we can control its z-order
        private OverlayWindow? overlayWindow;

        public MainWindow()
        {
            InitializeComponent();
            currentDuration = defaultDuration;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DurationInput.Text, out int minutes) && minutes > 0)
            {
                currentDuration = minutes;
                StartTimerOverlay(currentDuration);

                // Toggle buttons
                PlayButton.Visibility = Visibility.Collapsed;
                StopButton.Visibility = Visibility.Visible;
                PauseButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please enter a valid number of minutes.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (overlayWindow == null || !overlayWindow.IsVisible)
                return;

            if (!overlayWindow.IsPaused)
            {
                overlayWindow.PauseTimer();
                PauseButton.Content = "▶ Resume";
            }
            else
            {
                overlayWindow.ResumeTimer();
                PauseButton.Content = "⏸ Pause";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Close overlay if open
            if (overlayWindow != null && overlayWindow.IsVisible)
            {
                overlayWindow.Close();
                overlayWindow = null;
            }

            // Restore main window
            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;

            // Toggle buttons back
            PlayButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Collapsed;
        }

        // Add 2-second non-blocking delay before applying reset to overlay
        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Set input and internal duration to the value provided by user
            if (int.TryParse(DurationInput.Text, out int minutes) && minutes > 0)
            {
                currentDuration = minutes;
            }
            else
            {
                // if invalid, reset to default
                currentDuration = defaultDuration;
                DurationInput.Text = defaultDuration.ToString();
            }

            // Minimize the main window immediately after resetting
            this.WindowState = WindowState.Minimized;

            // If overlay is running, update remaining time after a short delay
            if (overlayWindow != null && overlayWindow.IsVisible)
            {
                overlayWindow.SetRemainingMinutes(currentDuration);

                // Wait 1 seconds without blocking UI, delay only affects the timer update
                await Task.Delay(1000);
            }
        }

        // Added missing handler referenced from XAML: DurationSlider ValueChanged
        private void DurationSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            // If DurationInput exists, update its text to reflect the slider value
            try
            {
                int minutes = (int)Math.Round(e.NewValue);
                if (DurationInput != null)
                {
                    DurationInput.Text = minutes.ToString();
                }
                currentDuration = minutes;
            }
            catch
            {
                // ignore any errors during design-time or initialization
            }
        }

        private void StartTimerOverlay(int minutes)
        {
            // If an overlay already exists, avoid creating another
            if (overlayWindow != null && overlayWindow.IsVisible)
                return;

            overlayWindow = new OverlayWindow(minutes);
            overlayWindow.Show();

            // Ensure overlay is topmost so it stays above other apps
            overlayWindow.Topmost = true;

            // Minimize the main window when the overlay starts
            this.WindowState = WindowState.Minimized;

            // Monitor state changes so restoring the main window brings it above the overlay
            StateChanged += MainWindow_StateChanged;

            // When the overlay closes, restore main window state if it was minimized and detach handlers
            overlayWindow.Closed += (s, e) =>
            {
                if (this.WindowState == WindowState.Minimized)
                    this.WindowState = WindowState.Normal;

                StateChanged -= MainWindow_StateChanged;
                this.Closed -= OnMainWindowClosed;
                overlayWindow = null;

                // Toggle buttons back when overlay closes
                PlayButton.Visibility = Visibility.Visible;
                StopButton.Visibility = Visibility.Collapsed;
                PauseButton.Visibility = Visibility.Collapsed;
             };

            // Ensure overlay closes when MainWindow closes
            this.Closed += OnMainWindowClosed;
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (overlayWindow == null)
                return;

            var helper = new WindowInteropHelper(this);
            var hwnd = helper.Handle;

            if (this.WindowState == WindowState.Normal || this.WindowState == WindowState.Maximized)
            {
                // Keep overlay topmost relative to other apps
                try
                {
                    overlayWindow.Topmost = true;

                    // Bring main window above other topmost windows (including overlay)
                    if (hwnd != IntPtr.Zero)
                    {
                        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
                        // Do not remove topmost — keep main as topmost while overlay exists so it stays above the overlay
                        SetForegroundWindow(hwnd);
                        this.Activate();
                    }
                }
                catch { }
            }
            else if (this.WindowState == WindowState.Minimized)
            {
                // When minimized, remove main's topmost so overlay remains above all other windows
                try
                {
                    if (hwnd != IntPtr.Zero)
                    {
                        SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
                    }

                    overlayWindow.Topmost = true;
                }
                catch { }
            }
        }

        private void OnMainWindowClosed(object? sender, EventArgs e)
        {
            if (overlayWindow?.IsVisible == true)
                overlayWindow.Close();
        }

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
