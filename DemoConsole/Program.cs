using System.Drawing;
using System.Runtime.Serialization;
using System.Windows;
using QuickForms.Core;
using QuickForms.Wpf;

Thread thread = new Thread(() =>
{
    QuickForm qf = new QuickForm();

    var a = qf.AddCheckBox("Hello");

    qf.AddCheckBox("Hello");

    a.Value = true;

    qf.AddButton("Hello!", () => { a.Value = !a.Value; });
    qf.AddButton("Hello!", () => { qf.AddButton("Hello bro!", () => {}); });

    qf.AddTrackBar("Hello", 1, 9, 1, d =>
    {
        Console.WriteLine(d);
    });

    var cb = qf.AddComboBox("Label", new string[]
    {
        "Hello",
        "World"
    }, s =>
    {
        Console.WriteLine(s);
    });

    cb.Value = "Reeas";

    qf.AddComboBox("Values", new []
    {
        0.Name("Zero"),
        1.Name("Uno")
    }, i =>
    {
        Console.WriteLine(i);
    });

    qf.AddTextBox("Hello", t =>
    {
        Console.WriteLine(t);
    });

    qf.AddColorPicker("Color", Color.Lime, (col) =>
    {
        Console.WriteLine(col.GetHue());
    });

    var qp = qf.AddCategory("Cats & Dogs");

    qp.AddButton("Hola", () => {});

    qp = qp.AddCategory("Nasty nested category with very long title");

    var copy = qp;

    qp.AddButton("Delete", () =>
    {
        copy.Clear();
    });

    qp = qp.AddCategory();

    qp.AddCheckBox("Yes or no?");

    qp = qf.AddCategory("Options");

    qp.AddTrackBar(null, 0, 10, 1);
    qp.AddColorPicker();
    qp.AddCheckBox();
    qp.AddTextBox();

    var table = qf.AddCategory();
    table.Padding = 5;
    table.Options.VerticalSpacing = 0;

    for (int r = 0; r < 5; r++)
    {
        var row = table.Split(5);

        foreach (IQuickUI col in row)
        {
            col.AddTextBox();
            col.Padding = 5;
        }
    }
    
    Themes theme = Themes.Dark;

    qf.AddButton("Change theme", () =>
    {
        Themes newTheme = theme == Themes.Dark ? Themes.Light : Themes.Dark;
        qf.SetTheme(newTheme);
        theme = newTheme;
    });

    new Application().Run(qf);
});

thread.SetApartmentState(ApartmentState.STA);
thread.Start();