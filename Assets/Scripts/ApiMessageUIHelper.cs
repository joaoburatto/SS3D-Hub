using System;
using UnityEngine;

/// <summary>
/// Receives API events and manages what to do with them
/// </summary>
public class ApiMessageUIHelper : MonoBehaviour
{
    public Animator animator;
    
    // TODO: message handler, a little notification tray
    public GameObject messagePrefab;
    public Transform messageHolder;

    private void Start()
    {
        ApiHandler.ApiResponseReceived += message => OnApiResponseReceived(message);
        ApiHandler.ApiRequestSent += OnApiRequestSent;
    }

    private void OnApiResponseReceived(string message)
    {
        animator.SetBool("Toggle", false);
        Debug.Log(message);
    }

    private void OnApiRequestSent()
    {
        animator.SetBool("Toggle", true);
    }

    private void OnDestroy()
    {
        ApiHandler.ApiResponseReceived -= OnApiResponseReceived;
        ApiHandler.ApiRequestSent -= OnApiRequestSent;
    }
}