using QuickForms.Wpf;

namespace DemoWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // ---------------------------------
            // Quick form external window
            // ---------------------------------

            QuickForms.WinForm.QuickForm qf = new QuickForms.WinForm.QuickForm();
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

            quickControlwf1.AddTextBox("Textbox");
            quickControlwf1.AddTrackBar("Trackbar", 0, 100);
            quickControlwf1.AddColorPicker("Color");

            // ---------------------------------
            // Right panel
            // ---------------------------------

            quickControlwf2.AddComboBox("Combo box", new []{0, 1, 2, 3, 4}.Select(x => $"Option {x}"));
            
            var cat = quickControlwf2.AddCategory();
            var cb = cat.AddCheckBox("Yes or no?");

            quickControlwf2.AddButton("Button", () =>
            {
                cb.Value = !cb.Value;
            });
        }
    }
}