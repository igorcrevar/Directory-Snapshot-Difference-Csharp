using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ICrew.DirSnapDiff
{
    public class RootDirItem : DirItem
    {
        public RootDirItem() : base()
        {
            Level = 0;
        }

        public RootDirItem(string path)
            : base()
        {
            Name = path;
            Level = 0;
        }

        public void Save(IReaderWriter readerWriter, string path)
        {
            var serializer = new XmlSerializer(typeof(RootDirItem));
            using (var writer = readerWriter.GetWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }

        public void Load(IReaderWriter readerWriter, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootDirItem));
            using (var reader = readerWriter.GetReader(path))
            {
                var item = (RootDirItem)serializer.Deserialize(reader);
                this.Name = item.Name;
                this.Files = item.Files;
                this.Level = item.Level;
                this.Directories = item.Directories;
            }
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
    }
}
