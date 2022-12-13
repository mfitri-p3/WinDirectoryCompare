using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WinDirectoryCompare
{
    /// <summary>
    /// Core library for all core functionalities.
    /// </summary>
    public class CoreLibrary : INotifyPropertyChanged
    {
        #region Properties

        private string _sourcePath;
        /// <summary>
        /// Directory path of source.
        /// </summary>
        public string SourcePath 
        { 
            get
            {
                return _sourcePath;
            }
            set
            {
                _sourcePath = value;
                if (Directory.Exists(_sourcePath) && !string.IsNullOrEmpty(_sourcePath))
                {
                    SourceFiles.Clear();
                    List<string> currentFiles = Directory.GetFiles(_sourcePath).ToList();
                    if (currentFiles != null)
                    {
                        foreach (var file in currentFiles)
                        {
                            FileItem freshItem = new FileItem(Path.GetFileNameWithoutExtension(file), Path.GetExtension(file), _sourcePath);
                            SourceFiles.Add(freshItem);
                        }
                    }
                }
                OnPropertyChanged("SourcePath");
            }
        }
        /// <summary>
        /// File items from source directory.
        /// </summary>
        public ObservableCollection<FileItem> SourceFiles { get; set; }
        private string _destinationPath;
        /// <summary>
        /// Directory path of destination.
        /// </summary>
        public string DestinationPath 
        { 
            get
            {
                return _destinationPath;
            }
            set
            {
                _destinationPath = value;
                if (Directory.Exists(_destinationPath) && !string.IsNullOrEmpty(_destinationPath))
                {
                    DestinationFiles.Clear();
                    List<string> currentFiles = Directory.GetFiles(_destinationPath).ToList();
                    if (currentFiles != null)
                    {
                        foreach (var file in currentFiles)
                        {
                            FileItem freshItem = new FileItem(Path.GetFileNameWithoutExtension(file), Path.GetExtension(file), _destinationPath);
                            DestinationFiles.Add(freshItem);
                        }
                    }
                }
                OnPropertyChanged("DestinationPath");
            }
        }
        /// <summary>
        /// File items from destination directory.
        /// </summary>
        public ObservableCollection<FileItem> DestinationFiles { get; set; }

        #endregion

        #region Constructor

        public CoreLibrary() 
        {
            SourcePath = "";
            DestinationPath = "";
            SourceFiles = new ObservableCollection<FileItem>();
            DestinationFiles = new ObservableCollection<FileItem>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check for difference(s) for files between two folders.
        /// </summary>
        public void FindDifference()
        {
            //Highlight files that are missing in the destination in comparison from the source.
            foreach (var file in SourceFiles)
            {
                bool isFileExists = DestinationFiles.Any(x => x.FileName.Equals(file.FileName));
                if (!isFileExists)
                {
                    file.IsMissing = true;
                }
            }

            //Highlight files that are new from the source in the destination.
            foreach (var file in DestinationFiles)
            {
                bool isFileExists = SourceFiles.Any(x => x.FileName.Equals(file.FileName));
                if (!isFileExists)
                {
                    file.IsNew = true;
                }
            }
        }
        /// <summary>
        /// Do the file transfer for missing files on destination folder.
        /// </summary>
        /// <param name="errMsg">Error message.</param>
        public void TransferMissingToDestination(out StringBuilder errMsg)
        {
            errMsg = new StringBuilder();
            List<FileItem> thoseMissingFiles = SourceFiles.Where(x => x.IsMissing == true).ToList();
            if (thoseMissingFiles != null)
            {
                foreach (FileItem fileItem in thoseMissingFiles)
                {
                    try
                    {
                        string newDestinationFile = Path.Combine(DestinationPath, fileItem.FileNameWithExtension);
                        File.Copy(fileItem.FullPath, newDestinationFile);
                        fileItem.DirectoryPath = DestinationPath;
                        DestinationFiles.Add(fileItem);
                    }
                    catch (Exception ex)
                    {
                        errMsg.AppendLine(ex.Message);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
    /// <summary>
    /// File item class.
    /// </summary>
    public class FileItem : INotifyPropertyChanged
    {
        #region Properties

        private string _fileName;
        public string FileName 
        { 
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }
        private string _extension;
        public string Extension 
        { 
            get
            {
                return _extension;
            }
            set
            {
                _extension = value;
                OnPropertyChanged("Extension");
            }
        }
        private string _directoryPath;
        public string DirectoryPath 
        { 
            get
            {
                return _directoryPath;
            }
            set
            {
                _directoryPath = value;
                OnPropertyChanged("DirectoryPath");
            }
        }
        private bool _isMissing;
        public bool IsMissing
        {
            get
            {
                return _isMissing;
            }
            set
            {
                _isMissing = value;
                OnPropertyChanged("IsMissing");
            }
        }
        private bool _isNew;
        public bool IsNew
        {
            get
            {
                return _isNew;
            }
            set
            {
                _isNew = value;
                OnPropertyChanged("IsNew");
            }
        }
        public string FullPath 
        {
            get
            {
                if (_extension.Contains('.'))
                {
                    return Path.Combine(DirectoryPath, string.Format("{0}{1}", FileName, Extension));
                }
                else
                {
                    return Path.Combine(DirectoryPath, string.Format("{0}.{1}", FileName, Extension));
                }
            }
        }
        public string FullPathWithoutExtension
        {
            get
            {
                return Path.Combine(DirectoryPath, FileName);
            }
        }
        public string FileNameWithExtension
        {
            get
            {
                return FileName + "." + Extension;
            }
        }

        #endregion

        #region Constructor

        public FileItem() { }
        public FileItem(string fileName, string extension, string directoryPath)
        {
            FileName = fileName;
            Extension = extension;
            DirectoryPath = directoryPath;
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));  
        }

        #endregion
    }
}
