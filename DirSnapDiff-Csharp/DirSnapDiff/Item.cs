using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ICrew.DirSnapDiff
{
    public class Item : IComparable<Item>
    {
        public Item()
        {
        }

        [XmlAttribute("n")]
        public string Name { get; set; }

        [XmlAttribute("s")]
        public long Size { get; set; }
       
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Item)
            {
                var robj = (Item)obj;
                return robj.Name == Name;
            }

            return false;
        }

        public int CompareTo(Item other)
        {
            return Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
