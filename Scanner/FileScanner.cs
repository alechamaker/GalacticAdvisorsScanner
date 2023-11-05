using System.Text.RegularExpressions;

namespace Scanner
{

	public class FileScanner
	{
		private Task<Dictionary<string, Match>> _scan;
		private string _path;
		private Regex[] _patterns;

		/// <summary>
		/// Constructs a file scanner
		/// </summary>
		/// <param name="path">Path to the file to scan</param>
		/// <param name="patterns">Regex patterns to match in the file</param>
		public FileScanner(string path, Regex[] patterns)
		{
			_path = path;	
			_patterns = patterns;
			_scan = new Task<Dictionary<string, Match>>(() => Scan());
		}

		/// <summary>
		/// Start the file scanner
		/// </summary>
		/// <returns>A running file scanner task</returns>
		public Task<Dictionary<string, Match>> StartScan()
		{
			_scan.Start();
			return _scan;
		}

		/// <summary>
		/// Logic to scan the file
		/// </summary>
		/// <returns>Matches found in the file</returns>
		private Dictionary<string, Match> Scan() 
		{
			var resultMatches = new Dictionary<string, Match>();
			try
			{
				//WaitForFile(_path);
				string contents = File.ReadAllText(_path);
				foreach (Regex pattern in _patterns) 
				{
					MatchCollection matches = pattern.Matches(contents);
					foreach (Match match in matches)
					{
						//Console.WriteLine(match.Value);
						resultMatches.Add($"{_path}:{match.Index}", match);
					}
				}
				return resultMatches;

			}
			//the file is already being used by another process..
			//for now we will skip it..
			catch(System.IO.IOException)
			{ return resultMatches; }
			//we don't have access to the file.. skip it
			catch(System.UnauthorizedAccessException)
			{ return resultMatches; }
			catch(Exception e) 
			{
				Console.WriteLine(e.GetType());
				Console.WriteLine(e.Message);
				return resultMatches;
			}

		}

	/*	TO-DO re-implement these functions as optional as they will significantly increase runtime
	 *	public static bool IsFileReady(string filename)
		{
			// If the file can be opened for exclusive access it means that the file
			// is no longer locked by another process.
			try
			{
				using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
					return inputStream.Length > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static async void WaitForFile(string filename)
		{
			//This will lock the execution until the file is ready
			//TODO: Add some logic to make it async and cancelable
			var wait = new Task(() =>
			{
				var startTime = DateTime.Now;
				while (!IsFileReady(filename)) 
				{
					if (DateTime.Now - startTime >= new TimeSpan(0, 0, 3))
						break;
				}
			});
			wait.Start();
			await wait;
		}*/
	}
}
