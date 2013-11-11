using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ICrew.DirSnapDiff
{
    [Flags]
    enum CommandRequestedEnum
    {
        None = 0,
        GetSnapshot = 1,
        CompareSnapshots = 2,
        Help = 4
    }

    class CommandLineParams
    {
        public string SnapshotSearchDirectory { get; internal set; }
        public string SnapshotSaveFilePath { get; internal set; }

        public string CompareSnapshotFilePath { get; internal set; }
        public bool DiffSortBySize { get; internal set; }
        public string DiffSaveFilePath { get; internal set; }
    }

    class CommandLineParser
    {
        private CommandLineParams parameters;
        private CommandRequestedEnum commandRequested;

        public CommandLineParser(string[] args)
        {
            parameters = new CommandLineParams();
            Init(args);
        }

        private void HandleSearch(string[] args, ref int i)
        {
            if (i + 1 < args.Length)
            {
               parameters.SnapshotSearchDirectory = args[++i];
               if (!Directory.Exists(parameters.SnapshotSearchDirectory))
                {
                    throw new InvalidParameterExcetion("Snapshot search directory does not exist: " +
                                                        parameters.SnapshotSearchDirectory);
                }
            }
            else
            {
                throw new InvalidParameterExcetion("Snapshot search directory not specified");
            }
        }

        private void HandleCompare(string[] args, ref int i)
        {
            if (i + 1 < args.Length)
            {
                parameters.CompareSnapshotFilePath = args[++i];
                if (!File.Exists(parameters.CompareSnapshotFilePath))
                {
                    throw new InvalidParameterExcetion("Compare snapshot file does not exist: " +
                                                        parameters.SnapshotSearchDirectory);
                }
            }
            else
            {
                throw new InvalidParameterExcetion("Compare snapshot file not specified");
            }
        }

        private bool FileCanBeSaved(string filePath)
        {
            try
            {
                return Directory.Exists(Path.GetDirectoryName(filePath)) &&
                       !string.IsNullOrWhiteSpace(Path.GetFileName(filePath));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void HandleOther(string arg, CommandRequestedEnum state)
        {
            switch (state)
            {
                case CommandRequestedEnum.GetSnapshot:
                    parameters.SnapshotSaveFilePath = arg;
                    if (!FileCanBeSaved(arg))
                    {
                        throw new InvalidParameterExcetion("Invalid parameter: " + arg);
                    }
                    break;
                case CommandRequestedEnum.CompareSnapshots:
                    parameters.DiffSaveFilePath = arg;
                    if (!FileCanBeSaved(arg))
                    {
                        throw new InvalidParameterExcetion("Invalid parameter: " + arg);
                    }
                    break;
            }
        }

        public void Init(string[] args)
        {
            var currentState = CommandRequestedEnum.None;
            commandRequested = CommandRequestedEnum.None;
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i].ToLower())
                {
                    case "-s": case "--search":
                        HandleSearch(args, ref i);
                        commandRequested |= CommandRequestedEnum.GetSnapshot;
                        currentState = CommandRequestedEnum.GetSnapshot;
                        break;
                    case "-c": case "--compare":
                        HandleCompare(args, ref i);
                        commandRequested |= CommandRequestedEnum.CompareSnapshots;
                        currentState = CommandRequestedEnum.CompareSnapshots;
                        break;
                    case "--sortbysize":
                        parameters.DiffSortBySize = true;
                        break;
                    case "-h": case "--help":
                        commandRequested = CommandRequestedEnum.Help;
                        // do not proccess anything more just show help
                        return;
                    default:
                        HandleOther(args[i], currentState);
                        break;
                }
            }

            if ((commandRequested & CommandRequestedEnum.CompareSnapshots) == 0 && parameters.DiffSortBySize)
            {
                throw new InvalidParameterExcetion("Sort by size specified but snapshot to compare doesnt");
            }
        }

        public bool IsGetSnapshot()
        {
            return (commandRequested & CommandRequestedEnum.GetSnapshot) > 0;
        }

        public bool IsCompareSnapshot()
        {
            return (commandRequested & CommandRequestedEnum.CompareSnapshots) > 0;
        }

        public bool IsHelp()
        {
            return commandRequested == CommandRequestedEnum.Help;
        }

        public CommandLineParams Parameters
        {
            get
            {
                return parameters;
            }
        }
    }
}
