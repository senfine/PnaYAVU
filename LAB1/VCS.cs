using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Linq;

namespace ConsloleVCS
{
    class VCS
    {
        public static List<DirectoryVersion> DirectoryList = new List<DirectoryVersion>(); 
        public static DirectoryVersion ActiveDirectory { get; set; } 

        private bool HasMethod(string methodName)
        {
            var type = this.GetType();
            return type.GetMethod(methodName) != null;
        } 

        public void Start() 
        {
            if (File.Exists("data.txt")) 
            {
                DirectoryList = Json.Decode<List<DirectoryVersion>>(File.ReadAllText("data.txt"));
            }
        }

        public void Init(string parameter) 
        {
            if (Directory.Exists(parameter))
            {
                foreach (DirectoryVersion dir in DirectoryList)
                {
                    if (parameter == dir.Path)
                    {
                        Console.WriteLine("Путь уже инициализирован.");
                        return;
                    }
                }
                ActiveDirectory = new DirectoryVersion() {Path = parameter};
                ActiveDirectory.Init();
                DirectoryList.Add(ActiveDirectory);
                Console.WriteLine("Путь инициализирован. Папка добавлена в список.");
            }
            else
            {
                Console.WriteLine("Ошибка: Указанного пути не существует."); 
            }

        }

