using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThorusCommon.IO;
using ThorusCommon.IO.Settings;

namespace ThorusCommon.Engine
{
    public static class SimulationData
    {
        static string _workFolder = Environment.CurrentDirectory;
        static string _dataFolder = "";
        static DictionaryFile _settingsFile;

        static object _availableSnapshotsLock = new object();
        static List<SimDateTime> _availableSnapshots = new List<SimDateTime>();
        static List<string> _availableDataTypes = new List<string>();

        public static event EventHandler SnapshotListChanged = null;

        public static string WorkFolder
        {
            get { return _workFolder; }
            set
            {
                // Only ThorusControlPanel calls this.
                _workFolder = value;
                _settingsFile["WorkFolder"] = value;
                _settingsFile.SaveFile();
            }
        }

        public static string DataFolder
        {
            get
            {
                if (_workFolder?.Length > 0)
                {
                    if (string.IsNullOrEmpty(_dataFolder))
                        _dataFolder = Path.Combine(_workFolder, "data");

                    return _dataFolder;
                }

                return null;
            }
        }

        public static bool IsDefaultDataFolder
        {
            get
            {
                var dataDirInfo = new DirectoryInfo(_dataFolder);
                var defaultDataDirInfo = new DirectoryInfo(Path.Combine(_workFolder, "data"));

                return (string.Compare(dataDirInfo.FullName, defaultDataDirInfo.FullName, true) == 0);
            }
        }


        public static void SetNewDataFolder(string path)
        {
            if (Directory.Exists(path))
            {
                _dataFolder = path;
                LookupDataFiles(null);
            }
        }


        public static List<SimDateTime> AvailableSnapshots
        {
            get
            {
                lock (_availableSnapshotsLock)
                {
                    return new List<SimDateTime>(_availableSnapshots);
                }
            }
        }

        public static List<string> AvailableDataTypes
        {
            get
            {
                lock (_availableSnapshotsLock)
                {
                    return new List<string>(_availableDataTypes);
                }
            }
        }

        public static List<string> GetDataFiles(string category, string filter = "*_MAP_????-??-??_??.thd")
        {
            string folder = DataFolder;

            if (category != null)
                folder = Path.Combine(DataFolder, category);

            if (Directory.Exists(folder))
                return Directory.EnumerateFiles(folder, filter).ToList();

            return null;
        }

        static SimulationData()
        {
            string winDir = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string winRoot = Path.GetPathRoot(winDir);
            string programDataDir = Path.Combine(winRoot, "ProgramData");
            string applicationDataDir = $"{programDataDir}/{Constants.Company}/{Constants.Product}";

            if (!Directory.Exists(applicationDataDir))
                Directory.CreateDirectory(applicationDataDir);

            _settingsFile = new DictionaryFile($"{applicationDataDir}/Settings.json");

            try
            {
                _workFolder = _settingsFile["WorkFolder"];

                if (_workFolder?.Length > 0 && Directory.Exists(_workFolder) == false)
                    Directory.CreateDirectory(_workFolder);
            }
            catch
            {
                _workFolder = winRoot;
            }
        }

        public static void SaveSettings()
        {
            _settingsFile.SaveFile();
        }

        public static void LookupDataFiles(string category)
        {
            lock (_availableSnapshotsLock)
            {
                try
                {
                    _availableSnapshots.Clear();

                    List<string> files = GetDataFiles(category);
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);

                        SimDateTime sdt = SimDateTime.FromFileName(fileName);
                        string dataType = fileName.Substring(0, 4).ToUpperInvariant();

                        if (_availableSnapshots.Contains(sdt) == false)
                            _availableSnapshots.Add(sdt);
                        if (_availableDataTypes.Contains(dataType) == false)
                            _availableDataTypes.Add(dataType);
                    }

                    _availableSnapshots.Sort();

                    SnapshotListChanged?.Invoke(null, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
        }

        public static SimDateTime SelectPreviousSnapshot(SimDateTime sdt)
        {
            List<SimDateTime> snapshots = AvailableSnapshots;
            if (snapshots != null && snapshots.Count > 0)
            {
                if (snapshots.Contains(sdt) == false)
                    sdt = SelectNearestSnapshot(sdt);

                int idx = snapshots.IndexOf(sdt);
                if (idx <= 1)
                    return snapshots[0];
                if (idx >= snapshots.Count)
                    return snapshots[snapshots.Count - 1];

                return snapshots[idx - 1];
            }

            return null;
        }

        public static SimDateTime SelectNextSnapshot(SimDateTime sdt)
        {
            List<SimDateTime> snapshots = AvailableSnapshots;
            if (snapshots != null && snapshots.Count > 0)
            {
                if (snapshots.Contains(sdt) == false)
                    sdt = SelectNearestSnapshot(sdt);

                int idx = snapshots.IndexOf(sdt);
                if (idx < 0)
                    return snapshots[0];
                if (idx >= snapshots.Count)
                    return snapshots[snapshots.Count - 1];

                return snapshots[idx + 1];
            }

            return null;
        }

        public static SimDateTime SelectNearestSnapshot(SimDateTime sdt)
        {
            List<SimDateTime> snapshots = AvailableSnapshots;
            if (snapshots != null && snapshots.Count > 0)
            {
                if (snapshots.Contains(sdt))
                    return sdt;

                int minOffset = 32768;

                foreach (SimDateTime val in snapshots)
                {
                    int offset = Math.Abs(val.GetHoursOffset(sdt));
                    if (offset < minOffset)
                        minOffset = offset;
                }

                foreach (SimDateTime val in snapshots)
                {
                    int offset = Math.Abs(val.GetHoursOffset(sdt));
                    if (offset == minOffset)
                        return val;
                }

                return snapshots[0];
            }

            return null;
        }

        public static bool DataCategoryExists(string category)
        {
            var list = GetDataFiles(category);
            return (list != null && list.Count > 0);
        }

    }
}
