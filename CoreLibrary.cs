using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace WinDirectoryCompare
{
    internal class CoreLibrary
    {
        #region Properties

        /// <summary>
        /// Directory path of source.
        /// </summary>
        public string SourcePath { get; set; }
        /// <summary>
        /// File items from source directory.
        /// </summary>
        public List<FileItem> SourceFiles { get; set; }
        /// <summary>
        /// Directory path of destination.
        /// </summary>
        public string DestinationPath { get; set; }
        /// <summary>
        /// File items from destination directory.
        /// </summary>
        public List<FileItem> DestinationFiles { get; set; }

        #endregion

        #region Constructor

        public CoreLibrary() 
        {
            SourcePath = "";
            DestinationPath = "";
            SourceFiles = new List<FileItem>();
            DestinationFiles = new List<FileItem>();
        }

        #endregion

        #region Methods

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

        public void TransferMissingToDestination(out StringBuilder errMsg)
        {
            errMsg = new StringBuilder();
            List<FileItem> thoseMissingFiles = SourceFiles.FindAll(x => x.IsMissing == true);
            if (thoseMissingFiles != null)
            {
                foreach (FileItem fileItem in thoseMissingFiles)
                {
                    try
                    {
                        string newDestinationFile = Path.Combine(DestinationPath, fileItem.FileNameWithExtension);
                        File.Move(fileItem.FullPath, newDestinationFile);
                    }
                    catch (Exception ex)
                    {
                        errMsg.AppendLine(ex.Message);
                    }
                }
            }
        }

        #endregion
    }

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
