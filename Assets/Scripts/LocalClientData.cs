using UnityEngine;

namespace SimpleJSON
{
    /// <summary>
    /// Stores local user data
    ///
    /// TODO: Save token locally so he doesn't need to log in everytime
    /// </summary>
    public class LocalClientData : MonoBehaviour
    {
        private string username;
        private string ckey;
    }
}