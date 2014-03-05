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
/// GameCircle local user implemention for Unity Social API.
/// </summary>
public class AGSSocialLocalUser : ILocalUser {
    // A reference to the GameCircle player is kept, if available.
    AGSPlayer player = null;
    
    #region ILocalUser implementation
    /// <summary>
    /// Authenticate the local user with the active Social plugin.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void Authenticate(System.Action<bool> callback) {
        // The Unity Social API implies a heavy connection between
        // initialization of the Social API and the local user.
        // http://docs.unity3d.com/Documentation/Components/net-SocialAPI.html
        // This means that the local player should be available as early as possible.
        callback += (successStatus) => {
            if(successStatus) {  
                // On a successful initialization of the GameCircle through
                // the Unity Social API, immediately begin the process
                // of retrieving the local player.
                AGSPlayerClient.PlayerReceivedEvent += (player) => {
                    this.player = player;
                };
                AGSPlayerClient.RequestLocalPlayer();
            }
            else {
                player = null;   
            }
        };
        Social.Active.Authenticate(this,callback);
    }
    
    /// <summary>
    /// Loads this local user's friends list.
    /// </summary>
    /// <param name='callback'>
    /// Callback.
    /// </param>
    public void LoadFriends(System.Action<bool> callback) {
        AGSClient.LogGameCircleError("ILocalUser.LoadFriends is not available for GameCircle");
        // Call the callback with a "false" to let it know the friends list was not loaded.
        if(null != callback) {
            callback(false);
        }
    }
 
    /// <summary>
    /// Gets the local user's friends.
    /// </summary>
    /// <value>
    /// The friends.
    /// </value>
    public IUserProfile[] friends {
        get {
            AGSClient.LogGameCircleError("ILocalUser.friends.get is not available for GameCircle");
            return null;
        }
    }
 
    /// <summary>
    /// Gets a value indicating whether this <see cref="GameCircleLocalUser"/> is authenticated.
    /// </summary>
    /// <value>
    /// <c>true</c> if authenticated; otherwise, <c>false</c>.
    /// </value>
    public bool authenticated {
        get {
            return null != player;
        }
    }
 
    /// <summary>
    /// Gets a value indicating whether this <see cref="GameCircleLocalUser"/> is underage.
    /// </summary>
    /// <value>
    /// <c>true</c> if underage; otherwise, <c>false</c>.
    /// </value>
    public bool underage {
        get {
            AGSClient.LogGameCircleError("ILocalUser.underage.get is not available for GameCircle");
            return false;
        }
    }
    #endregion

    #region IUserPlayer implementation
    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    /// <value>
    /// The name of the user.
    /// </value>
    public string userName {
        get {
            if(null != player) {
                return player.alias;
            }
            return null;
        }
    }
 
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string id {
        get {
            if(null != player) {
                return player.playerId;
            }
            return null;
        }
    }
 
    /// <summary>
    /// Gets a value indicating whether this <see cref="GameCircleLocalUser"/> is friend.
    /// </summary>
    /// <value>
    /// <c>true</c> if is friend; otherwise, <c>false</c>.
    /// </value>
    public bool isFriend {
        get {
            AGSClient.LogGameCircleError("ILocalUser.isFriend.get is not available for GameCircle");
            return false;
        }
    }
 
    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>
    /// The state.
    /// </value>
    public UserState state {
        get {
            AGSClient.LogGameCircleError("ILocalUser.state.get is not available for GameCircle");
            return UserState.Offline;
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
            AGSClient.LogGameCircleError("ILocalUser.image.get is not available for GameCircle");
            return null;
        }
    }
    #endregion

}