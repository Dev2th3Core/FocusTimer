using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

namespace FocusTimer
{
    public partial class MainWindow : Window
    {
        private int defaultDuration = 30; // default 30 minutes
        private int currentDuration;

        // Keep a reference to the overlay so we can control its z-order
        private OverlayWindow? overlayWindow;

        // Persisted settings values
        private string persistedBackgroundColor = "#A0000000";
        private string persistedTimerColor = "#AFFFFFFF";
        private string persistedPosition = "Center";
        private string persistedTimerFontSize = "100";

        public MainWindow()
        {
            InitializeComponent();
            currentDuration = defaultDuration;

            // Initialize inline settings inputs using FindName to avoid generated field dependencies
            var sb = FindName("SettingsBackgroundInput") as TextBox;
            var st = FindName("SettingsTimerColorInput") as TextBox;
            var sf = FindName("SettingsTimerFontSizeInput") as TextBox;
            var sp = FindName("SettingsPositionCombo") as ComboBox;

            if (sb != null) sb.Text = persistedBackgroundColor;
            if (st != null) st.Text = persistedTimerColor;
            if (sf != null) sf.Text = persistedTimerFontSize;

            if (sp != null)
            {
                for (int i = 0; i < sp.Items.Count; i++)
                {
                    if (sp.Items[i] is ComboBoxItem item && string.Equals(item.Content.ToString(), persistedPosition, StringComparison.OrdinalIgnoreCase))
                    {
                        sp.SelectedIndex = i;
                        break;
                    }
                }
            }
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

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Toggle settings panel visibility
            var settingsPanel = FindName("SettingsPanel") as FrameworkElement;
            var mainControls = FindName("MainControls") as FrameworkElement;
            var sb = FindName("SettingsBackgroundInput") as TextBox;
            var st = FindName("SettingsTimerColorInput") as TextBox;
            var sf = FindName("SettingsTimerFontSizeInput") as TextBox;
            var sp = FindName("SettingsPositionCombo") as ComboBox;

            if (settingsPanel == null || mainControls == null) return;

            if (settingsPanel.Visibility == Visibility.Visible)
            {
                settingsPanel.Visibility = Visibility.Collapsed;
                mainControls.Visibility = Visibility.Visible;
            }
            else
            {
                // populate current values so user edits preserve others
                if (sb != null) sb.Text = persistedBackgroundColor;
                if (st != null) st.Text = persistedTimerColor;
                if (sf != null) sf.Text = persistedTimerFontSize;
                if (sp != null)
                {
                    for (int i = 0; i < sp.Items.Count; i++)
                    {
                        var item = (ComboBoxItem)sp.Items[i];
                        if (string.Equals(item.Content.ToString(), persistedPosition, StringComparison.OrdinalIgnoreCase))
                        {
                            sp.SelectedIndex = i;
                            break;
                        }
                    }
                }

                settingsPanel.Visibility = Visibility.Visible;
                mainControls.Visibility = Visibility.Collapsed;
            }
        }

        private void SettingsCancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide settings and restore main controls without applying
            var settingsPanel = FindName("SettingsPanel") as FrameworkElement;
            var mainControls = FindName("MainControls") as FrameworkElement;
            if (settingsPanel != null) settingsPanel.Visibility = Visibility.Collapsed;
            if (mainControls != null) mainControls.Visibility = Visibility.Visible;
        }

        private void SettingsApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Read values from inline settings
            var sb = FindName("SettingsBackgroundInput") as TextBox;
            var st = FindName("SettingsTimerColorInput") as TextBox;
            var sf = FindName("SettingsTimerFontSizeInput") as TextBox;
            var sp = FindName("SettingsPositionCombo") as ComboBox;

            if (sb != null) persistedBackgroundColor = sb.Text;
            if (st != null) persistedTimerColor = st.Text;
            if (sf != null) persistedTimerFontSize = sf.Text;
            if (sp != null && sp.SelectedItem is ComboBoxItem posItem) persistedPosition = posItem.Content.ToString();

            // Apply to overlay if running
            try
            {
                var bg = (Color)ColorConverter.ConvertFromString(persistedBackgroundColor);
                var tc = (Color)ColorConverter.ConvertFromString(persistedTimerColor);
                if (overlayWindow != null)
                {
                    overlayWindow.SetBackgroundColor(new SolidColorBrush(bg));
                    overlayWindow.SetTimerColor(new SolidColorBrush(tc));
                    if (double.TryParse(persistedTimerFontSize, out double fs))
                        overlayWindow.SetTimerFontSize(fs);
                    overlayWindow.SetTimerPosition(persistedPosition);
                }
            }
            catch
            {
                MessageBox.Show("Invalid settings values.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            var settingsPanelAfter = FindName("SettingsPanel") as FrameworkElement;
            var mainControlsAfter = FindName("MainControls") as FrameworkElement;
            if (settingsPanelAfter != null) settingsPanelAfter.Visibility = Visibility.Collapsed;
            if (mainControlsAfter != null) mainControlsAfter.Visibility = Visibility.Visible;
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

            // Apply persisted appearance/position to new overlay
            try
            {
                var bgColor = (Color)ColorConverter.ConvertFromString(persistedBackgroundColor);
                var timerColor = (Color)ColorConverter.ConvertFromString(persistedTimerColor);
                overlayWindow.SetBackgroundColor(new SolidColorBrush(bgColor));
                overlayWindow.SetTimerColor(new SolidColorBrush(timerColor));
                overlayWindow.SetTimerPosition(persistedPosition);
                if (double.TryParse(persistedTimerFontSize, out double fs))
                    overlayWindow.SetTimerFontSize(fs);
            }
            catch { }

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
