using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private string email;
    private string password;
    private string username;
    // Start is called before the first frame update
    void Start()
    {
        LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest { };
    }

    private void OnLogInSuccess(LoginResult result)
    {
        Debug.Log("Success Login");
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Success Register");
    }
    private void OnLogInFaill(PlayFabError error)
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest { Email = email ,Password = password,Username=username};
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,OnRegisterSuccess, OnRegisterFailure);
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    public void OnClickLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = email , Password = password};
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLogInSuccess, OnLogInFaill);
    }
    public void SetEmaill(string email_)
    {
        email = email_;
    }
    public void SetPassword(string password_)
    {
        password = password_;
    }
    public void SetUsername(string username_)
    {
        username = username_;
    }
}
