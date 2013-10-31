using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ICrew.DirSnapDiff
{
    public class FileItem : Item
    {
        public FileItem()
        {
        }

        public FileItem(string path)
        {
            var fi = new System.IO.FileInfo(path);
            Name = fi.Name;
            Size = fi.Length;
        }
    }
}
