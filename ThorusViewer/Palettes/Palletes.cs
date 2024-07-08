using ThorusCommon;
using ThorusCommon.IO;
using ThorusCommon.MatrixExtensions;
using ThorusViewer.Palettes;

namespace ThorusViewer.Pallettes
{
    #region Elevation Palettes
    public class E_00_Palette : WeatherDataPalette
    {
        public E_00_Palette()
            : base("E_00")
        {
            _lineWidth = 1;
            _lineSpacing = 500;
            _minMax = new Range<float>(0, 9000);
            _acceptsContourLines = true;
            this.Description = "ELV";
            this.Unit = "m";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class E_WL_Palette : WeatherDataPalette
    {
        public E_WL_Palette()
            : base("E_WL")
        {
            _lineWidth = 1;
            _lineSpacing = 1;
            _minMax = new Range<float>(0, 1);
            _acceptsContourLines = true;
            this.Description = "WL mask";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class N_00_Palette : WeatherDataPalette
    {
        public N_00_Palette()
            : base("N_00")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _minMax = new Range<float>(0, 100f);
            _acceptsContourLines = true;
            this.Description = "SNOW COVER";
            this.Unit = "cm";
            this.ShowContours = false;
        }
    }

    public class N_DD_Palette : WeatherDataPalette
    {
        public N_DD_Palette()
            : base("N_DD")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _minMax = new Range<float>(0, 100f);
            _acceptsContourLines = true;
            this.Description = "FALLEN SNOW";
            this.Unit = "cm";
            this.ShowContours = false;
        }
    }

    public class R_00_Palette : WeatherDataPalette
    {
        public R_00_Palette()
            : base("R_00")
        {
            _lineWidth = 1;
            _lineSpacing = 0.5f;
            _minMax = new Range<float>(0, 15f);
            _acceptsContourLines = true;
            this.Description = "SOIL ACCUMULATED RAIN";
            this.Unit = "cm";
            this.ShowContours = false;
        }
    }
    public class R_DD_Palette : WeatherDataPalette
    {
        public R_DD_Palette()
            : base("R_DD")
        {
            _lineWidth = 1;
            _lineSpacing = 0.5f;
            _minMax = new Range<float>(0, 100f);
            _acceptsContourLines = true;
            this.Description = "FALLEN RAIN";
            this.Unit = "mm";
            this.ShowContours = false;
        }
    }

    public class B_00_Palette : WeatherDataPalette
    {
        public B_00_Palette()
            : base("B_00")
        {
            _lineWidth = 1;
            _lineSpacing = 1;
            _minMax = new Range<float>(0, 15f);
            _acceptsContourLines = true;
            this.Description = "BLIZZARD CONDITIONS";
            this.Unit = "";
            this.ShowContours = false;
        }
    }
    #endregion

    #region Pressure Palettes
    public class PressurePalette : WeatherDataPalette
    {
        public PressurePalette()
            : base("P")
        {
            _lineWidth = 2;
            this.Unit = "hPa";
            this.ShowContours = true;
            this.ShowHeatmap = false;
        }
    }

    public class P_00_Palette : PressurePalette
    {
        public P_00_Palette()
        {
            float[] minMax = LevelPressure.SeaLevelPressure.IdentifyPressureRange();
            _lineSpacing = (minMax[1] - minMax[0]) / 30;
            _minMax = new Range<float>(minMax[0], minMax[1]);
        }
    }

    public class P_01_Palette : PressurePalette
    {
        public P_01_Palette()
        {
            float[] minMax = LevelPressure.MidLevelPressure.IdentifyPressureRange();
            _lineSpacing = (minMax[1] - minMax[0]) / 30;
            _minMax = new Range<float>(minMax[0], minMax[1]);
        }
    }

    public class P_02_Palette : PressurePalette
    {
        public P_02_Palette()
        {
            float[] minMax = LevelPressure.TopLevelPressure.IdentifyPressureRange();
            _lineSpacing = (minMax[1] - minMax[0]) / 30;
            _minMax = new Range<float>(minMax[0], minMax[1]);
        }
    }

    public class P_03_Palette : PressurePalette
    {
        public P_03_Palette()
        {
            float[] minMax = LevelPressure.JetLevelPressure.IdentifyPressureRange();
            _lineSpacing = (minMax[1] - minMax[0]) / 30;
            _minMax = new Range<float>(minMax[0], minMax[1]);
        }
    }
    #endregion

    #region Temperature Palettes

    public class TemperaturePalette : WeatherDataPalette
    {
        public TemperaturePalette()
            : base("T")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _minMax = TempRange;
            _acceptsContourLines = true;
            this.Unit = "°C";
            this.ShowContours = false;
        }
    }

