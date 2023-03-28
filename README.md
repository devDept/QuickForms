# ![Logo](https://github.com/devdept/QuickForms/blob/new-style/banner.png?raw=true)

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
