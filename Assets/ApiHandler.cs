using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHandler : MonoBehaviour
{
    public static ApiHandler singleton { get; private set; }
    
    public string secret;

    public string apiUrl;
    

    public enum RequestType
    {
        GET = 0,
        POST = 1,
        DELETE = 2,
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
        
        switch (type)
        {
            case RequestType.GET:
                request = UnityWebRequest.Get(path);
                break;
            case RequestType.POST:
                request = UnityWebRequest.Post(path, body);
                break;
        }

        request.SetRequestHeader("Content-Type", "application/json");
        //UploadHandler customUploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(formData));
        //request.uploadHandler = customUploadHandler;
        request.SendWebRequest();

        while (!request.isDone) { } //Debug.Log("just wait bro"); // ah yes, wait functions  

        if (request.isNetworkError) Debug.Log(request.error);

        JSONNode data = JSON.Parse(request.downloadHandler.text);

        string formString = "";
        foreach (var value in body)
        {
            formString += value;
        }
        
        string dataString = "";
        foreach (var value in data)
        {
            dataString += value;
        }
        
        Debug.Log("Form: " + dataString);
        Debug.Log("Result: " + dataString);
        
        return data;
    }

    private IEnumerator HeartbeatCoroutine()
    {
        while (true)
        {
            // API heartbeat test (is it on?)
            JSONNode response = ApiHandler.singleton.SendApiRequest(ApiHandler.RequestType.GET, "heartbeat");
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
