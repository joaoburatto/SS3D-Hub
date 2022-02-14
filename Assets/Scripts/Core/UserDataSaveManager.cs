using System.IO;
using UnityEngine;
using Utils;

namespace Core
{
    /// <summary>
    /// Handles saving a txt file with json data for score purposes
    /// </summary>
    public static class UserDataSaveManager
    {
        private static string savePath;
        
        public static void UpdateSavePath()
        {
            savePath = Application.dataPath + "/account.txt";
        }
        
        public static void SaveStats(string username, string ckey, string token)
        {
            UserData newSaveData = new UserData(username, ckey, token);
            
            string saveDataAsJson = JsonUtility.ToJson(newSaveData);
            File.WriteAllText(savePath, saveDataAsJson);
        }
        
        public static void SaveStats(UserData newUserdata)
        {
            string saveDataAsJson = JsonUtility.ToJson(newUserdata);
            File.WriteAllText(savePath, saveDataAsJson);
        }
        
        public static void SaveStats(JsonRequest json)
        {
            json.TryGetData("username", out string username);
            json.TryGetData("id", out string ckey);
            json.TryGetData("token", out string token);

            UserData newUserdata = new UserData(username, ckey, token);
            string saveDataAsJson = JsonUtility.ToJson(newUserdata);
            File.WriteAllText(savePath, saveDataAsJson);
        }
        
        public static UserData LoadStats()
        {
            CreateSavefileIfNotExist();
            
            string saveDataString = File.ReadAllText(savePath);
            return JsonUtility.FromJson<UserData>(saveDataString);
        }

        public static void CreateSavefileIfNotExist()
        {
            if (!File.Exists(savePath))
            {
                SaveStats("", "", "");
            }
        }
        
        public class UserData
        {
            public string Username;
            public string Ckey;
            public string Token;

            public UserData(string username, string ckey, string token)
            {
                Username = username;
                Ckey = ckey;
                Token = token;
            }
        }
    }
}