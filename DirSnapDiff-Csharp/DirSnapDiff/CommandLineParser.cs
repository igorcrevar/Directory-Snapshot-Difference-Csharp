using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ICrew.DirSnapDiff
{
    enum CommandRequestedEnum
    {
        SearchAndSave, Compare, Info, DetailedHelp
    }

    class CommandLineParser
    {
        public CommandLineParser(string[] args)
        {
            Init(args);
        }

        public void Init(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i].ToLower())
                {
                    case "-d": case "--directory":
                        if (i + 1 < args.Length)
                        {
                            SearchDirectory = args[++i];
                        }
                        break;
                    case "-s": case "--save":
                        if (i + 1 < args.Length)
                        {
                            SaveFilePath = args[++i];
                        }
                        break;
                    case "-c": case "--compare":
                        if (i + 1 < args.Length)
                        {
                            SnapshotFilePathToCompare = args[++i];
                        }
                        break;
                    case "--sortbysize":
                        SortDiffsBySize = true;
                        break;
                    case "-h": case "--help":
                        CommandRequested = CommandRequestedEnum.DetailedHelp;
                        // do not proccess anything more just show help
                        return;
                }
            }

            if (!string.IsNullOrEmpty(SnapshotFilePathToCompare))
            {
                if (!File.Exists(SnapshotFilePathToCompare))
                {
                    throw new CompareFileDoesNotExistException(SnapshotFilePathToCompare);
                }

                if (string.IsNullOrEmpty(SaveFilePath))
                {
                    throw new DirSnapDiffInvalidParameter("-s not specified");
                }

                CommandRequested = CommandRequestedEnum.Compare;
            }
            else if (!string.IsNullOrEmpty(SaveFilePath))
            {
                // set to current dir if not specify
                if (string.IsNullOrEmpty(SearchDirectory))
                {
                    SearchDirectory = Directory.GetCurrentDirectory();
                }
                else if (!Directory.Exists(SearchDirectory))
                {
                    throw new SearchDirectoryNotExistException(SearchDirectory);
                }

                CommandRequested = CommandRequestedEnum.SearchAndSave;
            }
            else
            {
                CommandRequested = CommandRequestedEnum.Info;
            }
        }

        public string SearchDirectory { get; private set; }
        public string SaveFilePath { get; private set; }
        public string SnapshotFilePathToCompare { get; private set; }
        public CommandRequestedEnum CommandRequested { get; private set; }
        public bool SortDiffsBySize { get; private set; }
    }
}
