using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;

namespace FocusTimer.Avalonia
{
    public partial class SettingsView : UserControl
    {
        public event EventHandler? ApplyRequested;
        public event EventHandler? CancelRequested;

        // Default settings constants
        private const string DefaultBackgroundColor = "#40000000";
        private const string DefaultTimerColor = "#AFFFFFFF";
        private const double DefaultTimerFontSize = 100;
        private const int DefaultPositionIndex = 0; // "Center"

        public SettingsView()
        {
            InitializeComponent();
        }

        private void SettingsApplyButton_Click(object? sender, RoutedEventArgs e)
        {
            ApplyRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SettingsCancelButton_Click(object? sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SettingsResetButton_Click(object? sender, RoutedEventArgs e)
        {
            // Reset controls to default values
            this.FindControl<ColorPicker>("SettingsBackgroundPicker")!.Color = Color.Parse(DefaultBackgroundColor);
            this.FindControl<ColorPicker>("SettingsTimerColorPicker")!.Color = Color.Parse(DefaultTimerColor);
            this.FindControl<Slider>("SettingsTimerFontSizeInput")!.Value = DefaultTimerFontSize;
            this.FindControl<ComboBox>("SettingsPositionCombo")!.SelectedIndex = DefaultPositionIndex;
        }

        public void LoadSettings(string backgroundColor, string timerColor, string timerFontSize, string position)
        {
            // Find controls
            var bgPicker = this.FindControl<ColorPicker>("SettingsBackgroundPicker");
            var timerPicker = this.FindControl<ColorPicker>("SettingsTimerColorPicker");
            var fontSlider = this.FindControl<Slider>("SettingsTimerFontSizeInput");
            var posCombo = this.FindControl<ComboBox>("SettingsPositionCombo");

            // Set values
            if (bgPicker != null) bgPicker.Color = Color.Parse(backgroundColor);
            if (timerPicker != null) timerPicker.Color = Color.Parse(timerColor);
            if (fontSlider != null && double.TryParse(timerFontSize, out var fontSize))
            {
                fontSlider.Value = fontSize;
            }
            if (posCombo != null)
            {
                // Find the index for the given position string
                for (int i = 0; i < posCombo.Items.Count; i++)
                {
                    if (posCombo.Items[i] is ComboBoxItem item && string.Equals(item.Content?.ToString(), position, StringComparison.OrdinalIgnoreCase))
                    {
                        posCombo.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        // Public properties to access the values
        public string BackgroundColor => this.FindControl<ColorPicker>("SettingsBackgroundPicker")?.Color.ToString() ?? "#40000000";
        public string TimerColor => this.FindControl<ColorPicker>("SettingsTimerColorPicker")?.Color.ToString() ?? "#AFFFFFFF";
        public string TimerFontSize => this.FindControl<Slider>("SettingsTimerFontSizeInput")?.Value.ToString() ?? "100";
        public string Position 
        {
            get 
            {
                var combo = this.FindControl<ComboBox>("SettingsPositionCombo");
                if (combo?.SelectedItem is ComboBoxItem item)
                    return item.Content?.ToString() ?? "Center";
                return "Center";
            }
        }
    }
}
