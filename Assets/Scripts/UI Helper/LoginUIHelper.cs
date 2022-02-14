using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI_Helper
{
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
            JsonRequest json = new JsonRequest();
            json.AddData("username", usernameInputField.textComponent.text);
            json.AddData("password", passwordInputField.text);

            Debug.Log(json.ToString());
            JSONNode response = ApiHandler.singleton.SendApiRequest(ApiHandler.RequestType.POST, SS3DRequests.User.Authenticate,json.ToString());
        
            // TODO: User validation and local client data
            if (response == null)
            {
                Debug.Log("account access is invalid");
            }
            else
            {
                UserDataSaveManager.SaveStats(new JsonRequest(response));
                Debug.Log("nice id, you can pass sire");
            }
        }
    }
}
