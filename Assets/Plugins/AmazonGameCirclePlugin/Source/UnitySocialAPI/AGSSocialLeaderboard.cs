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
/// GameCircle leaderboard implemention for Unity Social API.
/// </summary>
public class AGSSocialLeaderboard : ILeaderboard {
    // keep a reference to the GameCircle leaderboard, if available.
    readonly AGSLeaderboard leaderboard;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialLeaderboard"/> class.
    /// </summary>
    /// <param name='leaderboard'>
    /// Leaderboard.
    /// </param>
    public AGSSocialLeaderboard(AGSLeaderboard leaderboard) {
        if(null == leaderboard) {
            AGSClient.LogGameCircleError("AGSSocialLeaderboard constructor \"leaderboard\" argument should not be null");
            return;
        }
        this.leaderboard = leaderboard;
        id = leaderboard.id;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialLeaderboard"/> class.
    /// </summary>
    public AGSSocialLeaderboard() {
        this.leaderboard = null;
    }
    
    /// <summary>
    /// Checks if scores are available for this leaderboard
    /// </summary>
    /// <returns>
    /// if scores are available
    /// </returns>
    public bool ScoresAvailable() {
        return null != leaderboard && null != leaderboard.scores && leaderboard.scores.Count > 0;
    }
    
    #region ILeaderboard Implementation
    /// <summary>
    /// Gets a value indicating whether this <see cref="AGSSocialLeaderboard"/> is loading.
    /// </summary>
    /// <value>
    /// <c>true</c> if loading; otherwise, <c>false</c>.
    /// </value>
    public bool loading {
        get {   
            AGSClient.LogGameCircleError("ILeaderboard.loading.get is not available for GameCircle");
            return false;
        }
    }
    
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string id {
        get;
        set;
    }
    
    /// <summary>
    /// Gets or sets the user scope.
    /// </summary>
    /// <value>
    /// The user scope.
    /// </value>
    public UserScope userScope {
        get;
        set;
    }
    
    /// <summary>
    /// Gets or sets the range.
    /// </summary>
    /// <value>
    /// The range.
    /// </value>
    public Range range {
        get;
        set;
    }
    
    /// <summary>
    /// Gets or sets the time scope.
    /// </summary>
    /// <value>
    /// The time scope.
    /// </value>
    public TimeScope timeScope {
        get;
        set;
    }
    
    /// <summary>
    /// Gets the local user score.
    /// </summary>
    /// <value>
    /// The local user score.
    /// </value>
    public IScore localUserScore {
        get {
            if(!ScoresAvailable()) {
                return null;
            }
            // return the first score in the list
            return new AGSSocialLeaderboardScore(leaderboard.scores[0],leaderboard);
        }
    }
    
    /// <summary>
    /// Gets the max range.
    /// </summary>
    /// <value>
    /// The max range.
    /// </value>
    /// <exception cref='System.NotImplementedException'>
    /// Is thrown when a requested operation is not implemented for a given type.
    /// </exception>
    public uint maxRange {
        get {   
            AGSClient.LogGameCircleError("ILeaderboard.maxRange.get is not available for GameCircle");
            return 0;
        }
    }
    
    /// <summary>
    /// Gets the scores.
    /// </summary>
    /// <value>
    /// The scores.
    /// </value>
    public IScore[] scores {
        get {
            if(!ScoresAvailable()) {
                return null;
            }
            AGSSocialLeaderboardScore [] leaderboardScores = new AGSSocialLeaderboardScore[leaderboard.scores.Count];
            for(int scoreIndex = 0; scoreIndex < leaderboard.scores.Count; scoreIndex++) {
                leaderboardScores[scoreIndex] = new AGSSocialLeaderboardScore(leaderboard.scores[scoreIndex],leaderboard);
            }
            return leaderboardScores;
        }
    }
    
    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string title {
        get {
            if(null == leaderboard) {
                return null;
            }
            return leaderboard.name;
        }
    }
    
    /// <summary>
    /// Sets the user filter.
    /// </summary>
    /// <param name='userIDs'>
    /// User I ds.
    /// </param>
    public void SetUserFilter(string[] userIDs) {   
        AGSClient.LogGameCircleError("ILeaderboard.SetUserFilter is not available for GameCircle");
    }
    
    /// <summary>
    /// Loads the scores.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadScores(System.Action<bool> callback) {   
        if(null == callback) {
            return;
        }
        // if the GameCircle leaderboard is not set, then
        // the scores were not loaded.
        if(null == leaderboard) {
            callback(false);
        }
        // GameCircle automatically populates the leaderboard's scores
        // when the leaderboard is retrieved.
        callback(true);
    }
    #endregion
}
 