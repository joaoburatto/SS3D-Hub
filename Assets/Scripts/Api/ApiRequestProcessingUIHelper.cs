using UnityEngine;
using Utils;

/// <summary>
/// Receives API events and manages what to do with them
/// </summary>
public class ApiRequestProcessingUIHelper : MonoBehaviour
{
    // TODO: message handler, a little notification tray
    public GameObject processingRequestPrefab;
    public GameObject processingRequestInstance;
    public Transform processingRequestHolder;

    private const float scaleInOutTime = .05f;
        
    private void Start()
    {
        ApiHandler.ApiResponseReceived += OnApiResponseReceived;
        ApiHandler.ApiRequestSent += OnApiRequestSent;
    }

    private void OnApiResponseReceived(JSONNode message)
    {
        LeanTween.scale(processingRequestInstance, Vector3.zero, scaleInOutTime);
        Debug.Log(message);
    }

    private void OnApiRequestSent(JSONNode message)
    {
        if (processingRequestInstance == null)
        {
            CreateNewInstance();
        }
        LeanTween.scale(processingRequestInstance, Vector3.one, scaleInOutTime);
    }

    private void CreateNewInstance()
    {
        processingRequestInstance = Instantiate(processingRequestPrefab, processingRequestHolder.position, Quaternion.identity,processingRequestHolder);
        processingRequestInstance.transform.localScale = Vector3.zero;
    }
    private void OnDestroy()
    {
        ApiHandler.ApiResponseReceived -= OnApiResponseReceived;
        ApiHandler.ApiRequestSent -= OnApiRequestSent;
    }
}