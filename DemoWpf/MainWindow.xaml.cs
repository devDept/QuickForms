using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuickForms.Wpf;

namespace DemoWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // ---------------------------------
            // Quick form external window
            // ---------------------------------

            QuickForm qf = new QuickForm();
            qf.Show();

            var a = qf.AddCheckBox("Hello");

            qf.AddCheckBox("Hello");

            a.Value = true;

            qf.AddButton("Hello darkness", () => { a.Value = !a.Value; });
            qf.AddButton("Light", () =>
            {
                qf.SetTheme(Themes.Light);
            });

            // ---------------------------------
            // Left panel
            // ---------------------------------

            QuickControlLeft.AddTextBox("Textbox");
            QuickControlLeft.AddTrackBar("Trackbar", 0, 100);
            QuickControlLeft.AddColorPicker("Color");

            // ---------------------------------
            // Right panel
            // ---------------------------------

            QuickControlRight.AddComboBox("Combo box", new[] { 0, 1, 2, 3, 4 }.Select(x => $"Option {x}"));

            var cat = QuickControlRight.AddCategory();
            var cb = cat.AddCheckBox("Yes or no?");

            QuickControlRight.AddButton("Button", () =>
            {
                cb.Value = !cb.Value;
            });
        }
    }
}
