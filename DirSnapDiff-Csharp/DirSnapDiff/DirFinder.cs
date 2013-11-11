using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace ICrew.DirSnapDiff
{
    class DirFinder
    {
        private IDirAccessAbstraction dirAccess;

        public DirFinder(IDirAccessAbstraction dirAccess)
        {
            this.dirAccess = dirAccess;
        }

        /// <summary>
        /// Reursive method to populate DIrItem with its children(files and subdirectories)
        /// </summary>
        /// <param name="path">path to search</param>
        /// <param name="parent">DirItem instance. It will be populated with child files and dirs</param>
        public void Search(string path, DirItem parent)
        {
            string[] filePaths;
            string[] dirPaths;
            try
            {
                filePaths = dirAccess.GetFiles(path);
                dirPaths = dirAccess.GetDirectories(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Try to access " + path + " " + ex.Message);
                return;
            }

            parent.Init(filePaths.Length, dirPaths.Length);
            foreach (string filePath in filePaths)
            {
                var item = new FileItem(filePath);
                parent.AddFile(item);
            }

            foreach (string dirPath in dirPaths)
            {
                var item = new DirItem(dirPath, parent.Level + 1);
                Search(dirPath, item);
                parent.AddDir(item);
            }
        }
    }
}
