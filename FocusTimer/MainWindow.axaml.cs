using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Threading.Tasks;

namespace FocusTimer.Avalonia
{
    public partial class MainWindow : Window
    {
        private OverlayWindow? overlayWindow;
        
        // Views
        private HomeView homeView;
        private SettingsView settingsView;
        private AnalysisView analysisView;

        public MainWindow()
        {
            InitializeComponent();
            
            // Initialize Views
            homeView = new HomeView();
            settingsView = new SettingsView();
            analysisView = new AnalysisView();

            // Subscribe to View Events
            homeView.StartRequested += (s, minutes) => StartTimerOverlay(minutes);
            homeView.StopRequested += (s, e) => StopTimer();
            homeView.PauseRequested += (s, e) => TogglePause();
            homeView.RestartRequested += (s, minutes) => RestartTimer(minutes);
            
            settingsView.ApplyRequested += (s, e) => ApplySettingsToOverlay();

            // Set Default View
            var content = this.FindControl<ContentControl>("PageContent");
            if (content != null) content.Content = homeView;

            this.PropertyChanged += MainWindow_PropertyChanged;
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void Nav_Click(object? sender, RoutedEventArgs e)
        {
            var content = this.FindControl<ContentControl>("PageContent");
            if (content == null || sender is not Button btn) return;

            switch (btn.Name)
            {
                case "NavHome": content.Content = homeView; break;
                case "NavSettings": content.Content = settingsView; break;
                case "NavAnalysis": content.Content = analysisView; break;
            }
        }

        private void HamburgerButton_Click(object? sender, RoutedEventArgs e)
        {
            var splitView = this.FindControl<SplitView>("MainSplitView");
            if (splitView != null) splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void MainWindow_SizeChanged(object? sender, SizeChangedEventArgs e)
        {
            var splitView = this.FindControl<SplitView>("MainSplitView");
            var pageContent = this.FindControl<ContentControl>("PageContent");
            if (splitView == null) return;

            // Tablet/iPad breakpoint (768px)
            if (e.NewSize.Width < 768)
            {
                splitView.DisplayMode = SplitViewDisplayMode.Overlay;
                splitView.CompactPaneLength = 0;
                if (pageContent != null) pageContent.Margin = new Thickness(0);
            }
            else
            {
                splitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                splitView.CompactPaneLength = 48;
                if (pageContent != null) pageContent.Margin = new Thickness(10, 0, 0, 0);
            }
        }

        private void MainWindow_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Window.WindowStateProperty)
            {
                var container = this.FindControl<Border>("MainContainer");
                if (container != null)
                {
                    container.CornerRadius = this.WindowState == WindowState.Maximized 
                        ? new CornerRadius(0) 
                        : new CornerRadius(15);
                }

                if (overlayWindow != null && overlayWindow.IsVisible)
                {
                    if (this.WindowState == WindowState.Normal || this.WindowState == WindowState.Maximized)
                    {
                        this.Topmost = true;
                        this.Activate();
                    }
                    else
                    {
                        this.Topmost = false;
                    }
                }
                else
                {
                    this.Topmost = false;
                }
            }
        }

        private void StopTimer()
        {
            if (overlayWindow != null && overlayWindow.IsVisible)
            {
                overlayWindow.Close();
                overlayWindow = null;
            }

            if (this.WindowState == WindowState.Minimized)
                this.WindowState = WindowState.Normal;
        }

        private void TogglePause()
        {
            if (overlayWindow == null || !overlayWindow.IsVisible) return;

            if (!overlayWindow.IsPaused)
                overlayWindow.PauseTimer();
            else
                overlayWindow.ResumeTimer();
        }

        private void RestartTimer(int minutes)
        {
            if (overlayWindow != null && overlayWindow.IsVisible)
            {
                overlayWindow.SetRemainingMinutes(minutes);
                if (overlayWindow.IsPaused)
                {
                    overlayWindow.ResumeTimer();
                    homeView.SetTimerRunningState(true);
                }
            }
        }

        private void ApplySettingsToOverlay()
        {
            if (overlayWindow == null) return;
            try
            {
                var bg = Color.Parse(settingsView.BackgroundColor);
                var tc = Color.Parse(settingsView.TimerColor);
                
                overlayWindow.SetBackgroundColor(new SolidColorBrush(bg));
                overlayWindow.SetTimerColor(new SolidColorBrush(tc));
                
                if (double.TryParse(settingsView.TimerFontSize, out double fs))
                    overlayWindow.SetTimerFontSize(fs);
                
                overlayWindow.SetTimerPosition(settingsView.Position);
            }
            catch { /* Handle invalid color strings */ }
        }

        private void StartTimerOverlay(int minutes)
        {
            if (overlayWindow != null && overlayWindow.IsVisible) return;

            overlayWindow = new OverlayWindow(minutes);
            overlayWindow.Show();
            overlayWindow.Topmost = true;

            ApplySettingsToOverlay();

            this.WindowState = WindowState.Minimized;

            overlayWindow.Closed += (s, e) =>
            {
                if (this.WindowState == WindowState.Minimized)
                    this.WindowState = WindowState.Normal;
                
                this.Topmost = false;
                overlayWindow = null;
                
                // Reset HomeView UI
                homeView.SetTimerRunningState(false);
            };
        }

        // Window Title Bar Events
        private void Minimize_Click(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        
        private void Close_Click(object? sender, RoutedEventArgs e)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.Shutdown();
            }
            else
            {
                Close();
            }
        }

        private void MaximizeRestore_Click(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        // Drag move logic for custom title bar
        private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                MaximizeRestore_Click(sender, null!);
            }
            else
            {
                BeginMoveDrag(e);
            }
        }
    }
}
