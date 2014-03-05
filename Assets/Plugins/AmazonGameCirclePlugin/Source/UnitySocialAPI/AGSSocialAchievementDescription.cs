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
/// GameCircle Unity Social API achievement description.
/// </summary>
public class AGSSocialAchievementDescription : IAchievementDescription {
    // track a readonly reference to the achievement associated with this description, if available.
    public readonly AGSAchievement achievement; 
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialAchievementDescription"/> class.
    /// </summary>
    /// <param name='achievement'>
    /// Achievement.
    /// </param>
    public AGSSocialAchievementDescription(AGSAchievement achievement) {
        if(null == achievement) {
            AGSClient.LogGameCircleError("AGSSocialAchievementDescription constructor \"achievement\" argument should not be null");
            return;
        }
        this.achievement = achievement;
        id = achievement.id;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSSocialAchievementDescription"/> class.
    /// </summary>
    public AGSSocialAchievementDescription() {
        achievement = null;   
    }
    
    #region IAchievementDescription implementation
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
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string title {
        get {
            if(null == achievement) {
                return null;
            }
            return achievement.title;
        }
    }
    /// <summary>
    /// Gets the image.
    /// </summary>
    /// <value>
    /// The image.
    /// </value>
    public Texture2D image {
        get {
            AGSClient.LogGameCircleError("IAchievementDescription.image.get is not available for GameCircle");
            return null;
        }
    }
    
    /// <summary>
    /// Gets the achieved description. 
    /// The achievement description achieved / unachieved is handled
    /// by GameCircle internally, so this just returns the available
    /// description for this achievement (the locked description if
    /// the achievement is locked, the unlocked if it is unlocked)
    /// </summary>
    /// <value>
    /// The achieved description.
    /// </value>
    public string achievedDescription {
        get {
            if(null == achievement) {
                return null;
            }
            return achievement.decription;
        }
    }
    
    /// <summary>
    /// Gets the unachieved description.
    /// The achievement description achieved / unachieved is handled
    /// by GameCircle internally, so this just returns the available
    /// description for this achievement (the locked description if
    /// the achievement is locked, the unlocked if it is unlocked)
    /// </summary>
    /// <value>
    /// The unachieved description.
    /// </value>
    public string unachievedDescription {
        get {
            if(null == achievement) {
                return null;
            }    
            return achievement.decription;
        }
    }
    
    /// <summary>
    /// Gets a value indicating whether this <see cref="AGSSocialAchievementDescription"/> is hidden.
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
    /// Gets the point value of this achievement.
    /// </summary>
    /// <value>
    /// The point value of this achievement.
    /// </value>
    public int points {
        get {
            if(null == achievement) {
                return 0;
            }
            return achievement.pointValue;
        }
    }
    #endregion
}
