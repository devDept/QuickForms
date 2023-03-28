using System;

namespace QuickForms.Core
{
    public class QuickOptions : ICloneable
    {
        /// <summary>
        /// Default options: every quick UI is created with a copy of this options.
        /// </summary>
        public static readonly QuickOptions Default = new ();

        /// <summary>
        /// Vertical spacing between controls.
        /// </summary>
        public double VerticalSpacing { get; set; } = 10;

        /// <summary>
        /// Height of each control (or row) of the quick UI.
        /// </summary>
        public double ComponentHeight { get; set; } = 30;

        /// <summary>
        /// Percentage of the quick UI taken by labels next to controls.
        /// </summary>
        public int LabelPercentage { get; set; } = 30;

        /// <summary>
        /// Default padding for newly created quick UIs.
        /// </summary>
        public double DefaultPadding { get; set; } = 10;

        protected QuickOptions()
        {
        }

        public QuickOptions Clone()
        {
            return new QuickOptions
            {
                VerticalSpacing = VerticalSpacing,
                ComponentHeight = ComponentHeight,
                LabelPercentage = LabelPercentage,
                DefaultPadding = DefaultPadding
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
