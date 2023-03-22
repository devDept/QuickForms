namespace QuickForms.Core
{
    public class QuickOptions
    {
        public static readonly QuickOptions Default = new ();

        public double VerticalSpacing { get; set; } = 10;

        public double ComponentHeight { get; set; } = 30;

        public int LabelPercentage { get; set; } = 30;
        
        protected QuickOptions()
        {
        }

        public QuickOptions Copy()
        {
            return new QuickOptions
            {
                VerticalSpacing = VerticalSpacing,
                ComponentHeight = ComponentHeight,
                LabelPercentage = LabelPercentage
            };
        }
    }
}
