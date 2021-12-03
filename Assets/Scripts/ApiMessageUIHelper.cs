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
        ApiHandler.ApiRequestSent += message => OnApiRequestSent(message);

        animator.enabled = false;
    }

    private void OnApiResponseReceived(string message)
    {
        animator.enabled = true;
        animator.SetBool("Toggle", false);
        Debug.Log(message);
    }

    private void OnApiRequestSent(string message)
    {
        animator.enabled = true;
        animator.SetBool("Toggle", true);
    }

    private void OnDestroy()
    {
        ApiHandler.ApiResponseReceived -= OnApiResponseReceived;
        ApiHandler.ApiRequestSent -= OnApiRequestSent;
    }
}