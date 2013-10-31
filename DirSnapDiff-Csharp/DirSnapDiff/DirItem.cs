using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ICrew.DirSnapDiff
{
    public class DirItem : Item
    {
        private int dirsCount = 0;
        private int filesCount = 0;
        private FileItem[] fileItems;
        private DirItem[] dirItems;

        public DirItem()
        {
        }

        public DirItem(string path, int level) : this()
        {
            Name = Path.GetFileName(path);
            Level = level;
        }

        [XmlAttribute("l")]
        public int Level { get; set; }

        [XmlArray("fs")]
        [XmlArrayItem("f", typeof(FileItem))]
        public FileItem[] Files 
        {
            get
            {
                return fileItems;
            }

            set
            {
                fileItems = value;
                filesCount = fileItems.Length;
            }
        }

        [XmlArray("ds")]
        [XmlArrayItem("d", typeof(DirItem))]
        public DirItem[] Directories 
        {
            get
            {
                return dirItems;
            }

            set
            {
                dirItems = value;
                dirsCount = dirItems.Length;
            }
        }

        /// <summary>
        /// Init file and dir arrays
        /// </summary>
        /// <param name="filesCount">number of files</param>
        /// <param name="dirsCount">number of directories</param>
        public void Init(int filesCount, int dirsCount)
        {
            fileItems = new FileItem[filesCount];
            dirItems = new DirItem[dirsCount];
            Size = 0;
            dirsCount = filesCount = 0;
        }

        public void AddDir(DirItem dirItem)
        {
            dirItems[dirsCount++] = dirItem;
            Size += dirItem.Size;
        }

        public void AddFile(FileItem fileItem)
        {
            fileItems[filesCount++] = fileItem;
            Size += fileItem.Size;
        }

        /// <summary>
        /// Finds differences between two dir items
        /// </summary>
        /// <param name="other">DirItem instance that will be compared with this instance</param>
        /// <returns>IEnumerbale of DiffItem represents all differences</returns>
        public IEnumerable<DiffItem> GetDiff(RootDirItem other)
        {
            return GetDiff(other, this.Name);
        }

        private IEnumerable<DiffItem> GetDiff(DirItem other, string pathSoFar)
        {
            List<DiffItem> diffs = new List<DiffItem>();
            // added or updated files
            var otherFilesMap = other.Files.ToDictionary(x => x, x => x);
            foreach (var file in Files)
            {
                FileItem otherFile;
                if (!otherFilesMap.TryGetValue(file, out otherFile))
                {
                    diffs.Add(new DiffItem()
                    {
                        Name = string.Format(@"{0}\{1}", pathSoFar, file.Name),
                        Size = file.Size,
                        Status = DiffItemStatus.New
                    });
                }
                else if (file.Size != otherFile.Size)
                {
                    diffs.Add(new DiffItem()
                    {
                        Name = string.Format(@"{0}\{1}", pathSoFar, file.Name),
                        Size = file.Size - otherFile.Size,
                        Status = DiffItemStatus.Old
                    });
                }
            }

            // removed files
            var thisFilesSet = new HashSet<FileItem>(Files);
            foreach (var file in other.Files)
            {
                if (!thisFilesSet.Contains(file))
                {
                    diffs.Add(new DiffItem()
                    {
                        Name = string.Format(@"{0}\{1}", pathSoFar, file.Name),
                        Size = file.Size,
                        Status = DiffItemStatus.Removed,
                    });
                }
            }

            // removed directories
            var thisDirsSet = new HashSet<DirItem>(Directories);
            foreach (var dir in other.Directories)
            {
                if (!thisDirsSet.Contains(dir))
                {
                    diffs.Add(new DiffItem()
                    {
                        Name = string.Format(@"{0}\{1}", pathSoFar, dir.Name),
                        Size = dir.Size,
                        Status = DiffItemStatus.Removed,
                    });
                }
            }

            // new or changed dirs
            var otherDirsMap = other.Directories.ToDictionary(x => x, x => x);
            foreach (var dir in Directories)
            {
                DirItem otherDir;
                if (!otherDirsMap.TryGetValue(dir, out otherDir))
                {
                    diffs.Add(new DiffItem()
                    {
                        Name = string.Format(@"{0}\{1}", pathSoFar, dir.Name),
                        Size = dir.Size,
                        Status = DiffItemStatus.New,
                    });
                }
                else if (otherDir.Size != dir.Size)
                {
                    diffs.AddRange(dir.GetDiff(otherDir, string.Format(@"{0}\{1}", pathSoFar, dir.Name)));
                }
            }

            return diffs;
        }

        public override bool Equals(object obj)
        {
            if (obj is DirItem)
            {
                var robj = (DirItem)obj;
                // directories are same if has same name and on same level
                return robj.Level == Level && robj.Name == Name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
