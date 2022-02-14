using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

public class ApiHandler : MonoBehaviour
{
    public static ApiHandler singleton { get; private set; }
    
    // responsible for requests
    private static readonly HttpClient HttpClient;
 
    public string apiUrl;
    
    public static event Action<JSONNode> ApiRequestSent;
    public static event Action<JSONNode> ApiResponseReceived;
    public static event Action<int> ApiConnectionLost;
    private int ApiTimeoutSeconds = 0;

    public enum RequestType
    {
        GET = 0,
        POST = 1,
        DELETE = 2,
    }

    static ApiHandler()
    {
        HttpClient = new HttpClient();
    }
    
    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        StartCoroutine(HeartbeatCoroutine());
    }

    /// <summary>
    /// Sends an request to the Hub API (CentCom).
    /// </summary>
    /// <param name="type">Request type (GET, POST, PUT...)</param>
    /// <param name="path">The API url path (ie: "users/login""</param>
    /// <param name="formData">The data that is in the body of the request, should always be a JSON</param>
    /// <param name="ignoreMessageSystem"></param>
    /// <returns></returns>
    public JSONNode SendApiRequest(RequestType type, string path, string formData = null, bool ignoreMessageSystem = false)
    {
        UnityWebRequest request = new UnityWebRequest();
        
        string fullPath = apiUrl + path;

        switch (type)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(fullPath);
                break;
            case RequestType.POST:
                // this code ensures the data is sent properly to the API
                request.url = fullPath;
                request.method = "post";
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(JsonUtility.ToJson(formData)) ? null : Encoding.UTF8.GetBytes(formData));
                request.SetRequestHeader("Accept", "application/json");
                request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
                break;
            case RequestType.DELETE:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        request.SendWebRequest();
        if (!ignoreMessageSystem)
            ApiRequestSent?.Invoke(fullPath);

        while (!request.isDone) { } //Debug.Log("just wait bro"); // ah yes, wait functions  

        if (request.isNetworkError)
        {
            if (path.Equals(SS3DRequests.Heartbeat))
            {
                Debug.Log(request.error);
                ApiTimeoutSeconds++;
                ApiConnectionLost?.Invoke(ApiTimeoutSeconds);
            }
        }
        else
        {
            ApiTimeoutSeconds = 0;
        }
        // transforms the received data into a JSON object
        JSONNode data = JSON.Parse(request.downloadHandler.text);

        // in case we get an error message
        if (request.responseCode == 400)
            return null;
        
        if (!ignoreMessageSystem)
            ApiResponseReceived?.Invoke(data);
        return data;
    }

    /// <summary>
    /// A request send every second to check the API status
    /// </summary>
    private IEnumerator HeartbeatCoroutine()
    {
        while (true)
        {
            JSONNode response = SendApiRequest(ApiHandler.RequestType.GET, SS3DRequests.Heartbeat, null, true);
            Debug.Log(response["message"]);

            yield return new WaitForSeconds(1);
        }
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
