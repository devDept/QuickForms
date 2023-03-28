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
        /// <summary>
        /// Gets or sets the padding of this quick UI, i.e., the spacing between
        /// the outer rectangle and the inner controls.
        /// </summary>
        double Padding { get; set; }

        /// <summary>
        /// Gets or sets the options for this quick UI. Options are only taken into
        /// account when adding something to this UI: changing options does not have
        /// any effect on controls that have already been added.
        /// </summary>
        QuickOptions Options { get; set; }

        /// <summary>
        /// Adds a checkbox to this quick UI.
        /// </summary>
        /// <param name="label">
        /// The text for the label to be added to the left of the checkbox.
        /// When null, the label will not be added.
        /// </param>
        /// <param name="function">The function invoked when the checkbox value changes.</param>
        /// <returns>A boolean parameter linked to the checkbox value.</returns>
        Parameter<bool> AddCheckBox(string? label = null, Action<bool>? function = null);

        /// <summary>
        /// Adds a text box to this quick UI.
        /// </summary>
        /// <param name="label">
        /// The text for the label to be added to the left of the text box.
        /// When null, the label will not be added.
        /// </param>
        /// <param name="function">The function invoked when the text box value changes.</param>
        /// <returns>A string parameter linked to the text box value.</returns>
        Parameter<string> AddTextBox(string? label = null, Action<string>? function = null);

        /// <summary>
        /// Adds a track bar (or slider) to this quick UI.
        /// </summary>
        /// <param name="label">
        /// The text for the label to be added to the left of the track bar.
        /// When null, the label will not be added.
        /// </param>
        /// <param name="min">Minimum track bar value.</param>
        /// <param name="max">Maximum track bar value.</param>
        /// <param name="step">Track bar step (the minimum change in value).</param>
        /// <param name="function">The function invoked when the track bar value changes.</param>
        /// <returns>A double parameter linked to the track bar.</returns>
        Parameter<double> AddTrackBar(string? label, double min, double max, double? step = null,
            Action<double>? function = null);

        /// <summary>
        /// Adds a combo box to this quick UI. To give a name to the items, you may use <see cref="Named{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the combo box elements.</typeparam>
        /// <param name="label">
        /// The text for the label to be added to the left of the combo box.
        /// When null, the label will not be added.
        /// </param>
        /// <param name="values">List of combo box values.</param>
        /// <param name="function">The function invoked when the combo box selected item changes.</param>
        /// <returns>A parameter linked to the combo box selected item.</returns>
        Parameter<T> AddComboBox<T>(string? label, IEnumerable<T> values, Action<T>? function = null);

        /// <summary>
        /// Adds a HSV color picker to this quick UI.
        /// </summary>
        /// <param name="label">
        /// The text for the label to be added to the left of the color picker.
        /// When null, the label will not be added.
        /// </param>
        /// <param name="color">Initially selected color.</param>
        /// <param name="function">The function invoked when the selected color changes.</param>
        /// <returns>A color parameter linked to the selected color.</returns>
        Parameter<Color> AddColorPicker(string? label = null, Color? color = null, Action<Color>? function = null);

        /// <summary>
        /// Adds a button to this quick UI.
        /// </summary>
        /// <param name="text">Text of the button.</param>
        /// <param name="function">The function invoked when the button is clicked.</param>
        void AddButton(string? text, Action function);

        /// <summary>
        /// Adds a category to this quick UI. A category is another quick UI nested in the current one,
        /// inside a box and possibly with a title.
        /// </summary>
        /// <param name="title">The category title.</param>
        /// <returns>The quick UI of the newly created category.</returns>
        IQuickUI AddCategory(string? title = null);
        
        /// <summary>
        /// Splits this quick UI in <paramref name="n"/> columns and returns a quick UI for each column.
        /// </summary>
        /// <param name="n">The number of splits (or columns).</param>
        /// <returns>An array of <paramref name="n"/> quick UIs resulting from splitting this one.</returns>
        IQuickUI[] Split(int n = 2);

        /// <summary>
        /// Clear this quick UI, removing all the controls.
        /// </summary>
        void Clear();
    }
}