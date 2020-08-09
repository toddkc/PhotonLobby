using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TrackNetworkHands : MonoBehaviour
{
    [SerializeField] private string objectName = default;
    private PhotonView view;
    public Transform objectToTrack;
    private Transform thisTransform;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        thisTransform = transform;
    }

    private void Start()
    {
        if (view.IsMine)
        {
            StartCoroutine(FindObject());
        }
    }

    private IEnumerator FindObject()
    {
        yield return null;
        while (objectToTrack == null)
        {
            yield return null;
            objectToTrack = GameObject.Find(objectName).transform;
        }
        yield return null;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (objectToTrack == null) return;
            thisTransform.position = objectToTrack.position;
            thisTransform.rotation = objectToTrack.rotation;
        }
    }
}