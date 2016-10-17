using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementsUtility {
    public static void CalculatePointsAchievement(int score)
    {
        if (Social.localUser.authenticated)
        {
            if (score >= 1000)
            {
                Social.ReportProgress(MobileLearnResources.achievement_points_adept, 100.0f, (bool success) =>
                {

                });
            }
        }
    }

    public static void CalculateAsteroidsAchievement(int asteroids)
    {
        if (Social.localUser.authenticated)
        {
            if (asteroids >= 5)
            {
                Social.ReportProgress(MobileLearnResources.achievement_asteroid_destroyer, 100.0f, (bool success) =>
                {

                });
            }
        }
    }

    public static void StartGameAchievement()
    {
        Social.ReportProgress(MobileLearnResources.achievement_start_the_game_already, 100.0f, (bool success) =>
        {

        });
    }
}
