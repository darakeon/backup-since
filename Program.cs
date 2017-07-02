using System;
using System.Configuration;
using System.Linq;
using System.IO;

namespace CopySinceDate
{
    class Program
    {
        static String mainPath = ConfigurationManager.AppSettings["MainPath"];

        static String[] analyzeFolders =
            ConfigurationManager.AppSettings["AnalyzeFolders"]
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

        static String backupFolder = ConfigurationManager.AppSettings["BackupFolder"];

        static Int32 copiedFiles = 0;

        public static void Main()
        {
            var init = DateTime.MinValue;

            while(init == DateTime.MinValue)
            {
                Console.Write("Insert a valid date: ");
                var date = Console.ReadLine();

                DateTime.TryParse(date, out init);
            }

            
            var dirs = Directory.GetDirectories(mainPath);

            foreach (var dir in dirs)
            {
                var dirName = dir.Replace(mainPath, "").Substring(1);

                if (!analyzeFolders.Contains(dirName))
                    continue;

                copyChangesInDir(dir, init);
            }


            Console.WriteLine();
            Console.WriteLine("Copied Files: {0}", copiedFiles);
            Console.ReadLine();
        }

        private static void copyChangesInDir(String mainDir, DateTime init)
        {
            var newDir = mainDir.Replace(mainPath, backupFolder);


            var dirs = Directory.GetDirectories(mainDir);

            if (isVersioned(mainDir, dirs))
                return;

            foreach (var dir in dirs)
            {
                copyChangesInDir(dir, init);
            }


            var files = Directory.GetFiles(mainDir);

            foreach (var file in files)
            {
                var info = new FileInfo(file);

                if (info.LastWriteTime < init)
                    continue;

                if (!Directory.Exists(newDir))
                    Directory.CreateDirectory(newDir);

                var newFile = file.Replace(mainDir, newDir);

                File.Copy(file, newFile);

                copiedFiles++;
            }


        }

        private static Boolean isVersioned(String mainDir, String[] dirs)
        {
            var hg = Path.Combine(mainDir, ".hg");
            var svn = Path.Combine(mainDir, ".svn");
            var git = Path.Combine(mainDir, ".git");
            
            return dirs.Contains(hg) 
                || dirs.Contains(svn) 
                || dirs.Contains(git);
        }



    }
}
