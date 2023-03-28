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

Other than buttons, you can use QuickForms to create track bars, check boxes, combo boxes, and color pickers.
