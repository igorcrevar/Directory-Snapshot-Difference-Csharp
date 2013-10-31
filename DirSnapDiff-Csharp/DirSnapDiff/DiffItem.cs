using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff
{
    public enum DiffItemStatus : short 
    { 
        New = 0, Old = 1, Removed = 2 
    };
    
    public class DiffItem
    {
        public DiffItem()
        {
        }

        public string Name { get; set; }
        public long Size { get; set; }
        public DiffItemStatus Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Name, Size, Status.ToString());
        }
    }

    public class DiffItemSizeComparer : IComparer<DiffItem>
    {
        public int Compare(DiffItem x, DiffItem y)
        {
            // greater size should be first
            int size = y.Size.CompareTo(x.Size);
            if (size != 0)
            {
                return size;
            }

            int status = x.Status.CompareTo(y.Status);
            if (status != 0)
            {
                return status;
            }

            return x.Name.CompareTo(y.Name);
        }
    }

    public class DiffItemDefaultComparer : IComparer<DiffItem>
    {
        public int Compare(DiffItem x, DiffItem y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
