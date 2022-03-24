using RegRipperGUI.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegRipperGUI.Handlers
{
    public class FileHandler
    {
        public static DTOs.File Process(string path, Enumerations.ProcessMode Mode = Enumerations.ProcessMode.Parallel, DTOs.File parent = null)
        {
            DirectoryInfo folderBase = null;
            DTOs.File root = null;
            DTOs.File current = null;

            try
            {
                if ((parent != null))
                {
                    root = parent;
                }
                else
                {
                    root = new DTOs.File
                    {
                        Name = System.IO.Path.GetFileName(path),
                        PathFullName = path,
                        PathRelative = "",
                        IsFolder = true,
                        Size = GetFileSizeSumFromDirectory(path),
                        Icon = "Folder",
                        IsExpanded = true
                    };
                }
                if ((root.Childs == null))
                {
                    root.Childs = new List<DTOs.File>();
                }

                if (Mode == ProcessMode.Parallel)
                {
                    folderBase = new DirectoryInfo(path);
                    foreach (FileInfo file in folderBase.GetFiles())
                    {
                        root.Childs.Add(new DTOs.File
                        {
                            Name = file.Name,
                            PathFullName = file.FullName,
                            PathRelative = file.FullName.Replace(path, string.Empty),
                            IsFolder = false,
                            LastWrite = file.LastWriteTime.ToLocalTime(),
                            Size = file.Length,
                            Icon = "File"
                        });
                    }

                    foreach (DirectoryInfo directory in folderBase.GetDirectories())
                    {
                        current = new DTOs.File
                        {
                            Name = System.IO.Path.GetFileName(directory.Name),
                            PathFullName = directory.FullName,
                            PathRelative = directory.FullName.Replace(path, string.Empty),
                            IsFolder = true,
                            LastWrite = directory.LastWriteTime.ToLocalTime(),
                            Size = GetFileSizeSumFromDirectory(path),
                            Icon = "Folder"
                        };
                        root.Childs.Add(current);
                        Process(directory.FullName, Mode, current);
                    }
                }
                else
                {
                    string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    if (files.Length != 0)
                    {
                        foreach (var item in files)
                        {
                            var directory = new DirectoryInfo(item);
                            var file = new FileInfo(item);
                            current = new DTOs.File
                            {
                                Name = System.IO.Path.GetFileName(item),
                                PathFullName = item,
                                PathRelative = item.Replace(path, string.Empty),
                                IsFolder = true,
                                LastWrite = directory.LastWriteTime.ToLocalTime(),
                                Size = file.Length
                            };

                            root.Childs.Add(current);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                root.Name = "Falla: " + ex.Message;
            }
            return root;
        }

        public static double GetFileSizeSumFromDirectory(string searchDirectory)
        {
            var files = Directory.EnumerateFiles(searchDirectory);

            // get the sizeof all files in the current directory
            var currentSize = (from file in files let fileInfo = new FileInfo(file) select fileInfo.Length).Sum();

            var directories = Directory.EnumerateDirectories(searchDirectory);

            // get the size of all files in all subdirectories
            var subDirSize = (from directory in directories select GetFileSizeSumFromDirectory(directory)).Sum();

            return currentSize + subDirSize;
        }
    }
}