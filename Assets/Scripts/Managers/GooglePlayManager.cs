using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GooglePlayManager : MonoBehaviour {
    void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;

        Social.localUser.Authenticate((bool success) =>
       {
           if(success)
           {
               AchievementsUtility.StartGameAchievement();
           }
       });
    }
}
