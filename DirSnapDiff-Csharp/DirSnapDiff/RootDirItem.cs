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

        public void Save(string path)
        {
            var serializer = new XmlSerializer(typeof(RootDirItem));
            using (var writer = new System.IO.StreamWriter(path))
            {
                serializer.Serialize(writer, this);
            }
        }

        public void Load(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootDirItem));
            using (var reader = new System.IO.StreamReader(path))
            {
                var item = (RootDirItem)serializer.Deserialize(reader);
                this.Name = item.Name;
                this.Files = item.Files;
                this.Level = item.Level;
                this.Directories = item.Directories;
            }
        }

    }
}
