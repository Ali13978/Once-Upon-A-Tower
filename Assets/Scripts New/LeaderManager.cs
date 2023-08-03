using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using Dan.Models;

public class LeaderManager : MonoBehaviour
{
    #region Singleton
    public static LeaderManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion
    //public Key: f644b7c80da1b608b86dfe1676e7abff3e3ff299cec236e300135f1ba25a0565
    //secret key: f9ee0c75ed60dace5790323506bce568460e07f7623af00e2a07af97f774a66f67d9ea1032862f9bb8de9b4a9d816df2da990fb32141d88764c25f93b6fc203e9d946abd4e75f9c6c66cebc40f1579e287ea634bfb424ff35a1f5b76072b2fae0877adda530a32dcdbd614fc5e0663f158077f583042e21237bc822a4cc2640b

    private int score;

    public void UploadScoreOnLeaderBoard()
    {
        score = SingletonMonoBehaviour<Game>.Instance.Digger.Coins;
        score *= SaveGame.Instance.MissionLevel;
        string playerName = WaxToolkitManager.instance.PlayerAccount;

        LeaderboardCreator.UploadNewEntry("f644b7c80da1b608b86dfe1676e7abff3e3ff299cec236e300135f1ba25a0565", playerName, score);
    }
}
