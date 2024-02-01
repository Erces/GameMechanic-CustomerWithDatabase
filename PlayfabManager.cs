using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager i;

    public List<SaveableObject> so;

    void Awake()
    {
        i = this;
        Login();
    }
    #region Login
    void Login()
    {
        var req = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(req, OnSuccess, OnError);
    }
    public void OnError(PlayFabError error)
    {
        Debug.Log("ERROR " + error);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Connected");
    }

    #endregion
    // Update is called once per frame

    #region SendDataToServer
    public void SendMoneyToServer(int _money)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Money",
                    Value = _money
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }
    public void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Money sended to server");
    }
    #endregion

    #region GetLeaderboard
    public void GetLeaderboard()
    {
        var req = new GetLeaderboardRequest
        {
            StatisticName = "Money",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(req, OnLeaderboardGet, OnError);
    }

    public void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        {

        }
    }
    #endregion

    public void SaveItemsOnMap()
    {
        Debug.Log("Saveitemsonmap");
        foreach (var item in so)
        {
            var req = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
            {
                {"Items", JsonUtility.ToJson(item) }
            }
            };
            PlayFabClientAPI.UpdateUserData(req, OnDataSend, OnError);
        }
        
    }

    public void OnDataSend(UpdateUserDataResult res)
    {
        Debug.Log("Data Sended");
    }





}
