using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour {
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) =>
       {
           if(success)
           {
               AchievementsUtility.StartGameAchievement();
           }
       });
    }
}
