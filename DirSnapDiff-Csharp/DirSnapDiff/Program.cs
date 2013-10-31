using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ICrew.DirSnapDiff
{
    class Program
    {
        static void Info()
        {
            Console.WriteLine(" Directory Snapshot Difference by Igor Crevar");
            Console.WriteLine();
            Console.WriteLine(" dirsnap -s snapshot_file_path [-d directory_path]");
            Console.WriteLine();
            Console.WriteLine("     * snapshot_file_path = path where snapshot file will be saved");
            Console.WriteLine("     * directory_path = path that will be examined. Defaults to current path");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(" dirsnap -c snapshot_file_path -s diff_file_path [-d directory_path] [--sortbysize]");
            Console.WriteLine();
            Console.WriteLine("     * snapshot_file_path = path where is old snapshot for comparing");
            Console.WriteLine("     * diff_file_path = path where difference will be saved. if not specified path to directoty is inside snapshot file(same directory will be compared)");
            Console.WriteLine("     * directory_path = directory that will be examined");
            Console.WriteLine("     * --sortbysize = sort by size, otherwise differences will be sorted by name");
            Console.WriteLine();
        }

        static void SearchAndSave(CommandLineParser clp)
        {
            var ds = new DirFinder();
            var root = ds.Find(clp.SearchDirectory);
            root.Save(clp.SaveFilePath);
        }

        static void Compare(CommandLineParser clp)
        {
            // load old snapshot
            var oldRoot = new RootDirItem();
            oldRoot.Load(clp.SnapshotFilePathToCompare);

            // find new snapshot
            var ds = new DirFinder();
            var root = ds.Find(string.IsNullOrEmpty(clp.SearchDirectory) ? oldRoot.Name : clp.SearchDirectory);

            // find diff and save it
            var diffs = root.GetDiff(oldRoot).ToList();
            long size = diffs.Aggregate(0L, (acc, item) => acc + item.Size);

            var result = new StringBuilder("Size: ");
            result.AppendLine(size.ToString());
            
            if (clp.SortDiffsBySize)
            {
                diffs.Sort(new DiffItemSizeComparer());
            }
            else
            {
                diffs.Sort(new DiffItemDefaultComparer());
            }

            result.Append(string.Join(Environment.NewLine, diffs));
            File.WriteAllText(clp.SaveFilePath, result.ToString(), Encoding.UTF8);
        }

        static void Main(string[] args)
        {
            try
            {
                var clp = new CommandLineParser(args);
                switch (clp.CommandRequested)
                {
                    case CommandRequestedEnum.Info:
                        Info();
                        break;
                    case CommandRequestedEnum.DetailedHelp:
                        Info();
                        break;
                    case CommandRequestedEnum.Compare:
                        Compare(clp);
                        break;
                    case CommandRequestedEnum.SearchAndSave:
                        SearchAndSave(clp);
                        break;
                }
            }
            catch (SearchDirectoryNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CompareFileDoesNotExistException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (DirSnapDiffInvalidParameter ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
