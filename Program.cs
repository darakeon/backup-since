using System;
using System.IO;

namespace CopySinceDate
{
	class Program
	{
		public static void Main()
		{
			var date = DateTime.MinValue;

			while (date == DateTime.MinValue)
			{
				Console.Write("Insert a valid date: ");
				var dateWrote = Console.ReadLine();

				DateTime.TryParse(dateWrote, out date);
			}


			var originDirectories = Directory.GetDirectories(Config.OriginPath);

			foreach (var originDirectory in originDirectories)
			{
				var destinyDirectory = originDirectory.Replace(Config.OriginPath, Config.DestinyPath);
				
				if (Directory.Exists(destinyDirectory))
					continue;

				Copier.CopyChangesInDir(originDirectory, date);
			}


			Console.WriteLine();
			Console.WriteLine("Copied Files: {0}", Copier.CopiedFiles);
			Console.ReadLine();
		}
	}
}
