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

        static String backupFolder = 
            Path.Combine(mainPath, DateTime.Now.ToString("yyyyMMddHHmm"));

        static Int32 copiedFiles;



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



    }
}
