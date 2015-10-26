using UnityEngine;
using System.Collections;

public class AppleTVIcons : ScriptableObject
{
	public Texture2D[] _SmallIcons;	
	public Texture2D[] _LargeIcons;

	public Texture2D _TopShelfIcon;

	public Texture2D _GameCenterDashboard;
	public LeaderboardIcon[] _LeaderboardIcons;

	[System.Serializable]
	public class LeaderboardIcon
	{
		public string _Name;
		public string _LeaderboardId;
		public Texture2D[] _ImageStack;
	}
}
