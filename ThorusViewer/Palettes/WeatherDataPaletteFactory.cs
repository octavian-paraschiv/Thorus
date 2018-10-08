using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ThorusCommon.Data;
using OxyPlot;
using ThorusViewer.Views;
using ThorusCommon.IO;

namespace ThorusViewer.Palettes
{
    public static class ColorExtensions
    {
        public static OxyColor ColorFromString(string str)
        {
            string[] fields = str.Split(new char[]{ ' ' , ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length < 3)
                return OxyColors.Transparent;

            int i = 0;
            float r, g, b;

            if (float.TryParse(fields[i++], out r) == false)
                throw new Exception("ColorFromString: specify the RGB color components in range [0..1]");
            if (float.TryParse(fields[i++], out g) == false)
                throw new Exception("ColorFromString: specify the RGB color components in range [0..1]");
            if (float.TryParse(fields[i++], out b) == false)
                throw new Exception("ColorFromString: specify the RGB color components in range [0..1]");

            if ((r == g) && (g == b) && (r == 1))
                return OxyColors.Transparent;

            return OxyColor.FromRgb((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }
    }



    public class WeatherDataPalette
    {
        static readonly string asmFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        static readonly string palletesFolder = Path.Combine(asmFolder, "Palettes");

        const float TT = 30;
        public static readonly Range<float> TMidLevelRange = new Range<float>(-TT, TT);
        public static readonly Range<float> TempRange = new Range<float>(-2 * TT, 2 * TT);

        public string Type { get; private set; }
        public string Description { get; protected set; }
        public string Unit { get; protected set; }

        public bool ShowWindMap { get; set; }
        public bool ShowHeatmap { get; set; }
        public bool ShowContours { get; set; }

        protected bool _acceptsContourLines = false;
        public bool AcceptsContourLines { get { return _acceptsContourLines; } }

        protected bool _isDefault = false;
        public bool IsDefault
        {
            get { return _isDefault; }
        }

        protected float _lineWidth = 3;
        public float LineWidth
        {
            get { return _lineWidth; } set { _lineWidth = value; }
        }

        protected Range<float> _minMax = new Range<float>(-60, 60);
        public Range<float> MinMax
        {
            get { return _minMax; }
            set { _minMax = value; }
        }

        protected float _lineSpacing = 2;
        public float LineSpacing
        {
            get { return _lineSpacing; }
            set { _lineSpacing = value; }
        }

        private List<OxyColor> _colorSteps = new List<OxyColor>();
        public List<OxyColor> ColorSteps
        {
            get
            {
                return _colorSteps;
            }
        }

        protected LineColor _lineColor = new LineColor();
        public LineColor LineColor
        {
            get
            {
                return _lineColor;
            }

            set
            {
                _lineColor = value;
            }
        }

        public WeatherDataPalette(string type)
        {
            this.ShowContours = true;
            this.ShowHeatmap = true;
            this.Type = type;
            ReadPalette();
        }

        private void ReadPalette()
        {
            char ch = Type.ToUpperInvariant()[0];
            //switch (ch)
            //{
            //    case 'M':
            //    case 'L':
            //        ch = 'T';
            //        break;
            //}

            string paletteFileName = Path.Combine(palletesFolder, string.Format("{0}.thd", ch));
            string[] lines = File.ReadAllLines(paletteFileName);

            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                OxyColor c = ColorExtensions.ColorFromString(line);
                _colorSteps.Add(c);
            }
        }
    }

    public static class WeatherDataPaletteFactory
    {
        static Dictionary<string, WeatherDataPalette> _palettes = 
            new Dictionary<string, WeatherDataPalette>();

        static WeatherDataPalette _default = null;

        public static List<string> PaletteTypes
        {
            get
            {
                return _palettes.Keys.ToList();
            }
        }

        static WeatherDataPaletteFactory()
        {
            var paletteTypes = (from type in Assembly.GetAssembly(typeof(WeatherDataPaletteFactory)).GetTypes()
                               where type.IsSubclassOf(typeof(WeatherDataPalette))
                               select type).ToList();

            foreach(Type paletteType in paletteTypes)
            {
                try
                {
                    if (paletteType.Name.Contains("_") == false)
                        continue;

                    WeatherDataPalette pal = Activator.CreateInstance(paletteType) as WeatherDataPalette;
                    if (pal != null)
                    {
                        _palettes.Add(paletteType.Name.ToUpperInvariant().Substring(0, 4), pal);
                        if (pal.IsDefault)
                            _default = pal;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }

            if (_default == null)
                _default = _palettes.Values.First();
        }

        public static WeatherDataPalette GetPaletteForDataFile(string dataFile)
        {
            string dataType = "";
            string fileTitle = Path.GetFileNameWithoutExtension(dataFile);
            if (fileTitle.Length > 4)
                dataType = fileTitle.Substring(0, 4);

            return GetPaletteForDataType(dataType);
        }

        public static WeatherDataPalette GetPaletteForDataType(string dataType)
        {
            if (_palettes != null && _palettes.ContainsKey(dataType))
                return _palettes[dataType];

            return _default;
        }
    }
}
