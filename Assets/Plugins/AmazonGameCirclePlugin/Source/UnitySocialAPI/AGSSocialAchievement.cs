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
/// GameCircle Unity Social API achievement implementation.
/// </summary>
public class AGSSocialAchievement : IAchievement {
    // Track a readonly reference to the achievement from the plugin (if available)
    public readonly AGSAchievement achievement; 
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialAchievement"/> class.
    /// </summary>
    /// <param name='achievement'>
    /// Achievement.
    /// </param>
    public AGSSocialAchievement(AGSAchievement achievement) {
        if(null == achievement) {
            AGSClient.LogGameCircleError("AGSSocialAchievement constructor \"achievement\" argument should not be null");
            return;
        }
        this.achievement = achievement;
        id = achievement.id;
        percentCompleted = achievement.progress;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialAchievement"/> class.
    /// </summary>
    public AGSSocialAchievement() {
        achievement = null;   
    }
 
    /// <summary>
    /// Gets or sets the achievement ID.
    /// </summary>
    /// <value>
    /// The achievement ID.
    /// </value>
    public string id {
        get;
        set;
    }
    
    /// <summary>
    /// Gets or sets the percent completed.
    /// </summary>
    /// <value>
    /// The percent completed.
    /// </value>
    public double percentCompleted {
        get;
        set;
    }
    
    /// <summary>
    /// Gets a value indicating whether this <see cref="AGSSocialAchievement"/> is completed.
    /// </summary>
    /// <value>
    /// <c>true</c> if completed; otherwise, <c>false</c>.
    /// </value>
    public bool completed {
        get {
            if(null == achievement) {
                return false;
            }
            return achievement.isUnlocked;
        }
    }
    
    /// <summary>
    /// Gets a value indicating whether this <see cref="AGSSocialAchievement"/> is hidden.
    /// </summary>
    /// <value>
    /// <c>true</c> if hidden; otherwise, <c>false</c>.
    /// </value>
    public bool hidden {
        get {
            if(null == achievement) {
                return false;
            }
            return achievement.isHidden;
        }
    }
    
    /// <summary>
    /// Gets the date an achievement was unlocked
    /// </summary>
    /// <value>
    /// The date an achievement was unlocked
    /// </value>
    public System.DateTime lastReportedDate {
        get {
            if(null == achievement) {
                // Ideally this would return "null" if the achievement
                // was not retrieved, but System.DateTime cannot be null.
                AGSClient.LogGameCircleError("IAchievement.lastReportedDate.get is not available, returning System.DateTime.MinValue.");
                return System.DateTime.MinValue;
            }
            return achievement.dateUnlocked;
        }
    }
    
    /// <summary>
    /// Reports progress made for this achievement.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void ReportProgress(System.Action<bool> callback) {
        // Forward the AGSClient callbacks to the passed in callback.
        if(null != callback) {
            AGSAchievementsClient.UpdateAchievementSucceededEvent += (a) => { callback(true); };
            AGSAchievementsClient.UpdateAchievementFailedEvent += (a,e) => { callback(false); };
        }
        AGSAchievementsClient.UpdateAchievementProgress(id,(float) percentCompleted);  
    }
}
