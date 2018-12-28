using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsloleVCS
{
    class DirectoryVersion
    {
        public string Path { get; set; } 

        public string Name 
        {
            get
            {
                DirectoryInfo dir = new DirectoryInfo(Path); 
                return dir.Name;
            }
        }

        public List<FileVersion> FileList = new List<FileVersion>(); 

        public void Init(params string[] parameters)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileInfo[]files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                if (!parameters.Contains(file.Name))
                    FileList.Add(new FileVersion()
                    {
                        Name = file.Name,
                        Size = file.Length,
                        Created = file.CreationTime.ToString(),
                        Modified = file.LastWriteTime.ToString(), 
                        Label = ""
                    });
            }
        }

    }
}
