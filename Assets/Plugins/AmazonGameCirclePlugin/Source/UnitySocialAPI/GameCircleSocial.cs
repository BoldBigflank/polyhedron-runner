/**
 * © 2012-2013 Amazon Digital Services, Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except in compliance with the License. A copy
 * of the License is located at
 *
 * http://aws.amazon.com/apache2.0/
 *
 * or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */
using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// GameCircle Unity Social API implementation
/// </summary>
public class GameCircleSocial : ISocialPlatform {
    
    #region local variables
    AGSSocialLocalUser gameCircleLocalUser = new AGSSocialLocalUser();
    #endregion
    
    #region static variables
    // keep a static instance of the GameCircleSocial plugin around.
    static GameCircleSocial socialInstance = new GameCircleSocial();
    #endregion
    
    #region static interface
    /// <summary>
    /// Gets the GameCircleSocial instance.
    /// </summary>
    /// <value>
    /// The GameCircleSocial instance.
    /// </value>
    public static GameCircleSocial Instance {
        get { return socialInstance; }
    }
    #endregion
    
    #region ISocialPlatform Overrides
    /// <summary>
    /// Gets the local user.
    /// </summary>
    /// <value>
    /// The local user.
    /// </value>
    public ILocalUser localUser {
        get { return gameCircleLocalUser; }
    }
    
