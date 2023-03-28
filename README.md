# ![Logo](https://raw.githubusercontent.com/devdept/QuickForms/main/banner.png)

![example workflow](https://github.com/devdept/QuickForms/actions/workflows/main.yml/badge.svg)

QuickForms is a C# library to quickly prototipe user interfaces via code using WinForm or WPF. QuickForms can be used to create a new window in your application, or to add a new panel to an existing window. Here's an example that shows how to create a quick form with a 'Hello World' button:

```c#
QuickForm qf = new QuickForm();
qf.Show();

qf.AddButton("Hello World", () =>
{
    MessageBox.Show("Hello World!");
});
```

Other than buttons, you can use QuickForms to create track bars, check boxes, combo boxes, and color pickers. Here's an animated GIF that demonstrates some of the functionalities:

![Sample usage GIF](https://raw.githubusercontent.com/devdept/QuickForms/main/media/sample_usage.gif)

It is also possible to edit the user interface dynamically and add or remove existing controls. The following GIF shows a table where the number of rows and columns can be changed using two track bars.

![Dynamic table GIF](https://raw.githubusercontent.com/devdept/QuickForms/main/media/dynamic_table.gif)
