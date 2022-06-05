using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        }

        public void TransferAllToDestination(bool enableOverwrite = false)
        {

        }

        public void TransferMissingToDestination()
        {

        }

        #endregion
    }

    public class FileItem
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string DirectoryPath { get; set; }
        public bool IsHighlighted { get; set; }
        public string FullPath 
        {
            get
            {
                return Path.Combine(DirectoryPath, string.Format("{0}.{1}", FileName, Extension));
            }
        }
        public string FullPathWithoutExtension
        {
            get
            {
                return Path.Combine(DirectoryPath, FileName);
            }
        }
        public FileItem()
        {
            FileName = "";
            Extension = "";
            DirectoryPath = "";
            IsHighlighted = false;
        }
        public FileItem(string fileName, string extension, string directoryPath, bool isHighlighted)
        {
            FileName = fileName;
            Extension = extension;
            DirectoryPath = directoryPath;
            IsHighlighted = isHighlighted;
        }
    }
}
