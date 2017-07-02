using System;
using System.IO;

namespace CopySinceDate
{
	public class Copier
	{
		internal static Int32 CopiedFiles { get; private set; }

		internal static void CopyChangesInDir(String mainDir, DateTime init)
		{
			var newDir = mainDir.Replace(Config.OriginPath, Config.BackupFolder);


			var dirs = Directory.GetDirectories(mainDir);

			if (ignore(mainDir))
				return;

			foreach (var dir in dirs)
			{
				CopyChangesInDir(dir, init);
			}


			var files = Directory.GetFiles(mainDir);

			foreach (var file in files)
			{
				if (ignore(file))
					continue;

				var info = new FileInfo(file);

				if (info.LastWriteTime < init)
					continue;

				if (!Directory.Exists(newDir))
					Directory.CreateDirectory(newDir);

				var newFile = file.Replace(mainDir, newDir);

				File.Copy(file, newFile);

				CopiedFiles++;
			}


		}

		private static Boolean ignore(String path)
		{
			return path.Contains("Resharper")
			       || path.Contains("dotCover")
			       || path.EndsWith("bin")
			       || path.EndsWith("obj");
		}
	}
}