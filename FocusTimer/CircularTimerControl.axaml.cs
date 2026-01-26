using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace FocusTimer.Avalonia
{
    public partial class CircularTimerControl : UserControl
    {
        private bool _isDragging = false;
        private int _minutes = 30;
        private double _previousAngle;
        private double _accumulatedMinutes;
        private const double Radius = 110; // Half of 220 width
        private const double CenterX = 125; // Half of 250 container
        private const double CenterY = 125;

        public static readonly DirectProperty<CircularTimerControl, int> MinutesProperty =
            AvaloniaProperty.RegisterDirect<CircularTimerControl, int>(
                nameof(Minutes),
                o => o.Minutes,
                (o, v) => o.Minutes = v);

        public int Minutes
        {
            get => _minutes;
            set
            {
                if (SetAndRaise(MinutesProperty, ref _minutes, Math.Max(0, value)))
                {
                    UpdateVisuals();
                }
            }
        }

        public CircularTimerControl()
        {
            InitializeComponent();
            UpdateVisuals();
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _isDragging = true;
            var point = e.GetPosition(this.FindControl<Panel>("Container"));
            _previousAngle = GetAngle(point);
            _accumulatedMinutes = Minutes;
            e.Pointer.Capture(this.FindControl<Panel>("Container"));
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            if (_isDragging)
            {
                var point = e.GetPosition(this.FindControl<Panel>("Container"));
                double currentAngle = GetAngle(point);
                double delta = currentAngle - _previousAngle;

                // Handle wrap-around (crossing 12 o'clock)
                if (delta > 180) delta -= 360;
                if (delta < -180) delta += 360;

                _accumulatedMinutes += delta / 6.0; // 360 degrees = 60 minutes
                int newMinutes = (int)Math.Round(_accumulatedMinutes);
                if (newMinutes < 0) newMinutes = 0;

                Minutes = newMinutes;
                _previousAngle = currentAngle;
            }
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            _isDragging = false;
            e.Pointer.Capture(null);
        }

        private double GetAngle(Point p)
        {
            double dx = p.X - CenterX;
            double dy = p.Y - CenterY;

            // Calculate angle in degrees (0 at top, clockwise)
            double angleRad = Math.Atan2(dy, dx);
            double angleDeg = angleRad * 180 / Math.PI;
            
            // Adjust so -90 (top) is 0 degrees
            angleDeg += 90;
            if (angleDeg < 0) angleDeg += 360;

            return angleDeg;
        }

        private void UpdateVisuals()
        {
            // Update Text
            var textBlock = this.FindControl<TextBlock>("TimeText");
            if (textBlock != null) textBlock.Text = _minutes.ToString();

            // Map minutes to 0-60 for visual representation
            double visualMinutes = _minutes == 0 ? 0 : ((_minutes - 1) % 60) + 1;

            // Calculate Angle for Visuals
            double angleDeg = (visualMinutes / 60.0) * 360;
            double angleRad = (angleDeg - 90) * Math.PI / 180;

            // Update Knob Position
            double knobX = CenterX + Radius * Math.Cos(angleRad);
            double knobY = CenterY + Radius * Math.Sin(angleRad);

            var knob = this.FindControl<Ellipse>("Knob");
            if (knob != null)
            {
                Canvas.SetLeft(knob, knobX);
                Canvas.SetTop(knob, knobY);
            }

            // Update Arc
            var arc = this.FindControl<Path>("ActiveArc");
            if (arc != null)
            {
                bool isLargeArc = visualMinutes > 30;
                Point startPoint = new Point(CenterX, CenterY - Radius); // Top center
                Point endPoint = new Point(knobX, knobY);

                if (visualMinutes >= 60)
                    arc.Data = Geometry.Parse($"M {startPoint.X},{startPoint.Y} A {Radius},{Radius} 0 1 1 {startPoint.X - 0.01},{startPoint.Y}");
                else
                    arc.Data = Geometry.Parse($"M {startPoint.X},{startPoint.Y} A {Radius},{Radius} 0 {(isLargeArc ? 1 : 0)} 1 {endPoint.X},{endPoint.Y}");
            }
        }
    }
}