using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff.DirFinder
{
    /// <summary>
    /// Base interface for all directory finders
    /// </summary>
    interface IDirFinder
    {
        /// <summary>
        /// Retrieve files and directories from path and populate then in dirItem.
        /// </summary>
        /// <param name="path">path where search starts</param>
        /// <param name="dirItem">DirItem instance that will be populated</param>
        void Search(string path, DirItem dirItem);
    }
}
