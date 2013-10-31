using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff
{
    sealed class CompareFileDoesNotExistException : Exception
    {
        public CompareFileDoesNotExistException(string compareFilePath)
        {
            CompareFilePath = compareFilePath;
        }

        private string CompareFilePath { get; set; }

        public override string Message
        {
            get
            {
                return "File to compare with, does not exist - " + CompareFilePath;
            }
        }
    }

    sealed class SearchDirectoryNotExistException : Exception
    {
        public SearchDirectoryNotExistException(string searchDirPath)
        {
            SearchDirPath = searchDirPath;
        }

        private string SearchDirPath { get; set; }

        public override string Message
        {
            get
            {
                return "Directory where to perform search does not exist - " + SearchDirPath;
            }
        }
    }

    sealed class DirSnapDiffInvalidParameter : Exception
    {
        public DirSnapDiffInvalidParameter(string message) : base(message)
        {
        }
    }
}
