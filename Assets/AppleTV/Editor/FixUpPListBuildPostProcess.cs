using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Diagnostics;

public static class FixPlistBuildPostProcess 
{

	[PostProcessBuild()]
	public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
	{

    }

	static public void RunPlistBuddyCommand(string workingDirectory, string arguments)
	{
		ProcessStartInfo proc = new ProcessStartInfo ("/usr/libexec/PlistBuddy", "Info.plist -c \"" + arguments + "\"");
		proc.WorkingDirectory = workingDirectory;
		proc.UseShellExecute = false;
		Process.Start(proc);
    }
		
}
