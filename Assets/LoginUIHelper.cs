using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginUIHelper : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonPressed);
    }

    private void OnLoginButtonPressed()
    {
        string json; 
        string username = usernameInputField.textComponent.text;
        string password = passwordInputField.text;

        json = "{'username': '" + username + "', " +
               "'password': '" + password +
               "'}";

        Debug.Log("username: " + usernameInputField.textComponent.text + " password: " + passwordInputField.text);
        Debug.Log(json);
        
        JSONNode response = ApiHandler.singleton.SendApiRequest(ApiHandler.RequestType.POST, "user/authenticate", json);

        if (response == null) 
        {
            Debug.Log("account access is invalid");
        }
        else
        {
            Debug.Log("nice id, you can pass sire");
        }
    }
}
