using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AcheivementsManager : MonoBehaviour
{
    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void DoGrantAchievement(string _achievement)
    {
        Social.ReportProgress(_achievement, 100.00f, (bool success) =>
        {
            if (success)
            {

            }
        });
    }

    public void DoIncrementalAchievement(string _achievement)
    {
        PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;
        platform.IncrementAchievement(_achievement, 1, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });
    }
}
