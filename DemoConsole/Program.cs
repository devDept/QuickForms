using System.Drawing;
using System.Windows;
using QuickForms.Core;
using QuickForms.Wpf;

Thread thread = new Thread(() =>
{
    QuickForm qf = new QuickForm();

    qf.AddTrackBar("TrackBar", 1, 9, 1, d => { Console.WriteLine(d); });

    var cb = qf.AddComboBox("ComboBox", new[]
    {
        "Hello",
        "World"
    }, s =>
    {
        // print the selected value
        Console.WriteLine(s);
    });

    // set the selected combo box item
    cb.Value = "World";

    qf.AddTextBox("TextBox", t =>
    {
        // print the user text
        Console.WriteLine(t);
    });

    qf.AddColorPicker("Color", Color.Lime, c =>
    {
        // print color's hex code
        Console.WriteLine($"#{c.R:X2}{c.G:X2}{c.B:X2}");
    });

    var qp = qf.AddCategory("Category");

    qp.AddCheckBox("Yes or no?");

    qp.AddButton("Button", () => { });

    // create a table with 2 rows and 5 columns
    var table = qf.AddCategory();
    table.Padding = 5;
    table.Options.VerticalSpacing = 0;

    for (int r = 0; r < 2; r++)
    {
        foreach (IQuickUI col in table.Split(5))
        {
            col.AddTextBox();
            col.Padding = 5;
        }
    }

    // current theme
    Themes theme = Themes.Dark;

    // create a button to change the theme
    qf.AddButton("Change theme", () =>
    {
        Themes newTheme = theme == Themes.Dark ? Themes.Light : Themes.Dark;
        qf.SetTheme(newTheme);
        theme = newTheme;
    });

    new Application().Run(qf);
});

// the application must be single threaded in order
// for WinForm / WPF to work
thread.SetApartmentState(ApartmentState.STA);
thread.Start();