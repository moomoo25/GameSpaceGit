using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
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
    public float playerHealth;
    public float playerDamage;
    public float playerHighScore;

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
                    playerHealth = eachStat.Value;
                    break;
                case "damage":
                    playerDamage = eachStat.Value;
                    break;
                case "highScore":
                    playerHighScore = eachStat.Value;
                    break;
            }
        }
           
        
    }
    #endregion
}
