using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Runn_R
{
	class Program
	{
		private static Mutex mutex = null;
		private static bool createdNew = false;
		private static string exeName;

		private static List<String> allFiles = new List<String>();

		public static void Main(string[] args)
		{
			try
			{
				// make sure only this instance running
				mutex = new Mutex(true, "Runn-R", out createdNew);
				if (!createdNew) return;

				// get the filename of the executable
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				exeName = Path.GetFileName(codeBase);

				// check argument
				bool debug = false; // default normal mode
				if (args.Length > 0 && args[0].Equals("-D"))
				{
					debug = true; // enable debug mode
				}

				// recursively read current directory
				readDirTree(Directory.GetCurrentDirectory());

				// any file found?
				if (allFiles.Count > 0)
				{
					var rnd = new Random();
					int idx = rnd.Next(allFiles.Count);
					// select a random file
					string rndFile = @allFiles[idx];

					if (debug) // debug mode
					{
						Console.WriteLine(rndFile);
					}
					else // normal mode
					{
						System.Diagnostics.Process.Start(rndFile);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
			}
			finally
			{
				// wait if other instance trying to start
				Thread.Sleep(100);
				// clean up
				if (createdNew)
				{
					mutex.ReleaseMutex();
					mutex.Dispose();
				}
			}
		}

		private static void readDirTree(string path)
		{
			foreach (string dirName in Directory.GetDirectories(path))
			{
				readDirTree(dirName);
			}

			foreach (string fileName in Directory.GetFiles(path))
			{
				// skip self
				if (!Path.GetFileName(fileName).Equals(exeName, StringComparison.OrdinalIgnoreCase))
				{
					allFiles.Add(fileName);
				}
			}
		}
	}
}
