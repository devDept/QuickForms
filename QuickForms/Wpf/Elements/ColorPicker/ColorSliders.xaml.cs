using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QuickForms.Wpf.Elements.ColorPicker.Models;

namespace QuickForms.Wpf.Elements.ColorPicker
{
    /* Color picker code is from https://github.com/PixiEditor/ColorPicker.
     */

    public class PickerControlBase : UserControl, IColorStateStorage
    {
        public static readonly DependencyProperty ColorStateProperty =
            DependencyProperty.Register(nameof(ColorState), typeof(ColorState), typeof(PickerControlBase),
                new PropertyMetadata(new ColorState(0, 0, 0, 1, 0, 0, 0, 0, 0, 0), OnColorStatePropertyChange));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(PickerControlBase),
                new PropertyMetadata(System.Windows.Media.Colors.Black, OnSelectedColorPropertyChange));

        public static readonly RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(ColorChanged),
                RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PickerControlBase));

        public ColorState ColorState
        {
            get => (ColorState)GetValue(ColorStateProperty);
            set => SetValue(ColorStateProperty, value);
        }
        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => 
                SetValue(SelectedColorProperty, value);
        }

        public NotifyableColor Color
        {
            get;
            set;
        }

        private bool ignoreColorPropertyChange = false;
        private bool ignoreColorChange = false;
        private Color previousColor = System.Windows.Media.Color.FromArgb(5, 5, 5, 5);
        public event RoutedEventHandler ColorChanged
        {
            add => AddHandler(ColorChangedEvent, value);
            remove => RemoveHandler(ColorChangedEvent, value);
        }

        public PickerControlBase()
        {
            Color = new NotifyableColor(this);
            Color.PropertyChanged += (sender, args) =>
            {
                var newColor = System.Windows.Media.Color.FromArgb(
                    (byte)Math.Round(Color.A),
                    (byte)Math.Round(Color.RGB_R),
                    (byte)Math.Round(Color.RGB_G),
                    (byte)Math.Round(Color.RGB_B));
                if (newColor != previousColor)
                {
                    RaiseEvent(new ColorRoutedEventArgs(ColorChangedEvent, newColor));
                    previousColor = newColor;
                }
            };
            ColorChanged += (sender, newColor) =>
            {
                if (!ignoreColorChange)
                {
                    ignoreColorPropertyChange = true;
                    SelectedColor = ((ColorRoutedEventArgs)newColor).Color;
                    ignoreColorPropertyChange = false;
                }
            };
        }

        private static void OnColorStatePropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((PickerControlBase)d).Color.UpdateEverything((ColorState)args.OldValue);
        }

        private static void OnSelectedColorPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var sender = (PickerControlBase)d;
            if (sender.ignoreColorPropertyChange)
                return;
            Color newValue = (Color)args.NewValue;
            sender.ignoreColorChange = true;
            sender.Color.A = newValue.A;
            sender.Color.RGB_R = newValue.R;
            sender.Color.RGB_G = newValue.G;
            sender.Color.RGB_B = newValue.B;
            sender.ignoreColorChange = false;
        }
    }
    
    public partial class ColorSliders : PickerControlBase
    {
        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register(nameof(SmallChange), typeof(double), typeof(ColorSliders),
                new PropertyMetadata(1.0));

        public static readonly DependencyProperty ShowAlphaProperty =
            DependencyProperty.Register(nameof(ShowAlpha), typeof(bool), typeof(ColorSliders),
                new PropertyMetadata(true));

        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }

        public bool ShowAlpha
        {
            get => (bool)GetValue(ShowAlphaProperty);
            set => SetValue(ShowAlphaProperty, value);
        }

        public ColorSliders() : base()
        {
            InitializeComponent();
        }
    }
}
