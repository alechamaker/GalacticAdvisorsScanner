using Scanner;
using System.Runtime.InteropServices;

//TO-DO add support for other operating systems
if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
{
	Console.WriteLine($"The platform '{Environment.OSVersion}' is not currently supported..");
	Environment.Exit(0);
}

var DrivesInfo = DriveInfo.GetDrives();
var scanners = new DriveScanner[DrivesInfo.Length];
string[] plainTextExt = new string[] { "*.txt", "*.log", "*.rtf" };

//Create drive scanners for each of the detected drives
for (int i = 0; i < DrivesInfo.Length; i++)
{
	Console.WriteLine($"Scanning drive '{DrivesInfo[i]}'");
	foreach (var ext in plainTextExt)
	{ 
		scanners[i] = new DriveScanner(DrivesInfo[i], ext);
		scanners[i].StartScan();
	}
}

//wait for all the scanners to finish running...
foreach (var scanner in scanners)
{
	scanner.WaitScan();
}
