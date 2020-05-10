using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
public class PlayFabLogin : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        userInput.gameObject.SetActive(false);
        confirmPasswordInput.gameObject.SetActive(false);
    }

    private void OnLogInSuccess(LoginResult result)
    {
        infoText.text = "Success Login";
        Debug.Log("Success Login");
        PlayerPrefs.SetString("Email", email);
        PlayerPrefs.SetString("Password", password);
        loginPanel.SetActive(false);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Success Register");
        infoText.text = "Success Register";
        PlayerPrefs.SetString("Email",email);
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
        if(confirmPasswordInput.text != passwordInput.text)
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
}
