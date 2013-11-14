using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Threading;

namespace ICrew.DirSnapDiff.DirFinder
{
    /// <summary>
    /// Multithread retrieveing of dirs and files 
    /// </summary>
    class ParallelDirFinder : IDirFinder
    {
        private IDirAccessAbstraction dirAccess;

        public ParallelDirFinder(IDirAccessAbstraction dirAccess)
        {
            this.dirAccess = dirAccess;
        }

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
            for (int i = 0; i < filePaths.Length; ++i)
            {
                var item = new FileItem(filePaths[i]);
                parent.AddFile(item);
            }

            Task[] tasks = new Task[dirPaths.Length];
            for (int i = 0; i < dirPaths.Length; ++i)
            {
                var dirPath = dirPaths[i];
                var item = new DirItem(dirPath, parent.Level + 1);
                parent.AddDir(item);
                tasks[i] = Task.Factory.StartNew(() => Search(dirPath, item));
            }

            Task.WaitAll(tasks);
        }
    }
}
