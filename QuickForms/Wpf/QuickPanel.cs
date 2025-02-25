using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using QuickForms.Core;
using QuickForms.Wpf.Elements;
using QuickForms.Wpf.Elements.ColorPicker;

namespace QuickForms.Wpf
{
    internal sealed class QuickPanel : Border, IQuickUI
    {
        public QuickOptions Options { get; set; }

        private readonly StackPanel _panel;

        // Maximum width of the first column to ensure consistent layout
        private double _maxWidth = 0;

        public new double Padding
        {
            get => base.Padding.Top;
            set => base.Padding = new Thickness(value);
        }
        
        public QuickPanel()
        {
            Options = QuickOptions.Default.Clone();
            
            _panel = new StackPanel();
            Child = _panel;

            Padding = Options.DefaultPadding;
        }

        private void Add(FrameworkElement element)
        {
            Thickness margin = new Thickness(0);

            if (_panel.Children.Count > 0)
                // add spacing between this element and the previous one
                margin.Top = Options.VerticalSpacing;

            element.Margin = margin;

            _panel.Children.Add(element);
        }

        private void Add(Label label, FrameworkElement element)
        {
            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Grid.SetColumn(label, 0);
            Grid.SetColumn(element, 1);

            grid.Children.Add(label);
            grid.Children.Add(element);

            Add(grid);

            // Force a layout update to ensure the Grid column width is correctly calculated.
            _panel.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            _panel.Arrange(new Rect(_panel.DesiredSize));
            _panel.UpdateLayout();

            if (grid.ColumnDefinitions[0].ActualWidth <= _maxWidth)
                grid.ColumnDefinitions[0].Width = new GridLength(_maxWidth);
            else
            {
                _maxWidth = grid.ColumnDefinitions[0].ActualWidth;
                foreach (Grid child in _panel.Children)
                {
                    child.ColumnDefinitions[0].Width = new GridLength(_maxWidth);
                }
            }
        }

