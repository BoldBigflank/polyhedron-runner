#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;
using System.Diagnostics;

public class CustomPostprocessScript : MonoBehaviour
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
	{
		Process process = new Process();
		process.StartInfo.FileName = "python";
		process.StartInfo.Arguments = string.Format("-B Assets/Editor/DicePlus/post_process.py \"{0}\"", pathToBuildProject);
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		process.Start();
		while (!process.StandardOutput.EndOfStream) {
			string sout = process.StandardOutput.ReadLine();
			UnityEngine.Debug.Log(sout);
		}

		while (!process.StandardError.EndOfStream) {
			string serr = process.StandardError.ReadLine();
			UnityEngine.Debug.LogError(serr);
		}

		process.WaitForExit();
	}
}
#endif
