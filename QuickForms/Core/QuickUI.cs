using System;
using System.Drawing;
using System.Collections.Generic;

namespace QuickForms.Core
{
    /// <summary>
    /// Interface for quick user interfaces.
    /// </summary>
    public interface IQuickUI
    {
        double Padding { get; set; }

        Parameter<bool> CheckBox(string label, Action<bool>? function = null);

        Parameter<string> TextBox(string label, Action<string>? function = null);

        Parameter<double> TrackBar(string label, double min, double max, double? step = null, Action<double>? function = null);

        Parameter<T> ComboBox<T>(string label, IEnumerable<T> values, Action<T>? function = null);

        Parameter<T> RadioButtons<T>(IDictionary<T, string> values, Action<T>? function = null);

        void Button(string text, Action function);

        IQuickUI Category(string? title = null);

        IQuickUI[] Split(int columns = 2);

        IQuickUI Label(string text, double percentage = 0.3);

        Parameter<Color> ColorPicker(Color color, Action<Color>? function);

        void Clear();
    }
}