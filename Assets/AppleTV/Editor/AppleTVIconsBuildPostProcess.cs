using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Reflection;
using System.Linq;

public class AppleTVIconsBuildPostProcess 
{
	[MenuItem("AppleTV/Re-export Icons")]
	static void TestAppleIconsExport()
	{
		string cachedPathToBuiltProject = EditorPrefs.GetString("PathToBuiltProject");
		if(!string.IsNullOrEmpty(cachedPathToBuiltProject))
		{
			OnPostprocessBuild(BuildTarget.iOS, cachedPathToBuiltProject);
		}
		else
		{
			Debug.LogWarning("You need to run a build before you can use this operation");
		}
	}

	[PostProcessBuild()]
	public static void OnPostprocessBuild (BuildTarget target, string pathToBuiltProject)
	{
		// Cache off built project path so we can use it if we want to re-export just the icons
		EditorPrefs.SetString("PathToBuiltProject", pathToBuiltProject);

		// Some hard coded paths
		const string assetCatalogPath = "/Unity-iPhone/Images.xcassets";
		const string brandAssetsPath = assetCatalogPath + "/AppIcon.brandassets";
		const string smallImageStack = "/App Icon - Small.imagestack";
		const string largeImageStack = "/App Icon - Large.imagestack";
		const string topShelfImageStack = "/Top Shelf Image.imageset";
		const string gamecenterDashboardPath = "/Dashboard Image.gcdashboardimage/Logo.imageset";

		// Load up icons assets
		AppleTVIcons icons = AssetDatabase.LoadAssetAtPath<AppleTVIcons>("Assets/AppleTV/AppleTVIcons.asset");

		// Create small and large image stacks
		CreateImageStack(icons._SmallIcons, pathToBuiltProject + brandAssetsPath + smallImageStack);
		CreateImageStack(icons._LargeIcons, pathToBuiltProject + brandAssetsPath + largeImageStack);

		// Top shelf image
		Directory.CreateDirectory (pathToBuiltProject + brandAssetsPath + topShelfImageStack);
		CopyImage (icons._TopShelfIcon, pathToBuiltProject + brandAssetsPath + topShelfImageStack);

		if (icons._GameCenterDashboard) 
		{
			// Dashboard
			Directory.CreateDirectory (pathToBuiltProject + assetCatalogPath + gamecenterDashboardPath);
			CopyImage (icons._GameCenterDashboard, pathToBuiltProject + assetCatalogPath + gamecenterDashboardPath);
		}

		// Copy pre made json
		string pathToJson = Path.GetDirectoryName (AssetDatabase.GetAssetPath (icons));
		File.Copy (pathToJson + "/BrandAssetsContents.json", pathToBuiltProject + brandAssetsPath + "/Contents.json", true);

		// Create leaderboard icons
		for(int i = 0; i < icons._LeaderboardIcons.Length; i++)
		{
			CreateLeaderboardIcons(pathToBuiltProject + assetCatalogPath, icons._LeaderboardIcons[i]);
		}

		// Remove old style app icon
		if (Directory.Exists (pathToBuiltProject + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset"))
		{
			Directory.Delete (pathToBuiltProject + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset", true);
		}
	}

	static void CreateLeaderboardIcons(string path, AppleTVIcons.LeaderboardIcon leaderboardIcon)
	{
		string leaderboardPath = path + "/" + leaderboardIcon._Name + ".gcleaderboard";
		string posterPath = leaderboardPath + "/Poster.imagestack";
		Directory.CreateDirectory (posterPath);
		Dictionary<string, Dictionary<string, object>> dict = new Dictionary<string, Dictionary<string, object>>();

		dict["properties"] = new Dictionary<string, object>();
		dict["properties"]["identifier"] = leaderboardIcon._LeaderboardId;

		string jsonOutput = MiniJSON.Json.Serialize(dict);
		File.WriteAllText (leaderboardPath + "/Contents.json", jsonOutput);

		CreateImageStack(leaderboardIcon._ImageStack, posterPath); 
	}

	static void CreateImageStack(Texture2D[] textures, string path)
	{

		Directory.CreateDirectory (path);
		WriteOutImageStackJson(path + "/Contents.json", textures.Length);

		for(int i = 0; i < textures.Length; i++)
		{
			string imageSetPath = path + "/" + i + ".imagestacklayer/Contents.imageset";
			Directory.CreateDirectory (imageSetPath);
			CopyImage(textures[i], imageSetPath);
		}
	}

	static void WriteOutImageStackJson(string path, int numberOfImages)
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();

		Dictionary<string, object>[] layers = new Dictionary<string, object>[numberOfImages];
		dict["layers"] = layers;
		for(int i = 0; i < numberOfImages; i++)
		{
			
			layers[i] = new Dictionary<string, object>();
			layers[i]["filename"] = i + ".imagestacklayer";
		}

		string jsonOutput = MiniJSON.Json.Serialize(dict);
		File.WriteAllText (path, jsonOutput);
	}

	static void WriteOutImageSetJson(string path, string filename)
	{
		Dictionary<string, object> dict = new Dictionary<string, object>();

		Dictionary<string, object>[] images = new Dictionary<string, object>[1];
		dict["images"] = images;
		images[0] = new Dictionary<string, object>();
		images[0]["filename"] = filename;
		images[0]["idiom"] = "tv";
		images[0]["scale"] = "1x";

		string jsonOutput = MiniJSON.Json.Serialize(dict);
		File.WriteAllText (path, jsonOutput);
	}

	static void CopyImage(Texture2D image, string targetPath)
	{
		if (image)
		{
			string pathToIcon = AssetDatabase.GetAssetPath (image);
			string iconFileName = Path.GetFileName (pathToIcon);
			File.Copy (pathToIcon, targetPath + "/" + iconFileName, true);
			WriteOutImageSetJson(targetPath + "/Contents.json", iconFileName);
		}
	}	

}
