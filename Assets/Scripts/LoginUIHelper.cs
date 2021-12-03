using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Helps the user to send his credentials data to the API using UI
/// </summary>
public class LoginUIHelper : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonPressed);
    }
    
    /// <summary>
    /// Sends a request to the Hub API to check user credentials
    /// </summary>
    private void OnLoginButtonPressed()
    {
        string username = usernameInputField.textComponent.text;
        string password = passwordInputField.text;

        var json = "{'username': '" + username + "', " + "'password': '" + password + "'}";
        Debug.Log(json);
        
        JSONNode response = ApiHandler.singleton.SendApiRequest(ApiHandler.RequestType.POST, "user/authenticate",json);
        
        // TODO: User validation and local client data
        if (response == null)
            Debug.Log("account access is invalid");
        
        else
            Debug.Log("nice id, you can pass sire");
        
    }
}
