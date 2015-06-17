using UnityEngine;
using System.Collections;

public class GameCircleScript : MonoBehaviour {
	public GUISkin guiSkin;
	public Texture agc;

	bool isServiceReady;
	AGSGameDataMap dataMap;
	AGSSyncableAccumulatingNumber gamesPlayed;

	bool didScore15;
	bool didScore30;
	bool didScore50;
	bool didScore75;
	bool didScore100;



	// Use this for initialization
	void Start () {
		didScore15 = false;
		didScore30 = false;
		didScore50 = false;
		didScore75 = false;
		didScore100 = false;

		guiSkin.button.fontSize = Mathf.Max (Screen.width, Screen.height) / 25;

		AGSClient.ServiceReadyEvent += serviceReadyHandler;
		AGSClient.ServiceNotReadyEvent += serviceNotReadyHandler;
		bool usesLeaderboards = true;
		bool usesAchievements = true;
		bool usesWhispersync = true;
		
		AGSClient.Init (usesLeaderboards, usesAchievements, usesWhispersync);
		isServiceReady = AGSClient.IsServiceReady();


		// Hook up feedback functions
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += submitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent += submitScoreFailed;

		AGSAchievementsClient.UpdateAchievementSucceededEvent += updateAchievementSucceeded;
		AGSAchievementsClient.UpdateAchievementFailedEvent += updateAchievementFailed;

		// Whispersync
		dataMap = AGSWhispersyncClient.GetGameData();
		if(dataMap != null){
			gamesPlayed = dataMap.GetAccumulatingNumber("gamesPlayed");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!didScore15 && GameManager.score == 15){
			didScore15 = true;
			ReportAchievement ("score_15", 100.0F);
		}

		if(!didScore30 && GameManager.score == 30){
			didScore30 = true;
			ReportAchievement("score_30", 100.0F);
		}
		if(!didScore50 && GameManager.score == 50){
			didScore50 = true;
			ReportAchievement ("score_50", 100.0F);
		}
		
		if(!didScore75 && GameManager.score == 75){
			didScore75 = true;
			ReportAchievement ("score_75", 100.0F);
		}
		
		if(!didScore100 && GameManager.score == 100){
			didScore100 = true;
			ReportAchievement("score_100", 100.0F);
		}
	}

	void GameOver(){ // Called by the HollowCubeScript
		ReportScore (GameManager.score);
		// Whispersync
		if(dataMap != null){ 
			gamesPlayed.Increment(1);
			if(gamesPlayed.AsInt() <= 10){
				ReportAchievement ("play_10", gamesPlayed.AsInt() / 10.0F * 100.0F  );
			}
			if(gamesPlayed.AsInt() <= 50){
				ReportAchievement ("play_50", gamesPlayed.AsInt() / 50.0F * 100.0F  );
			}
			if(gamesPlayed.AsInt() <= 100){
				ReportAchievement ("play_100", gamesPlayed.AsInt() / 100.0F  * 100.0F );
			}
			if(gamesPlayed.AsInt() <= 500){
				ReportAchievement ("play_500", gamesPlayed.AsInt() / 500.0F  * 100.0F );
			}
			if(gamesPlayed.AsInt() <= 1000){
				ReportAchievement ("play_1000", gamesPlayed.AsInt() / 1000.0F  * 100.0F );
			}
		}
	}

	private void ReportScore(int score){
		if(isServiceReady){
			//Debug.Log ("Reporting Score");
			if(score > 0) AGSLeaderboardsClient.SubmitScore("score",score);
			if(score == 0) ReportAchievement("score_0", 100.0F);
		}else{
			Debug.Log ("Score - Service is not ready");
		}
	}

	private void ReportAchievement(string id, float percent){
		if(isServiceReady){
//			Debug.Log ("Reporting Achievement");
			AGSAchievementsClient.UpdateAchievementProgress(id,percent);
		}else{
			Debug.Log ("Achievement - Service is not ready");
		}
	}

//	void OnGUI(){
//		GUI.skin = guiSkin;
//		if(!GameManager.gameInProgress && !GameManager.rewind){
//			if(GUI.Button(new Rect(Screen.width * 0.35F, Screen.height * 0.65F, Screen.width * 0.10F, Screen.height* 0.15F), agc)){
//				AGSClient.ShowGameCircleOverlay();
//			}
//		}
//	}

	// Game Circle Functions
	private void serviceNotReadyHandler (string error)    {
//		Debug.Log("Service is not ready");
		isServiceReady = false;
	}
	
	private void serviceReadyHandler ()    {
//		Debug.Log("Service is ready");
		isServiceReady = true;
	}

	private void submitScoreSucceeded(string leaderboardId){
		Debug.Log ("submitScoreSucceeded");
	}
	
	private void submitScoreFailed(string leaderboardId, string error){
		Debug.Log ("submitScoreFailed: " + error);
	}

	private void updateAchievementSucceeded(string achievementId) {
		Debug.Log ("updateAchievementSucceeded");
	}
	
	private void updateAchievementFailed(string achievementId, string error) {
		Debug.Log ("updateAchievementFailed: " + error);
	}
}
