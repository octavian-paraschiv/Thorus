namespace ThorusViewer.Models
{
    public enum LineColorMode
    {
        FixedColor = 0,
        Best_Contrast,
        Black_And_White
    }

    public class LineColor
    {
        public OxyPlot.OxyColor Color { get; private set; }
        public LineColorMode ColorMode { get; private set; }

        public LineColor(OxyPlot.OxyColor color) :
            this(color, LineColorMode.FixedColor)
        {
        }

        public LineColor(LineColorMode mode) :
            this(OxyPlot.OxyColors.Black, mode)
        {
        }

        public LineColor()
            : this(OxyPlot.OxyColors.Black)
        {
        }

        private LineColor(OxyPlot.OxyColor color, LineColorMode mode)
        {
            this.ColorMode = mode;

            if (mode == LineColorMode.FixedColor)
                this.Color = color;
            else
                this.Color = OxyPlot.OxyColors.Transparent;
        }

        public override bool Equals(object obj)
        {
            LineColor lc = (obj as LineColor);

            if (lc != null)
            {
                return (lc.Color == this.Color && lc.ColorMode == this.ColorMode);
            }

            return false;
        }

        public override string ToString()
        {
            if (ColorMode == LineColorMode.FixedColor)
                return Color.ToString();

            return ColorMode.ToString();
        }

        public override int GetHashCode() => Color.GetHashCode();
    }
}
