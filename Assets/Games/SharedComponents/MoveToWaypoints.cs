using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaypoints : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float pauseTime = 3;
    [Header("Waypoints")]
    [SerializeField]private List<Transform> waypoints = default;
    private WaitForSeconds pauseDelay;
    private bool waiting = false;
    private int currentIndex = 0;
    private Transform current;
    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        pauseDelay = new WaitForSeconds(pauseTime);
        current = waypoints[currentIndex];
    }

    private void FixedUpdate()
    {
        if (waiting) return;
        Move();
    }

    private void Move()
    {
        Vector3 _moveto = current.position - thisTransform.position;
        Vector3 _movement = _moveto.normalized;
        _movement *= speed * Time.deltaTime;
        if (_movement.magnitude >= _moveto.magnitude || _movement.magnitude == 0f)
        {
            thisTransform.position = current.position;
            UpdateWaypoint();
        }
        else
        {
            thisTransform.position += _movement;
        }
    }

    private void UpdateWaypoint()
    {
        currentIndex++;
        if (currentIndex >= waypoints.Count)
        {
            currentIndex = 0;
        }
        current = waypoints[currentIndex];
        if(pauseTime > 0)
        {
            StartCoroutine(WaitDelay());
        }
    }

    private IEnumerator WaitDelay()
    {
        waiting = true;
        yield return pauseDelay;
        waiting = false;
    }
}