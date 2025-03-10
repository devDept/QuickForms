﻿using System.Windows;
using System.Windows.Controls;

namespace QuickForms.Wpf.Elements
{
    /// <summary>
    /// Custom slider storing a format string for its value.
    /// </summary>
    internal class QuickSlider : Slider
    {
        public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.RegisterAttached("NumberFormat", typeof(string), typeof(QuickSlider), new PropertyMetadata("{0:0.00}"));
        public static readonly DependencyProperty ReadableValueProperty = DependencyProperty.RegisterAttached("ReadableValue", typeof(string), typeof(QuickSlider), new PropertyMetadata("0.00"));

        public string NumberFormat
        {
            get => (string) GetValue(NumberFormatProperty);
            set => SetValue(NumberFormatProperty, value);
        }

        public string ReadableValue
        {
            get => (string) GetValue(ReadableValueProperty);
            set => SetValue(ReadableValueProperty, value);
        }

        public QuickSlider(double min, double max) : base()
        {
            NumberFormat = $"{{0:{new string('0', 1)}.00}}"; // Format the number with at least 1 digit before the decimal point and 2 decimal places

            Minimum = min;
            Maximum = max;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            ReadableValue = string.Format(NumberFormat, newValue);

            base.OnValueChanged(oldValue, newValue);
        }
    }
}
