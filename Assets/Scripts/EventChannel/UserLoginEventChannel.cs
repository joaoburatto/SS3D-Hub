using System;
using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(fileName = "UserLoginEventChannel", menuName = "SS3D Event Channel/ UserLoginEventChannel", order = 0)]
    public class UserLoginEventChannel : ScriptableObject
    {
        public static event Action UserLoggedIn;

        public void OnUserLogin()
        {
            UserLoggedIn?.Invoke();
        }
    }
}