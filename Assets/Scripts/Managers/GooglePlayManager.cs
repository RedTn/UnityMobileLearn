using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour {
    void Awake()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;

        Social.localUser.Authenticate((bool success) =>
       {
           Debug.Log(success);
       });
    }
}
