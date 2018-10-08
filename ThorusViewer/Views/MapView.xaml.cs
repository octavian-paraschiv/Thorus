using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThorusViewer.Palettes;
using Microsoft.Win32;
using ThorusViewer.Models;
using OxyPlot;
using ThorusCommon.Engine;

namespace ThorusViewer.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
        }

        public void SaveImage(bool isAutoSave)
        {
            string fileName = (this.DataContext as MapViewModel).FileTitle;

            if (isAutoSave)
            {
                string dataType = App.ControlPanelModel.SelectedDataType.Name;
                string viewport = App.ControlPanelModel.SelectedViewport.Name;

                string imgFolder = string.Format(@"image\{0}\{1}", dataType, viewport);
                string saveFolder = System.IO.Path.Combine(SimulationData.WorkFolder, imgFolder);
                string jpgFile = string.Format("{0}.PNG", fileName);

                if (Directory.Exists(saveFolder) == false)
                    Directory.CreateDirectory(saveFolder);

                string imageFile = System.IO.Path.Combine(saveFolder, jpgFile);
                DoSave(imageFile);
            }
            else
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.AddExtension = true;
                dlg.DefaultExt = "PNG";
                dlg.Title = "Save image as ...";
                dlg.CreatePrompt = true;
                dlg.ValidateNames = true;
                dlg.Filter = "PNG files(*.png)|*.png|JPEG files(*.jpg)|*.jpg";
                dlg.FileName = fileName;

                if (dlg.ShowDialog() == true)
                {
                    string imageFile = dlg.FileName;
                    DoSave(imageFile);
                }
            }
        }

        const int XSize = 900;

        void DoSave(string imageFile)
        {
            string ext = System.IO.Path.GetExtension(imageFile).Trim('.').ToUpperInvariant();

            Size imgSize = new Size(XSize, (int)(3f * (float)XSize / 4f));
            
            if (ext == "PNG")
            {
                plotView.SaveBitmap(imageFile, (int)imgSize.Width, (int)imgSize.Height, OxyColors.Transparent);
                return;
            }

            string pngFile = System.IO.Path.ChangeExtension(imageFile, "png");

            // PlotView can only save as PNG. 
            // We need to do a conversion PNG->JPG
            plotView.SaveBitmap(pngFile, (int)imgSize.Width, (int)imgSize.Height, OxyColors.Transparent);

            if (File.Exists(pngFile))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(pngFile);
                img.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                img.Dispose();
                img = null;

                if (File.Exists(imageFile))
                    File.Delete(pngFile);
            }
        }

        internal void RefitMap()
        {
            (plotView.DataContext as MapViewModel).RefitMap();
        }
    }
}