    public class T_SL_Palette : TemperaturePalette
    {
        public T_SL_Palette()
        {
        }
    }

    public class T_SH_Palette : TemperaturePalette
    {
        public T_SH_Palette()
        {
        }
    }

    public class T_NL_Palette : TemperaturePalette
    {
        public T_NL_Palette()
        {
        }
    }

    public class T_NH_Palette : TemperaturePalette
    {
        public T_NH_Palette()
        {
        }
    }

    public class T_DL_Palette : TemperaturePalette
    {
        public T_DL_Palette()
        {
            _lineSpacing = 0.5f;
        }
    }

    public class T_DH_Palette : TemperaturePalette
    {
        public T_DH_Palette()
        {
            _lineSpacing = 0.5f;
        }
    }

    public class T_DA_Palette : TemperaturePalette
    {
        public T_DA_Palette()
        {
            _lineSpacing = 0.5f;
        }
    }

    public class T_TE_Palette : TemperaturePalette
    {
        public T_TE_Palette()
        {
        }
    }


    public class T_TL_Palette : TemperaturePalette
    {
        public T_TL_Palette()
        {
        }
    }

    public class T_TW_Palette : TemperaturePalette
    {
        public T_TW_Palette()
        {
        }
    }

    public class T_TS_Palette : TemperaturePalette
    {
        public T_TS_Palette()
        {
        }
    }


    public class T_00_Palette : TemperaturePalette
    {
        public T_00_Palette()
        {
        }
    }

    public class T_01_Palette : TemperaturePalette
    {
        public T_01_Palette()
        {
            //_minMax = TMidLevelRange;
        }
    }

    public class T_02_Palette : TemperaturePalette
    {
        public T_02_Palette()
        {
            //_lineSpacing = 5;
            //_minMax = new Range<float>(-60, 0);
        }
    }
    #endregion

    #region Humidity Palettes
    public class HumidityPalette : WeatherDataPalette
    {
        public HumidityPalette()
            : base("H")
        {
            _lineWidth = 1;
            _lineSpacing = 5;
            _minMax = new Range<float>(0, 100);
            _acceptsContourLines = true;

            this.Unit = "%";

            this.ShowHeatmap = true;
            this.ShowContours = false;
        }
    }

    public class H_00_Palette : HumidityPalette
    {
        public H_00_Palette()
        {
        }
    }

    public class H_01_Palette : HumidityPalette
    {
        public H_01_Palette()
        {
        }
    }

    public class H_02_Palette : HumidityPalette
    {
        public H_02_Palette()
        {
        }
    }

    public class A_00_Palette : HumidityPalette
    {
        public A_00_Palette()
        {
        }
    }

    #endregion

    #region Clouds and Fronts palettes

    public class F_SI_Palette : WeatherDataPalette
    {
        public F_SI_Palette()
            : base("H")
        {
            _lineWidth = 1f;
            _lineSpacing = 10;
            _minMax = new Range<float>(0, 100);
            _acceptsContourLines = true;

            this.Description = "Fog";
            this.Unit = "%";
            this.ShowContours = false;
        }
    }

    public class C_00_Palette : WeatherDataPalette
    {
        public C_00_Palette()
            : base("C")
        {
            _lineWidth = 1f;
            _lineSpacing = 10;
            _minMax = new Range<float>(0, 1200);
            _acceptsContourLines = true;

            this.Description = "PRC";
            this.Unit = "l/mp";
            this.ShowContours = false;
        }
    }

    public class F_00_Palette : WeatherDataPalette
    {
        public F_00_Palette()
            : base("F")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _minMax = new Range<float>(-10, 10);
            _acceptsContourLines = false;
            this.ShowContours = false;

            this.Description = "FR";
            this.Unit = "%";
        }
    }

