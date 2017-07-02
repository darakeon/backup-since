using System;
using System.Configuration;
using System.IO;

namespace CopySinceDate
{
	public class Config
	{
		internal static String OriginPath = ConfigurationManager.AppSettings["OriginPath"];
		internal static String DestinyPath = ConfigurationManager.AppSettings["DestinyPath"];

		internal static String BackupFolder = Path.Combine(OriginPath, DateTime.Now.ToString("yyyyMMddHHmm"));
	}
}