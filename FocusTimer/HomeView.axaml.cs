using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace FocusTimer.Avalonia
{
    public partial class HomeView : UserControl
    {
        // Events to communicate with MainWindow
        public event EventHandler<int>? StartRequested;
        public event EventHandler? StopRequested;
        public event EventHandler? PauseRequested;
        public event EventHandler<int>? RestartRequested;

        public HomeView()
        {
            InitializeComponent();
        }

        private void PlayButton_Click(object? sender, RoutedEventArgs e)
        {
            var durationPicker = this.FindControl<CircularTimerControl>("DurationPicker");
            if (durationPicker != null && durationPicker.Minutes > 0)
            {
                StartRequested?.Invoke(this, durationPicker.Minutes);
                SetTimerRunningState(true);
            }
        }

        private void StopButton_Click(object? sender, RoutedEventArgs e)
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
            SetTimerRunningState(false);
        }

        private void RestartButton_Click(object? sender, RoutedEventArgs e)
        {
            var durationPicker = this.FindControl<CircularTimerControl>("DurationPicker");
            if (durationPicker != null && durationPicker.Minutes > 0)
            {
                RestartRequested?.Invoke(this, durationPicker.Minutes);
            }
        }

        private void PauseButton_Click(object? sender, RoutedEventArgs e)
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
            
            // Toggle icon
            var pauseBtn = this.FindControl<Button>("PauseButton");
            if (pauseBtn != null)
            {
                pauseBtn.Content = pauseBtn.Content?.ToString() == "⏸" ? "▶" : "⏸";
            }
        }

        public void SetTimerRunningState(bool isRunning)
        {
            var playBtn = this.FindControl<Button>("PlayButton");
            var stopBtn = this.FindControl<Button>("StopButton");
            var pauseBtn = this.FindControl<Button>("PauseButton");
            var restartBtn = this.FindControl<Button>("RestartButton");

            if (playBtn != null) playBtn.IsVisible = !isRunning;
            if (stopBtn != null) stopBtn.IsVisible = isRunning;
            if (restartBtn != null) restartBtn.IsVisible = isRunning;
            if (pauseBtn != null) 
            {
                pauseBtn.IsVisible = isRunning;
                pauseBtn.Content = "⏸"; // Reset to pause icon
            }
        }
    }
}
