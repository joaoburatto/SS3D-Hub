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
        JSONArray data = new JSONArray();
        
        data["username"] = usernameInputField.text;
        data["password"] = passwordInputField.text;

        Debug.Log(data.ToString());
        
        JSONNode response = ApiHandler.singleton.SendApiRequest(ApiHandler.RequestType.POST, "user/authenticate", (string) data.ToString());
    }
}