    public class M_00_Palette : WeatherDataPalette
    {
        public M_00_Palette()
            : base("M")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _acceptsContourLines = false;
            _minMax = new Range<float>(-3, 3);
            this.ShowContours = false;
            this.Description = "Air mass type";
            this.Unit = "";
        }
    }

    public class TestPalette : WeatherDataPalette
    {
        public TestPalette()
            : base("C")
        {
            _lineWidth = 2;
            _lineSpacing = 10;
            _minMax = new Range<float>(-100, 100);
            this.Description = "test";
            this.Unit = "";
        }
    }

    public class D_D1_Palette : TestPalette
    {
        public D_D1_Palette()
        {
        }
    }
    public class D_D2_Palette : TestPalette
    {
        public D_D2_Palette()
        {
        }
    }
    public class D_D3_Palette : TestPalette
    {
        public D_D3_Palette()
        {
        }
    }
    #endregion

    #region Geopotential Height palettes
    public class GeopotentialHeightPalette : WeatherDataPalette
    {
        public GeopotentialHeightPalette()
            : base("Z")
        {
            _lineWidth = 1;
            _acceptsContourLines = true;
            this.Description = "Z";
            this.Unit = "m";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class Z_00_Palette : GeopotentialHeightPalette
    {
        public Z_00_Palette()
        {
            _minMax = new Range<float>(-550, 650);
            _lineSpacing = (_minMax.Max - _minMax.Min) / 30;
        }
    }

    public class Z_01_Palette : GeopotentialHeightPalette
    {
        public Z_01_Palette()
        {
            _minMax = new Range<float>(600, 2400);
            _lineSpacing = (_minMax.Max - _minMax.Min) / 30;
        }
    }

    public class Z_02_Palette : GeopotentialHeightPalette
    {
        public Z_02_Palette()
        {
            _minMax = new Range<float>(4600, 6400);
            _lineSpacing = (_minMax.Max - _minMax.Min) / 30;
        }
    }

    #endregion

    #region Custom data palettes
    public class CustomDataPalette : WeatherDataPalette
    {
        public CustomDataPalette()
            : base("D")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;

            _minMax = new Range<float>(-10, 10);

            this.Description = "D";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }


    public class D_BP_Palette : WeatherDataPalette
    {
        public D_BP_Palette()
            : base("D")
        {
            _minMax = new Range<float>(-2, 2);

            _lineWidth = 1;
            _lineSpacing = .1f;

            this.Description = "Blocking potential";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class D_FP_Palette : WeatherDataPalette
    {
        public D_FP_Palette()
            : base("H")
        {
            _minMax = new Range<float>(0, 1);

            _lineWidth = 1;
            _lineSpacing = .1f;

            this.Description = "Cyclogenetic potential";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class D_XX_Palette : CustomDataPalette
    {
        public D_XX_Palette()
        {
        }
    }

    public class D_YY_Palette : CustomDataPalette
    {
        public D_YY_Palette()
        {
        }
    }


    public class D_00_Palette : CustomDataPalette
    {
        public D_00_Palette()
        {
        }
    }

    public class D_01_Palette : CustomDataPalette
    {
        public D_01_Palette()
        {
        }
    }

    public class D_02_Palette : CustomDataPalette
    {
        public D_02_Palette()
        {
        }
    }
    #endregion

    #region Lifted Index
    public class LiftedIndexPalette : WeatherDataPalette
    {
        public LiftedIndexPalette()
            : base("L")
        {
            _lineWidth = 1;
            _lineSpacing = 1f;
            _minMax = new Range<float>(-15, 15);
            _acceptsContourLines = true;

            this.Description = "D";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }

    public class L_00_Palette : LiftedIndexPalette
    {
        public L_00_Palette()
        {
            this.Description = "Lift-Index (Thunderstorms)";
        }
    }
    #endregion

    public class E_LR_Palette : WeatherDataPalette
    {
        public E_LR_Palette()
            : base("D")
        {
            _minMax = new Range<float>(-10, 10);

            _lineWidth = 1;
            _lineSpacing = .1f;

            this.Description = "Environmental Lapse Rate";
            this.Unit = "";
            this.ShowContours = false;
            this.ShowHeatmap = true;
        }
    }
}
