using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.PfEditor.Json;
using System;
using UnityEngine;
using JsonObject = PlayFab.Json.JsonObject;
using UnityEngine.UI;
public class PlayFabController : MonoBehaviour
{
    public static PlayFabController singleton;
    private string email;
    private string password;
    private string username;
    private bool isCreateAccount;
    public InputField userInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField confirmPasswordInput;
    public Text butoonText;
    public Text createAccountButtonText;
    public Text infoText;
    public GameObject loginPanel;


    //Stats
    public float playerHealth_;
    public float playerDamage_;
    public float playerHighScore_;

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
        userInput.gameObject.SetActive(false);
        confirmPasswordInput.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            GetLeaderboard();
        }
    }
    #region login
    private void OnLogInSuccess(LoginResult result)
    {
        infoText.text = "Success Login";
        Debug.Log("Success Login");
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        loginPanel.SetActive(false);
        GetStatistics();
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Success Register");
        infoText.text = "Success Register";
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        loginPanel.SetActive(false);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = username }, OnDisplayName,OnPlayFabError);

        playerHealth_ = 100;
        playerDamage_ = 10;
        playerHighScore_ = 0;
        StartCloudUpdateStats();
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
        infoText.text = error.ErrorMessage;
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        infoText.text = error.GenerateErrorReport();
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
            userInput.gameObject.SetActive(true);
            confirmPasswordInput.gameObject.SetActive(true);
            butoonText.text = "Register";
            createAccountButtonText.text = "Back";
            isCreateAccount = true;
        }
        else
        {
            userInput.gameObject.SetActive(false);
            confirmPasswordInput.gameObject.SetActive(false);
            butoonText.text = "Login";
            createAccountButtonText.text = "CreateAccount";
            isCreateAccount = false;
        }

    }
    private void OnClickRegister()
    {
        if (confirmPasswordInput.text != passwordInput.text)
        {
            infoText.text = "Password not match.";
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
        email = emailInput.text;
        print(email);
    }
    public void SetPassword()
    {
        password = passwordInput.text;
        print(password);
    }
    public void SetUsername()
    {
        username = userInput.text;
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
    public  void StartCloudUpdateStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new {playerHealth=playerHealth_,playerDamage=playerDamage_,playerHighScore=playerHighScore_}, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnCloudUpdateStats, OnErrorShared);
    }
    private static void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        // CloudScript returns arbitrary results, so you have to evaluate them one step and one parameter at a time
        Debug.Log(JsonWrapper.SerializeObject(result.FunctionResult));
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
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            Debug.Log(player.DisplayName + " : " + player.StatValue);
        }
    }
    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion leaderboard
}
