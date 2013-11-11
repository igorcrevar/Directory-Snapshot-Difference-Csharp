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
        private class ReaderWriterDefault : IReaderWriter
        {
            public TextWriter GetWriter(string path)
            {
                return new StreamWriter(path);
            }

            public TextReader GetReader(string path)
            {
                return new StreamReader(path);
            }

            public void WriteAllText(string path, string text)
            {
                File.WriteAllText(path, text, Encoding.UTF8);
            }
        }

        private class DirAccessDefault : IDirAccessAbstraction
        {
            public string[] GetFiles(string path)
            {
                return Directory.GetFiles(path);
            }

            public string[] GetDirectories(string path)
            {
                return Directory.GetDirectories(path);
            }
        }

        static void Info()
        {
            Console.WriteLine(" Directory Snapshot Difference by Igor Crevar");
            Console.WriteLine();
            Console.WriteLine(" dirsnap -c snapshot_file_path [diff_file_path] [--sortbysize] " +
                                "[-s file_path_to_search [snapshot_file_path]]");
            Console.WriteLine();
            Console.WriteLine(" dirsnap -s directory_path [snapshot_file_path]");
            Console.WriteLine();
            Console.WriteLine("     * snapshot_file_path = path where is old snapshot for comparing");
            Console.WriteLine("     * diff_file_path = path where difference will be saved. if not specified path to directoty is inside snapshot file(same directory will be compared)");
            Console.WriteLine("     * --sortbysize = sort by size, otherwise differences will be sorted by name");
            Console.WriteLine("     * directory_path = directory that will be examined");
            Console.WriteLine("     * snapshot_file_path = file where current snapshot will be saved");
            Console.WriteLine("     If -c switch is used -s can be ommited. In that case snapshot directory is read from snapshot_file_path");
            Console.WriteLine();
        }

        private static void Execute(CommandLineParams clParams, bool isGetSnapshot, bool isCompare)
        {
            var dirFinder = new DirFinder(new DirAccessDefault());
            var readerWriter = new ReaderWriterDefault();
            CommandParameters param = new CommandParameters(dirFinder, readerWriter, clParams, null);
            if (isGetSnapshot)
            {
                new GetSnashotCommand().Execute(param);
            }

            if (isCompare)
            {
                new CompareSnapshotCommand().Execute(param);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                var clp = new CommandLineParser(args);
                if (clp.IsHelp())
                {
                    Info();
                }
                else if (clp.IsGetSnapshot() || clp.IsCompareSnapshot())
                {
                    Execute(clp.Parameters, clp.IsGetSnapshot(), clp.IsCompareSnapshot());
                }
                else
                {
                    Info(); // currently same as help but help should provide descriptive info
                }
            }
            catch (InvalidParameterExcetion ex)
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