        public void Status() 
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); 
                return;
            }
            Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path);
            List<FileVersion> oldfiles = ActiveDirectory.FileList; 
            List<FileInfo> newfiles = new DirectoryInfo(ActiveDirectory.Path).GetFiles().ToList();
            List<FileVersion> files = new List<FileVersion>(); 
            foreach (FileInfo newfile in newfiles)
            {
                files.Add(new FileVersion() { Name = newfile.Name }); 
            }
            foreach (FileVersion file in oldfiles)
            {
                files.Add(new FileVersion() { Name = file.Name }); 
            }
            files = files.GroupBy(p => p.Name).Select(g => g.First()).ToList(); 
            foreach (FileVersion file in files) 
            {
                int indnew = newfiles.FindIndex(item => item.Name == file.Name); 
                int indold = oldfiles.FindIndex(item => item.Name == file.Name); 
                if (indnew >= 0 && indold == -1) 
                {
                    FileInfo nf = newfiles[indnew];
                    file.Name = nf.Name;
                    file.Size = nf.Length;
                    file.Created = nf.CreationTime.ToString();
                    file.Modified = nf.LastWriteTime.ToString();
                    file.Log(ConsoleColor.Green, file.ToString("<-- new", -1, "", "")); 
                    continue;
                }
                else if (indnew == -1 && indold >=0) 
                {
                    FileVersion of = oldfiles[indold];
                    of.Log(ConsoleColor.Red, of.ToString("<-- deleted"));
                    continue;
                }
                else 
                {
                    FileInfo nf = newfiles[indnew]; 
                    FileVersion of = oldfiles[indold];
                    double lsize = -1;
                    string lcreated = "";
                    string lmodified = "";
                    ConsoleColor color = ConsoleColor.Green;
                    if (of.Label == "<-- removed") 
                    {
                        color = ConsoleColor.Red;
                        of.Log(color, of.ToString(of.Label));
                        continue;
                    }
                    else
                    {
                        if (of.Size != nf.Length) 
                        {
                            lsize = nf.Length;
                            color = ConsoleColor.Red;
                        }
                        if (of.Created != nf.CreationTime.ToString()) 
                        {
                            lcreated = "<--" + nf.CreationTime.ToString(); 
                            color = ConsoleColor.Red;
                        }
                        if (of.Modified !=nf.LastWriteTime.ToString()) 
                        {
                            lmodified = "<--" + nf.LastWriteTime.ToString();
                            color = ConsoleColor.Red;
                        }
                        of.Log(color, of.ToString(of.Label, lsize, lcreated, lmodified)); 
                    }
                }

            }
        }

        public void Add(string parameter) 
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); 
                return;
            }
            string dirpath = ActiveDirectory.Path;
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            if (!parameter.Contains(dirpath)) parameter = parameter.Insert(0, dirpath + "\\"); 
            if (File.Exists(parameter)) 
            {
                FileInfo file = new FileInfo(parameter);
                List<FileVersion> FileList = ActiveDirectory.FileList;
                foreach (FileVersion checkfile in FileList)
                {
                    if (checkfile.Name == file.Name)
                    {
                        if (checkfile.Label != "<-- removed") 
                        {
                            Console.WriteLine("Указанный файл уже находится под версионным контролем."); 
                            return; 
                        }
                        else 
                        {
                            checkfile.Size = file.Length;
                            checkfile.Created = file.CreationTime.ToString();
                            checkfile.Modified = file.LastWriteTime.ToString();
                            checkfile.Label = "<-- added";
                            Console.WriteLine("Файл добавлен обратно в версионный контроль."); 
                            return;
                        }
                    }
                }
                FileList.Add(new FileVersion() 
                {
                    Name = file.Name,
                    Size = file.Length,
                    Created = file.CreationTime.ToString(),
                    Modified = file.LastWriteTime.ToString(),
                    Label = "<-- added"
                });
                Console.WriteLine("Новый файл добавлен в версионный контроль."); 
            }
            else Console.WriteLine("Ошибка: Файл не найден."); 
        }

        public void Remove(string parameter) 
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); 
                return;
            }
            string dirpath = ActiveDirectory.Path;
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            if (!parameter.Contains(dirpath)) parameter = parameter.Insert(0, dirpath + "\\");
            if (File.Exists(parameter))
            {
                FileInfo file = new FileInfo(parameter);
                List<FileVersion> FileList = ActiveDirectory.FileList;
                foreach (FileVersion checkfile in FileList)
                {
                    if (checkfile.Name == file.Name)
                    {
                        if (checkfile.Label == "<-- removed") 
                        {
                            Console.WriteLine("Файл уже убран из версионного контроля"); 
                            return;
                        }
                        else
                        {
                            checkfile.Label = "<-- removed";
                            Console.WriteLine("Файл убран из версионного контроля"); 
                        }
                    }
                }
            }
            else Console.WriteLine("Ошибка: Файл не найден."); 
        }

        public void Apply()
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); 
                return;
            }
            List<string> removed = new List<string>();
            foreach (FileVersion file in ActiveDirectory.FileList)
            {
                if (file.Label == "<-- removed")
                {
                    removed.Add(file.Name);
                }
            }
            ActiveDirectory.FileList.Clear();
            ActiveDirectory.Init(removed.ToArray());
            Console.WriteLine("Сохранены все изменения в папке: {0}", ActiveDirectory.Path);
            return;
        }

        public void Listbranch()
        {
            if (DirectoryList.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            Console.WriteLine("Список отслеживаемых папок:");
            int dirnumber = 1;
            foreach (DirectoryVersion dir in DirectoryList)
            {
                Console.WriteLine("{0}) {1}", dirnumber++, dir.Name);
            }
        }

        public void Checkout(string parameter) 
        {
            if (int.TryParse(parameter, out int i))
            {
                ActiveDirectory = DirectoryList[i - 1]; 
                Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path); 
                return;
            }
            else if (!Directory.Exists(parameter)) 
            {
                Console.WriteLine("Ошибка: Указанный путь не существует."); 
                return;
            }
            else
            {
                int inddir = DirectoryList.FindIndex(item => item.Path == parameter);
                if (inddir == -1)
                {
                    Console.WriteLine("Ошибка: Данная папка не инициализирована."); 
                    return;
                }
                ActiveDirectory = DirectoryList[inddir];
                Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path); 
                return;
            }
        }

        public void Help() 
        {
            Console.WriteLine("Список команд:"); 
            Console.WriteLine("Init [dir_path] — инициализация СКВ для папки, путь к которой указан в dir_path."); 
            Console.WriteLine("Status — отображение статуса отслеживаемых файлов текущей отслеживаемой директории."); 
            Console.WriteLine("Add [file_path] — добавить файл под версионный контроль."); 
            Console.WriteLine("Remove [file_path] – удалить файл из-под версионного контроля."); 
            Console.WriteLine("Apply [dir_path] – сохранить все изменения в отслеживаемой папке (удалить все метки к файлам и сохранить изменения в них)."); 
            Console.WriteLine("Listbranch -  показать все отслеживаемые папки."); 
            Console.WriteLine("Checkout [dir_path] OR [dir_number] – перейти к указанной отслеживаемой директории"); 
        }

        public void Exit()
        {
            File.WriteAllText("data.txt", Json.Encode(DirectoryList));
            Environment.Exit(1);
        }

        public void ReadCommand(string command, string parameter = "") 
        {
            if (String.IsNullOrEmpty(command)) return; 
            if (parameter == "Start") return;
            if (!this.HasMethod(command))
            {
                Console.WriteLine("Ошибка: Нет команды с именем \"{0}\".", command);
                return;
            }
            else
            {
                MethodInfo method = this.GetType().GetMethod(command); 
                try
                {
                    if (String.IsNullOrEmpty(parameter)) 
                        method.Invoke(this, null);
                    else 
                        method.Invoke(this, new[] { parameter });
                }
                catch (TargetParameterCountException e)
                {
                    Console.WriteLine("Ошибка: {0}", e.Message); 
                    return;
                }
            }
            return;
        }
    }
}
