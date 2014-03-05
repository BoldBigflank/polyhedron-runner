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
using AmazonCommon;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

/// <summary>
///  Player client used to get information on the currently logged in player
/// </summary>
public class AGSPlayerClient : MonoBehaviour{
    

#if UNITY_ANDROID && !UNITY_EDITOR
    private static AmazonJavaWrapper JavaObject;
    private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.PlayerClientProxyImpl"; 
#elif UNITY_IOS
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleRequestLocalPlayer();
#endif
    
    /// <summary>
    /// Event called when alias request succeeds
    /// </summary>
    public static event Action<AGSPlayer> PlayerReceivedEvent;
    
    /// <summary>
    /// Event called when alias request fails
    /// </summary>
    /// <param name="failureReason">a string indicating the failure reason</param>
    public static event Action<string> PlayerFailedEvent;
     
    static AGSPlayerClient(){
#if UNITY_ANDROID && !UNITY_EDITOR
        // find the plugin instance
        JavaObject = new AmazonJavaWrapper(); 
        using( var PluginClass = new AndroidJavaClass( PROXY_CLASS_NAME ) ){
            if (PluginClass.GetRawClass() == IntPtr.Zero)
            {
                AGSClient.LogGameCircleWarning("No java class " + PROXY_CLASS_NAME + " present, can't use AGSPlayerClient" );
                return;
            }
            JavaObject.setAndroidJavaObject(PluginClass.CallStatic<AndroidJavaObject>( "getInstance" ));
        }
#endif
    }
    
    /// <summary>
    /// Request the local player player information
    /// </summary>
    public static void RequestLocalPlayer(){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
        JavaObject.Call( "requestLocalPlayer" );
#elif UNITY_IOS
        _AmazonGameCircleRequestLocalPlayer();
#else
        if( PlayerFailedEvent != null ){
            PlayerFailedEvent( "PLATFORM_NOT_SUPPORTED" );
        }
#endif
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void PlayerReceived( string json ){
        if( PlayerReceivedEvent != null ){
            var ht = json.hashtableFromJson();
            PlayerReceivedEvent( AGSPlayer.fromHashtable(ht) );
        }
    }
    
    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void PlayerFailed( string json ){
        if( PlayerFailedEvent != null ){
            var ht = json.hashtableFromJson();
            string error = GetStringFromHashtable(ht,"error");
            PlayerFailedEvent( error );
        }
    }
    
    /// <summary>
    /// Gets the string from hashtable.
    /// </summary>
    /// <returns>
    /// The string from hashtable.
    /// </returns>
    /// <param name='ht'>
    /// Ht.
    /// </param>
    /// <param name='key'>
    /// Key.
    /// </param>
    private static string GetStringFromHashtable(Hashtable ht, string key){
        string val = null;
        if(ht.Contains(key)){
            val = ht[key].ToString();
        }
        return val;
    }    
}