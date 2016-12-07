using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            build();
            string[] allline = File.ReadAllLines("config.txt");
            //line1:
            var a = Regex.Match(allline[0], @"From:(.*)").Groups[1];
            var b = Regex.Match(allline[2], @"Override:(.*)").Groups[1];
            var c = Regex.Match(allline[1], @"Folder:(.*)").Groups[1];
            if (!CheckPath(a.ToString()))
            {
                Console.WriteLine("Path khoong khop! check lai");
            }
            var path = Directory.GetCurrentDirectory();
            var from = a.ToString();
            try
            {
                if (!Directory.Exists(path + "\\" + c))
                    Directory.CreateDirectory(path + "\\" + c);
                else
                {
                    Directory.Delete(path + "\\" + c, true);
                }
                DirectoryCopy(from, path + "\\" + c, true);
            }
            catch (Exception)
            {
                Console.WriteLine("Co loi trong qua trinh copy file & folder");
            }
            Console.WriteLine("Copy Successful !!");
            Console.ReadLine();
        }

        static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        static bool CheckPath(string path)
        {
            return Directory.Exists(path);
        }
        static void build()
        {
            var path = Directory.GetCurrentDirectory();
            //E:\DRIVE\Visual Studio project\GenerateEntity\GenerateEntity\bin\Debug
            var g = Regex.Match(path, @"(\w+\\\w+)$").Groups;
            var sp = path.Replace(g[1].Value.ToString(), "");
            var files = Directory.GetFiles(sp);
            foreach (var item in files)
            {
                if (item.IndexOf(".txt") > 0)
                {
                    var nameFile = Regex.Match(item, @"(\\\w+\.\w+)$").Groups;
                    var i = path + nameFile[1];
                    File.Copy(item, i, true);
                }
            }
        }
    }
}
