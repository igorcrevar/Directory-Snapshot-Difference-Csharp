﻿using ICrew.DirSnapDiff.DirFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICrew.DirSnapDiff
{
    class CommandParameters
    {
        public CommandParameters(DirFinderExecutor dirFinderExecutor, IReaderWriter readerWritter, 
                                 CommandLineParams clParams, RootDirItem rootItem)
        {
            this.DirFinderExecutor = dirFinderExecutor;
            this.ReaderWriter = readerWritter;
            this.CLParams = clParams;
            this.RootItem = rootItem;
        }

        public DirFinderExecutor DirFinderExecutor { get; private set; }
        public IReaderWriter ReaderWriter { get; private set; }
        public CommandLineParams CLParams { get; private set; }
        public RootDirItem RootItem { get; set; }
    }

    interface ICommand
    {
        void Execute(CommandParameters param);
    }

    class GetSnashotCommand : ICommand
    {
        public void Execute(CommandParameters param)
        {
            var dirItem = param.DirFinderExecutor.Search(param.CLParams.SnapshotSearchDirectory);
            
            var savePath = param.CLParams.SnapshotSaveFilePath;
            if (!string.IsNullOrWhiteSpace(savePath))
            {
                dirItem.Save(param.ReaderWriter, savePath);
            }

            // update root item in params for later commands
            param.RootItem = dirItem;
        }
    }

    class CompareSnapshotCommand : ICommand
    {
        public void Execute(CommandParameters param)
        {
            // load root dir to compare
            var oldRootItem = new RootDirItem();
            oldRootItem.Load(param.ReaderWriter, param.CLParams.CompareSnapshotFilePath);

            var newRootItem = param.RootItem;
            // if new root item to compare is not passed, search dir for items in oldRootItem.Name
            if (newRootItem == null)
            {
                newRootItem = param.DirFinderExecutor.Search(oldRootItem.Name);
            }

            // find diff and save it
            var diffs = newRootItem.GetDiff(oldRootItem).ToList();
            long size = diffs.Aggregate(0L, (acc, item) => acc + item.Size);

            var result = new StringBuilder("Size: ");
            result.AppendLine(size.ToString());

            if (param.CLParams.DiffSortBySize)
            {
                diffs.Sort(new DiffItemSizeComparer());
            }
            else
            {
                diffs.Sort(new DiffItemDefaultComparer());
            }

            result.Append(string.Join(Environment.NewLine, diffs));
            param.ReaderWriter.WriteAllText(param.CLParams.DiffSaveFilePath, result.ToString());
        }
    }
}
