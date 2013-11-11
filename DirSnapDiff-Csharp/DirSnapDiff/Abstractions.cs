using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff
{
    /// <summary>
    /// Abstracts reading and writing to file
    /// </summary>
    public interface IReaderWriter
    {
        TextWriter GetWriter(string path);
        TextReader GetReader(string path);
        void WriteAllText(string path, string text);
    }

    /// <summary>
    /// Abstracts retrieving of files and directories for some dir
    /// </summary>
    interface IDirAccessAbstraction
    {
        string[] GetFiles(string path);
        string[] GetDirectories(string path);
    }
}
