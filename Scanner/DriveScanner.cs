using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scanner
{
	public class DriveScanner
	{
		private EnumerationOptions _defaultEnumOpts = new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = true };
		private DriveInfo _driveInfo;
		private Task _scan;
		private EnumerationOptions _enumerationOptions;
		private string _ssnPattern = @"(?!(000|666|9))\d{3}-(?!00)\d{2}-(?!0000)\d{4}";
		private string _ccPattern = @"/(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})/";
		private Regex[] _patterns;
		private List<Task<Dictionary<string, Match>>> _fileScanners = new List<Task<Dictionary<string, Match>>>();


		/// <summary>
		/// Constructs a drive scanner
		/// </summary>
		/// <param name="drive">DriveInfo for the drive to be scanned</param>
		/// <param name="searchPattern">Simple search pattern for file extensions to search for</param>
		/// <param name="enumOpts">Optional enumeration options</param>
		public DriveScanner(DriveInfo drive, string searchPattern, EnumerationOptions? enumOpts = null)
		{
			_driveInfo = drive;
			_scan = new Task(() => Scan(searchPattern));
			_enumerationOptions = (enumOpts == null) ? _defaultEnumOpts : enumOpts;
			_patterns = new Regex[] { new Regex(_ssnPattern), new Regex(_ccPattern)};
		}

		/// <summary>
		/// Starts the drive scanner
		/// </summary>
		public void StartScan()
		{
			_scan.Start();
		}

		/// <summary>
		/// waits for the drive scanner to finish running
		/// </summary>
		public void WaitScan()
		{
			_scan.Wait();
		}

		/// <summary>
		/// logic to scan for files across the drive
		/// </summary>
		/// <param name="searchPattern">Simple file extension pattern to search for</param>
		private void Scan(string searchPattern) 
		{
			Dictionary<string, Match> _results = new Dictionary<string, Match>();
			try
			{
				//iterate all the files in the drive...
				foreach (string file in Directory.EnumerateFiles(_driveInfo.Name, searchPattern, _enumerationOptions))
				{
					//wait until there is more space to scan...
					while (_fileScanners.Count >= 25)
					{
						//dummy scanner to hold a reference to a finished scanner
						Task<Dictionary<string,Match>> finishedScanner = new Task<Dictionary<string, Match>>(() => { return new Dictionary<string, Match>(); }) ;
						//wait for one of the scanners to finish before adding another to the running list
						//TO-DO.. Allow the drive scanner to continue searching for files while the file scanners finish running
						foreach(var scanner in _fileScanners)
						{
							scanner.Wait();
							foreach (var scannerResult in scanner.Result)
							{
								if(!_results.ContainsKey(scannerResult.Key))
									_results.Add(scannerResult.Key, scannerResult.Value);
							}
							finishedScanner = scanner;
							break;
						}
						_fileScanners.Remove(finishedScanner);
					}
					_fileScanners.Add(new FileScanner(file, _patterns).StartScan());
				}

				//wait for the final file scanners to finish running
				foreach (var scanner in _fileScanners)
				{
					scanner.Wait();
				}

				//log the results
				//TO-DO Create a summarization method to generate a prettier report
				foreach (var result in _results)
				{
					Console.WriteLine($"{result.Key}\t{result.Value}");
				}

			}
			//TO-DO write logic to retry any failed scans
			catch(Exception e) 
			{
				Console.WriteLine(e);
			}
		}

	}
}