    /// <summary>
    /// Loads the users.
    /// </summary>
    /// <param name='userIDs'>
    /// User IDs.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadUsers (string[] userIDs, System.Action<IUserProfile[]> callback) {
        AGSClient.LogGameCircleError("ISocialPlatform.LoadUsers is not available for GameCircle");
    }
    
    /// <summary>
    /// Reports the achievement progress.
    /// </summary>
    /// <param name='achievementID'>
    /// Achievement ID.
    /// </param>
    /// <param name='progress'>
    /// Progress.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void ReportProgress(string achievementID, double progress, System.Action<bool> callback) {
        // Forward the AGSClient callbacks to the passed in callback.
        if(null != callback) {
            AGSAchievementsClient.UpdateAchievementSucceededEvent += (a) => { callback(true); };
            AGSAchievementsClient.UpdateAchievementFailedEvent += (a,e) => { callback(false); };
        }
        AGSAchievementsClient.UpdateAchievementProgress(achievementID,(float) progress);  
    }
    
    /// <summary>
    /// Loads the achievement descriptions.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadAchievementDescriptions(System.Action<IAchievementDescription[]> callback) {
        // The callback argument should not be null for this function.
        if(null == callback) {
            AGSClient.LogGameCircleError("LoadAchievementDescriptions \"callback\" argument should not be null");
            return;
        }
        
        // Transform the passed in callback action to the type of callback the GameCircle plugin expects.
        AGSAchievementsClient.RequestAchievementsFailedEvent += (e) => { callback(null); };
        AGSAchievementsClient.RequestAchievementsSucceededEvent += (achievements) => {
            AGSSocialAchievementDescription [] descriptions = new AGSSocialAchievementDescription[achievements.Count];
            for(int achievementIndex = 0; achievementIndex < achievements.Count; achievementIndex++) {
                descriptions[achievementIndex] = new AGSSocialAchievementDescription(achievements[achievementIndex]);                
            }
            callback(descriptions);
        };
        AGSAchievementsClient.RequestAchievements();
        
    }
    
    /// <summary>
    /// Loads the achievements.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadAchievements(System.Action<IAchievement[]> callback) {
        // The callback argument should not be null for this function.
        if(null == callback) {
            AGSClient.LogGameCircleError("LoadAchievements \"callback\" argument should not be null");
            return;
        }
        
        // Transform the passed in callback action to the type of callback the GameCircle plugin expects.
        AGSAchievementsClient.RequestAchievementsFailedEvent += (e) => { callback(null); };
        AGSAchievementsClient.RequestAchievementsSucceededEvent += (achievements) => {
            AGSSocialAchievement [] socialAchievements = new AGSSocialAchievement[achievements.Count];
            for(int achievementIndex = 0; achievementIndex < achievements.Count; achievementIndex++) {
                socialAchievements[achievementIndex] = new AGSSocialAchievement(achievements[achievementIndex]);                
            }
            callback(socialAchievements);
        };
        AGSAchievementsClient.RequestAchievements();
    }
    
    /// <summary>
    /// Creates the achievement.
    /// </summary>
    /// <returns>
    /// The achievement.
    /// </returns>
    public IAchievement CreateAchievement() {
        return new AGSSocialAchievement();
    }
    
    /// <summary>
    /// Reports the score.
    /// </summary>
    /// <param name='score'>
    /// Score.
    /// </param>
    /// <param name='board'>
    /// Board.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void ReportScore(long score, string board, System.Action<bool> callback) {
        // Forward the AGSClient callbacks to the passed in callback.
        if(null != callback) {
            AGSLeaderboardsClient.SubmitScoreSucceededEvent += (a) => { callback(true); };
            AGSLeaderboardsClient.SubmitScoreFailedEvent += (a,e) => { callback(false); };
        }
        AGSLeaderboardsClient.SubmitScore(board,score);
    }
    
    /// <summary>
    /// Loads the scores.
    /// </summary>
    /// <param name='leaderboardID'>
    /// Leaderboard I.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadScores(string leaderboardID, System.Action<IScore[]> callback) {
        // Forward the AGSClient callbacks to the passed in callback.
        if(null != callback) {
            AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent += (leaderboards) => { 
                // If the leaderboard could not be found, call the callback with a null list.
                IScore [] scores = null;
                
                // Look through the retrieved leaderboards for the passed in leaderboard ID
                foreach(AGSLeaderboard leaderboard in leaderboards) {
                    if(leaderboard.id == leaderboardID) {
                        AGSSocialLeaderboard socialLeaderboard = new AGSSocialLeaderboard(leaderboard);
                        scores = socialLeaderboard.scores;
                        break;
                    }
                }
                callback(scores);
            };
            // If retrieving leaderboards failed, call the callback with a null list.
            AGSLeaderboardsClient.RequestLeaderboardsFailedEvent += (error) => { callback(null); };
        }
        // Request the leaderboard list so the requested leaderboard ID can be searched for.
        AGSLeaderboardsClient.RequestLeaderboards();
    }
    
    /// <summary>
    /// Creates the leaderboard.
    /// </summary>
    /// <returns>
    /// The leaderboard.
    /// </returns>
    public ILeaderboard CreateLeaderboard() {
        return new AGSSocialLeaderboard();
    }
    
    /// <summary>
    /// Shows the achievements UI.
    /// </summary>
    public void ShowAchievementsUI() {
        AGSAchievementsClient.ShowAchievementsOverlay();
    }
    
    /// <summary>
    /// Shows the leaderboard UI.
    /// </summary>
    public void ShowLeaderboardUI() {
        AGSLeaderboardsClient.ShowLeaderboardsOverlay();
    }
    
    /// <summary>
    /// Authenticate the specified user and callback.
    /// </summary>
    /// <param name='user'>
    /// User.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void Authenticate(ILocalUser user, System.Action<bool> callback) {
        // Forward the AGSClient callbacks to the passed in callback.
        if(null != callback) {
            AGSClient.ServiceReadyEvent += () => { 
                callback(true); 
            };
            AGSClient.ServiceNotReadyEvent += (error) => { callback(false); };
        }
        // If using GameCircle with the Unity Social API, 
        // initialize it with leaderboards and achievements, but not whispersync.
        AGSClient.Init(/*Leaderboards*/true,/*Achievements*/true,/*Whispersync*/false);
    } 
    
    /// <summary>
    /// Loads the friends.
    /// </summary>
    /// <param name='user'>
    /// User.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadFriends(ILocalUser user, System.Action<bool> callback) {
        if(user == null) {
            AGSClient.LogGameCircleError("LoadFriends \"user\" argument should not be null");
            return;
        }
        user.LoadFriends(callback);
    }
    
    /// <summary>
    /// Loads the scores.
    /// </summary>
    /// <param name='board'>
    /// Board.
    /// </param>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadScores(ILeaderboard board, System.Action<bool> callback) {
        // This function doesn't do anything with a null leaderboard.
        if(null == board) {
            AGSClient.LogGameCircleError("LoadScores \"board\" argument should not be null");
            return;
        }
        board.LoadScores(callback);
    }
    
    /// <summary>
    /// Gets the loading status of the leaderboard.
    /// </summary>
    /// <returns>
    /// The loading.
    /// </returns>
    /// <param name='board'>
    /// If set to <c>true</c> board.
    /// </param>
    public bool GetLoading(ILeaderboard board) {
        // This function doesn't do anything with a null leaderboard.
        if(null == board) {
            AGSClient.LogGameCircleError("GetLoading \"board\" argument should not be null");
            return false;
        }
        return board.loading;
    }
    #endregion
}
