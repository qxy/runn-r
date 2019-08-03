using System;
using System.Collections.Generic;
using System.IO;

namespace Runn_R
{
	class Program
	{
		static List<String> allFiles = new List<String>();

		public static void Main(string[] args)
		{
			try
			{		
				readDirTree(Directory.GetCurrentDirectory());

				if (allFiles.Count > 0)
				{
					var rnd = new Random();
					int idx = rnd.Next(allFiles.Count);

					System.Diagnostics.Process.Start(@allFiles[idx]);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
			}
		}

		static void readDirTree(string path)
		{
			foreach (string dirName in Directory.GetDirectories(path))
			{
				// if (crtlC) return;

				readDirTree(dirName);
			}

			foreach (string fileName in Directory.GetFiles(path))
			{
				// if (crtlC) return;

				if (!Path.GetFileName(fileName).Equals("Runn-R.exe", StringComparison.OrdinalIgnoreCase))
				{
					allFiles.Add(fileName);
				}
			}
		}
	}
}