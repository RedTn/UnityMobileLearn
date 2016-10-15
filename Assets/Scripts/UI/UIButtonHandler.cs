using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class UIButtonHandler : MonoBehaviour {
    public void OpenAchievementPane()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            Debug.Log("not authenticated");
        }
    }
}
