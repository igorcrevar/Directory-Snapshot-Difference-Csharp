using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;

namespace ICrew.DirSnapDiff
{
    class DirFinder
    {
        public DirFinder()
        {
        }

        public RootDirItem Find(string path)
        {
            var item = new RootDirItem(path);
            Find(path, item);
            return item;
        }

        private void Find(string path, DirItem parent)
        {
            string[] filePaths;
            string[] dirPaths;
            try
            {
                filePaths = Directory.GetFiles(path);
                dirPaths = Directory.GetDirectories(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Try to access " + path + " " + ex.Message);
                return;
            }

            parent.Init(filePaths.Length, dirPaths.Length);
            foreach (string filePath in filePaths)
            {
                var item = new FileItem(filePath);
                parent.AddFile(item);
            }

            foreach (string dirPath in dirPaths)
            {
                var item = new DirItem(dirPath, parent.Level + 1);                
                Find(dirPath, item);
                parent.AddDir(item);
            }
        }

        private bool CanRead(string path)
        {
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.ListDirectory & rule.FileSystemRights) != FileSystemRights.ListDirectory) continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    return true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    return false;
            }

            return true;
        }
    }
}
