using OxyPlot.WindowsForms;
using System.IO;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusViewer.Models;

namespace OPMedia.UI.Controls
{
    public partial class MapViewCtl : UserControl
    {
        private readonly MapViewModel _viewModel;
        private readonly PngExporter _exporter;

        public MapViewCtl()
        {
            InitializeComponent();

            try
            {
                _viewModel = new MapViewModel(this);

                _exporter = new PngExporter
                {
                    Width = XSize,
                    Height = (int)(3f * (float)XSize / 4f)
                };

                plotView.Model = _viewModel.Model;
            }
            catch { }
        }

        public void SaveImage(bool isAutoSave)
        {
            string fileName = _viewModel.FileTitle;

            if (isAutoSave)
            {
                string dataType = ControlPanelModel.Instance.SelectedDataType.Name;
                string viewport = ControlPanelModel.Instance.SelectedViewport.Name;

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
                SaveFileDialog dlg = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "PNG",
                    Title = "Save image as ...",
                    CreatePrompt = true,
                    ValidateNames = true,
                    Filter = "PNG files(*.png)|*.png|JPEG files(*.jpg)|*.jpg",
                    FileName = fileName
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string imageFile = dlg.FileName;
                    DoSave(imageFile);
                }
            }
        }

        const int XSize = 900;

        void DoSave(string imageFile)
        {
            string ext = Path.GetExtension(imageFile).Trim('.').ToUpperInvariant();

            if (ext == "PNG")
            {
                _exporter.ExportToFile(_viewModel.Model, imageFile);
                return;
            }

            string pngFile = System.IO.Path.ChangeExtension(imageFile, "png");

            // PlotView can only save as PNG. 
            // We need to do a conversion PNG->JPG
            _exporter.ExportToFile(_viewModel.Model, pngFile);

            if (File.Exists(pngFile))
            {
                using (var img = System.Drawing.Image.FromFile(pngFile))
                {
                    img.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                    img.Dispose();
                }

                if (File.Exists(imageFile))
                    File.Delete(pngFile);
            }
        }

        internal void RefitMap()
        {
            _viewModel.RefitMap();
        }
    }
}
