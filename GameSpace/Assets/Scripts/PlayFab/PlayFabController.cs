using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

using System;
using UnityEngine;
using JsonObject = PlayFab.Json.JsonObject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayFabController : MonoBehaviour
{
    public static PlayFabController singleton;
    private LoginSceneUI loginSceneUI;
    private string email;
    private string password;
    private string username;
    private bool isCreateAccount;
    private string myId = null;
    public string leaderBoardText;
    //

    public GameObject loginPanel;
    //
    private bool isSetPlayerData;
    private bool isGetPlayerData;
    private bool isSuccessOperaion;
    //Stats
    public float playerHealth_;
    public float playerDamage_;
    public float playerHighScore_;

    //Data
    public string playerClass;
    public string playerRace;
    public string playerSkill;
    private bool onload;
    public int playerColorIndex;

    private void OnEnable()
    {
        if (PlayFabController.singleton == null)
        {
            PlayFabController.singleton = this;
        }
        else
        {
            if (PlayFabController.singleton != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        loginSceneUI = GetComponent<LoginSceneUI>();
        loginSceneUI.userInput.gameObject.SetActive(false);
        loginSceneUI.confirmPasswordInput.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            GetPlayerData();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            SetUserData("Warrior", "Human", "Holy Hammer", "0");
        }
        if (isGetPlayerData && isSetPlayerData)
        {
            if (isSuccessOperaion == false)
            {
                SceneManager.LoadScene(1);
                isSuccessOperaion = true;
            }
        }
    }
    #region login
    private void OnLogInSuccess(LoginResult result)
    {
        loginSceneUI.infoText.text = "Success Login";
        Debug.Log("Success Login");
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        loginPanel.SetActive(false);
        GetStatistics();
        GetLeaderboard();
        myId = result.PlayFabId;
        isSetPlayerData = true;
        StartCoroutine(LoadNextScene(false));
    
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Success Register");
        loginSceneUI.infoText.text = "Success Register";
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        loginPanel.SetActive(false);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName, OnPlayFabError);
        GetLeaderboard();
        playerHealth_ = 100;
        playerDamage_ = 10;
        playerHighScore_ = 0;
        StartCloudUpdateStats();
        myId = result.PlayFabId;
        StartCoroutine(LoadNextScene(true));
     
   
    }
    IEnumerator LoadNextScene(bool isRegister)
    {
        if(isRegister)
        SetUserData("Warrior", "Human", "Holy Hammer", "0");
        yield return new WaitForEndOfFrame();
        GetPlayerData();
     
    }
    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + "is your new display name");
    }
    void OnPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    private void OnLogInFaill(PlayFabError error)
    {
        loginSceneUI.infoText.text = error.ErrorMessage;
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        loginSceneUI.infoText.text = error.GenerateErrorReport();
    }
    public void OnClickLogin()
    {
        if (!isCreateAccount)
        {
            SetUsername();
            SetEmaill();
            SetPassword();
            var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFaill);
        }
        else
        {
            OnClickRegister();
        }

    }
    public void OnClickCreateAccount()
    {
        if (!isCreateAccount)
        {
            loginSceneUI.userInput.gameObject.SetActive(true);
            loginSceneUI.confirmPasswordInput.gameObject.SetActive(true);
            loginSceneUI.butoonText.text = "Register";
            loginSceneUI.createAccountButtonText.text = "Back";
            isCreateAccount = true;
        }
        else
        {
            loginSceneUI.userInput.gameObject.SetActive(false);
            loginSceneUI.confirmPasswordInput.gameObject.SetActive(false);
            loginSceneUI.butoonText.text = "Login";
            loginSceneUI.createAccountButtonText.text = "CreateAccount";
            isCreateAccount = false;
        }

    }
    private void OnClickRegister()
    {
        if (loginSceneUI.confirmPasswordInput.text != loginSceneUI.passwordInput.text)
        {
            loginSceneUI.infoText.text = "Password not match.";
            return;
        }

        SetUsername();
        SetEmaill();
        SetPassword();
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
    public void SetEmaill()
    {
        email = loginSceneUI.emailInput.text;
        print(email);
    }
    public void SetPassword()
    {
        password = loginSceneUI.passwordInput.text;
        print(password);
    }
    public void SetUsername()
    {
        username = loginSceneUI.userInput.text;
    }
    #endregion
    #region request
    public void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate{ StatisticName = "health",Value = 100 },
                new StatisticUpdate{ StatisticName = "damage",Value = 10 },
                new StatisticUpdate{ StatisticName = "highScore",Value = 1000 },
            }
        },
        result => { Debug.Log("Userstatistic updated"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            switch (eachStat.StatisticName)
            {
                case "health":
                    playerHealth_ = eachStat.Value;
                    break;
                case "damage":
                    playerDamage_ = eachStat.Value;
                    break;
                case "highScore":
                    playerHighScore_ = eachStat.Value;
                    break;
            }
        }


    }
    public void StartCloudUpdateStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { playerHealth = playerHealth_, playerDamage = playerDamage_, playerHighScore = playerHighScore_ }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudUpdateStats, OnErrorShared);
    }
    private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
     
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        print(jsonResult.Values);
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue); // note how "messageValue" directly corresponds to the JSON values set in CloudScript
        Debug.Log((string)messageValue);
    }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion
    #region leaderboard
    public void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "highScore", MaxResultsCount = 20 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }
    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        // print 1st highScore  Debug.Log(result.Leaderboard[0].StatValue);
        int i = 0;
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            i++;
            Debug.Log(player.DisplayName + " : " + player.StatValue);
            leaderBoardText += player.DisplayName + " : " + player.StatValue + "\n";
            if (i == 5)
            {
                break;
            }
        }
    }
    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion leaderboard
    #region PlayerData
    public void GetPlayerData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myId,
            Keys = null

        }, UserDataSuccess, UserDataFailed);
    }
    void UserDataSuccess(GetUserDataResult result)
    {
        if(result.Data==null || !result.Data.ContainsKey("class")|| !result.Data.ContainsKey("race") || !result.Data.ContainsKey("skill") || !result.Data.ContainsKey("colorIndex"))
        {

        }
        else
        {
            playerClass = result.Data["class"].Value;
            playerRace =result.Data["race"].Value;
            playerSkill= result.Data["skill"].Value;

            playerColorIndex = int.Parse(result.Data["colorIndex"].Value);
           isGetPlayerData = true;
        }
    }
    void UserDataFailed(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    public void SetUserData(string playerClass,string playerRace,string playerSkill,string playerColorIndex)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
        {
            {"class",playerClass},
            {"race",playerRace},
            {"skill",playerSkill},
            {"colorIndex",playerColorIndex}
        }
        }, SetDataSuccess, UserDataFailed);
      
    }
    void SetDataSuccess(UpdateUserDataResult result)
    {
        isSetPlayerData = true;
        Debug.Log(result.DataVersion);
    }
    public void EditPlayerData(string playerClass, string playerRace, string playerSkill, string playerColorIndex)
    {
        if (onload == false)
        {
            isSetPlayerData = false;
            isGetPlayerData = false;
            print("onLoad");
            StartCoroutine(EditData(playerClass, playerRace, playerSkill, playerColorIndex));
            onload = true;
        }

    }
    IEnumerator EditData(string playerClass, string playerRace, string playerSkill, string playerColorIndex)
    {
            SetUserData(playerClass, playerRace, playerSkill, playerColorIndex);
        while (isSetPlayerData == false)
        {
            yield return new WaitForEndOfFrame();
        }
      
        GetPlayerData();
        while (isGetPlayerData == false)
        {
            yield return new WaitForEndOfFrame();
        }
        print("SaveData");
        onload = false;
    }
    #endregion
}
