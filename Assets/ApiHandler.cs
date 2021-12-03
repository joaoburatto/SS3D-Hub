using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHandler : MonoBehaviour
{
    public static ApiHandler singleton { get; private set; }
    
    private static readonly HttpClient HttpClient;
    
    public string secret;

    public string apiUrl;
    
    public static event System.Action ApiRequestSent;
    public static event System.Action<string> ApiResponseReceived;
    
    
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

    public JSONNode SendApiRequest(RequestType type, string path, string formData = null)
    {
        UnityWebRequest request = new UnityWebRequest();
        
        path = apiUrl + path;
        var body = formData;
        
        //body = JsonUtility.ToJson(body);
        //StringContent httpContent = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        
        switch (type)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(path);
                break;
            case RequestType.POST:
                request.url = path;
                request.method = "post";
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(string.IsNullOrEmpty(JsonUtility.ToJson(body)) ? null : Encoding.UTF8.GetBytes(body));
                
                Debug.Log(body);
                request.SetRequestHeader("Accept", "application/json");
                request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
                break;
        }
        
        //UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(formData));
        //request.uploadHandler = customUploadHandler;
        request.SendWebRequest();
        ApiRequestSent.Invoke();

        while (!request.isDone) { } //Debug.Log("just wait bro"); // ah yes, wait functions  

        if (request.isNetworkError) Debug.Log(request.error);

        JSONNode data = JSON.Parse(request.downloadHandler.text);

        
        string formString = "";
        if (formData.Length > 0)
        {
            foreach (var value in body)
                formString += value;
        }

        string dataString = "";
        foreach (var value in data)
            dataString += value;

        Debug.Log("Form: " + formString);
        Debug.Log("Result: " + dataString);

        if (request.responseCode == 400)
        {
            return null;
        }
        ApiResponseReceived.Invoke(data["message"]);
        return data;
    }

    private IEnumerator HeartbeatCoroutine()
    {
        while (true)
        {
            // API heartbeat test (is it on?)
            JSONNode response = SendApiRequest(ApiHandler.RequestType.GET, "heartbeat");
            //Debug.Log(response["message"]);
            yield return new WaitForSeconds(1);  
        }
    }
    
    void InitializeSingleton()
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
