using System;
using Core;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Stores local user data
    ///
    /// TODO: Save token locally so he doesn't need to log in everytime
    /// </summary>
    public class LocalClientData : MonoBehaviour
    {
        private static LocalClientData singleton;
        [SerializeField] private string username;
        [SerializeField] private string ckey;
        [SerializeField] private string token;

        private void Start()
        {
            Setup();
        }

        private void Awake()
        {
            InitializeSingleton();
        }

        private void Setup()
        {
            UserDataSaveManager.UserData data = UserDataSaveManager.LoadStats();
            username = data.Username;
            ckey = data.Ckey;
            token = data.Token;
        }

        private void InitializeSingleton()
        {
            if (singleton != null && singleton != this) { 
                Destroy(gameObject);
            }
            else
            {
                singleton = this;   
            }
        }
    }
}