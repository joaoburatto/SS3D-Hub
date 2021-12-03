using System;
using UnityEngine;

public class ApiMessageUIHelper : MonoBehaviour
{
    public Animator animation;
    public GameObject messagePrefab;
    public Transform messageHolder;

    private void Start()
    {
        ApiHandler.ApiResponseReceived += message => OnApiResponseReceived(message);
        ApiHandler.ApiRequestSent += OnApiRequestSent;
    }

    private void OnApiResponseReceived(string message)
    {
        animation.SetBool("Toggle", false);
        Debug.Log(message);
    }

    private void OnApiRequestSent()
    {
        animation.SetBool("Toggle", true);
    }
    
    
}