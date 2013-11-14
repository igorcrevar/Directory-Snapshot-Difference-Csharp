using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff.DirFinder
{
    /// <summary>
    /// Actual dirFinders should be executed through this class
    /// </summary>
    class DirFinderExecutor
    {
        private IDirFinder dirFinder;
        private long executionTimeInMillis;
        
        public DirFinderExecutor(IDirFinder dirFinder)
        {
            this.dirFinder = dirFinder;
            this.executionTimeInMillis = 0;
        }

        /// <summary>
        /// Search directory recursively
        /// </summary>
        /// <param name="path"></param>
        /// <returns>RootDirItem instance</returns>
        public RootDirItem Search(string path)
        {
            var rootDirItem = new RootDirItem(path);
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            dirFinder.Search(path, rootDirItem);
            stopWatch.Stop();
            executionTimeInMillis = stopWatch.ElapsedMilliseconds;
            return rootDirItem;
        }

        public long ExecutionTimeInMilis
        {
            get
            {
                return executionTimeInMillis;
            }
        }
    }
}