        private void Add(string? label, FrameworkElement element)
        {
            if (label != null)
            {
                var lab = new Label
                {
                    Content = label,
                    Height = Options.ComponentHeight,
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                Add(lab, element);
            }
            else
            {
                Add(element);
            }
        }

        public Parameter<bool> AddCheckBox(string? label = null, Action<bool>? function = null)
        {
            var checkbox = new CheckBox
            {
                Height = Options.ComponentHeight,
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            
            Add(label, checkbox);

            Parameter<bool> parameter = (WpfBooleanParameter) checkbox;

            if (function != null)
                parameter.Change(function);
            
            return parameter;
        }

        public Parameter<string> AddTextBox(string? label = null, Action<string>? function = null)
        {
            var tb = new TextBox
            {
                Height = Options.ComponentHeight,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            Add(label, tb);

            Parameter<string> p = (WpfStringParameter) tb;

            if(function != null)
                p.Change(function);

            return p;
        }

        public Parameter<double> AddTrackBar(string? label, double min, double max, double? step = null,
            Action<double>? function = null)
        {
            var track = new QuickSlider(min, max)
            {
                Height = Options.ComponentHeight,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            if (step != null)
            {
                track.TickFrequency = step.Value;
                track.IsSnapToTickEnabled = true;
            }

            Add(label, track);
            
            Parameter<double> p = (WpfDoubleParameter) track;

            if(function != null) p.Change(function);

            return p;
        }

        public Parameter<T> AddComboBox<T>(string? label, IEnumerable<T> values, Action<T>? function = null)
        {
            var cb = new ComboBox
            {
                Height = Options.ComponentHeight,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            foreach (var val in values)
                cb.Items.Add(val);

            Add(label, cb);
            
            Parameter<T> p = (WpfParameter<T>) cb;

            if(function != null) p.Change(function);

            return p;
        }
        
        public void AddButton(string? text, Action function)
        {
            Button btn = new Button
            {
                Content = text,
                Height = Options.ComponentHeight
            };

            btn.Click += (_, _) => { function(); };

            Add(btn);
        }

        public Parameter<Color> AddColorPicker(string? label, Color? color, Action<Color>? function)
        {
            var c = color ?? Color.Red;

            var cp = new ColorPicker
            {
                Height = Options.ComponentHeight,
                ActiveColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B)
            };

            Add(label, cp);

            Parameter<Color> parameter = (WpfColorParameter) cp;

            if(function != null)
                parameter.Change(function);

            return parameter;
        }

        public IQuickUI AddCategory(string? title = null)
        {
            Category cat = new Category
            {
                TitleVisibility = Visibility.Collapsed,
            };

            if (title != null)
            {
                cat.MinTitleHeight = Options.ComponentHeight;
                cat.Title = title;
                cat.TitleVisibility = Visibility.Visible;
            }

            cat.QuickPanel.Padding = 10;

            Add(cat);

            return cat.QuickPanel;
        }

        public IQuickUI[] Split(int n = 2)
        {
            Grid grid = new Grid();
            IQuickUI[] columns = new IQuickUI[n];

            for (int i = 0; i < n; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var panel = new QuickPanel();

                Grid.SetColumn(panel, i);
                grid.Children.Add(panel);
                columns[i] = panel;
            }

            Add(grid);

            return columns;
        }

        public void Clear()
        {
            _panel.Children.Clear();
        }
    }

    internal class WpfTrigger<T> : Trigger<T>
    {
        public static implicit operator RoutedEventHandler(WpfTrigger<T> trigger)
        {
            return (_, _) => trigger.Notify();
        }

        public static implicit operator RoutedPropertyChangedEventHandler<T>(WpfTrigger<T> trigger)
        {
            return (_, _) => trigger.Notify();
        }

        public static implicit operator SelectionChangedEventHandler(WpfTrigger<T> trigger)
        {
            return (_, _) => trigger.Notify();
        }

        public static implicit operator TextChangedEventHandler(WpfTrigger<T> trigger)
        {
            return (_, _) => trigger.Notify();
        }
    }

    internal static class Extenders
    {
        public static void GetTriggers<T>(this UIElement element, out WpfTrigger<T> lostFocus, out WpfTrigger<T> gotFocus)
        {
            lostFocus = new();
            gotFocus = new();

            element.LostFocus += lostFocus;
            element.GotFocus += gotFocus;
        }

        public static void GetTriggers<T>(this ButtonBase element, out WpfTrigger<T> click, out WpfTrigger<T> lostFocus, out WpfTrigger<T> gotFocus)
        {
            click = new();

            element.Click += click;

            GetTriggers(element, out lostFocus, out gotFocus);
        }
    }

    internal class WpfBooleanParameter : Parameter<bool>
    {
        public WpfBooleanParameter(Func<bool> getter, Action<bool> setter, Trigger<bool>? click = null,
            Trigger<bool>? change = null, Trigger<bool>? gotFocus = null, Trigger<bool>? lostFocus = null) : base(
            getter, setter, click, change, gotFocus, lostFocus)
        {
        }

        public static implicit operator WpfBooleanParameter(CheckBox checkBox)
        {
            checkBox.GetTriggers<bool>(out var click, out var lostFocus, out var gotFocus);

            Func<bool> getter = () => checkBox.IsChecked ?? false;
            Action<bool> setter = val => checkBox.IsChecked = val;

            return new WpfBooleanParameter(getter, setter, click, click, gotFocus, lostFocus);
        }
    }

    internal class WpfDoubleParameter : Parameter<double>
    {
        public WpfDoubleParameter(Func<double> getter, Action<double> setter, Trigger<double>? click = null,
            Trigger<double>? change = null, Trigger<double>? gotFocus = null, Trigger<double>? lostFocus = null) : base(
            getter, setter, click, change, gotFocus, lostFocus)
        {
        }

        public static implicit operator WpfDoubleParameter(Slider slider)
        {
            slider.GetTriggers<double>(out var lostFocus, out var gotFocus);

            WpfTrigger<double> change = new WpfTrigger<double>();

            slider.ValueChanged += change;

            Func<double> getter = () => slider.Value;
            Action<double> setter = d => slider.Value = d;

            return new WpfDoubleParameter(getter, setter, null, change, gotFocus, lostFocus);
        }
    }

    internal class WpfStringParameter : Parameter<string>
    {
        public WpfStringParameter(Func<string> getter, Action<string> setter, Trigger<string>? click = null,
            Trigger<string>? change = null, Trigger<string>? gotFocus = null, Trigger<string>? lostFocus = null) : base(
            getter, setter, click, change, gotFocus, lostFocus)
        {
        }

        public static implicit operator WpfStringParameter(TextBox textBox)
        {
            textBox.GetTriggers<string>(out var lostFocus, out var gotFocus);

            WpfTrigger<string> change = new WpfTrigger<string>();

            textBox.TextChanged += change;

            Func<string> getter = () => textBox.Text;
            Action<string> setter = t => textBox.Text = t;

            return new WpfStringParameter(getter, setter, null, change, gotFocus, lostFocus);
        }
    }

    internal class WpfColorParameter : Parameter<Color>
    {
        public WpfColorParameter(Func<Color> getter, Action<Color> setter, Trigger<Color>? click = null,
            Trigger<Color>? change = null, Trigger<Color>? gotFocus = null, Trigger<Color>? lostFocus = null) 
            : base(getter, setter, click, change, gotFocus, lostFocus)
        {
        }

        public static implicit operator WpfColorParameter(ColorPicker cp)
        {
            var change = new WpfTrigger<Color>();

            cp.ColorChanged += change;

            Func<Color> getter = () => Color.FromArgb(cp.ActiveColor.A, cp.ActiveColor.R, cp.ActiveColor.G, cp.ActiveColor.B);
            Action<Color> setter = c => cp.ActiveColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);

            return new WpfColorParameter(getter, setter, change: change);
        }
    }

    internal class WpfParameter<T> : Parameter<T>
    {
        public WpfParameter(Func<T> getter, Action<T> setter, Trigger<T>? click = null, Trigger<T>? change = null,
            Trigger<T>? gotFocus = null, Trigger<T>? lostFocus = null) : base(getter, setter, click, change, gotFocus,
            lostFocus)
        {
        }

        public static implicit operator WpfParameter<T>(ComboBox comboBox)
        {
            comboBox.GetTriggers<T>(out var lostFocus, out var gotFocus);

            var change = new WpfTrigger<T>();

            comboBox.SelectionChanged += change;

            Func<T> getter = () => (T) comboBox.SelectedItem;
            Action<T> setter = t => comboBox.SelectedItem = t;

            return new WpfParameter<T>(getter, setter, null, change, gotFocus, lostFocus);
        }
    }
}
