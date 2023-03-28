namespace QuickForms.Core
{
    /// <summary>
    /// Utility class to give a name to an object. It may be used
    /// with <see cref="IQuickUI.AddComboBox{T}"/> to display a
    /// name for each item.
    /// </summary>
    /// <typeparam name="T">Type for the named object.</typeparam>
    public class Named<T>
    {
        public string Name { get; set; }

        public readonly T Value;

        public Named(string name, T value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Named<T> { Value: not null } named)
            {
                return named.Value.Equals(Value);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }

        public static implicit operator T(Named<T> val)
        {
            return val.Value;
        }
    }

    public static class NameExtenders
    {
        /// <summary>
        /// Creates an instance of <see cref="Named{T}"/> using this instance.
        /// </summary>
        public static Named<T> Name<T>(this T obj, string name) => new (name, obj);
    }
}
