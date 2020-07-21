using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// add this component to listen for GameEvents and trigger UnityEvents
/// </summary>

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent = default;
    [SerializeField] private UnityEvent responseEvent = default;

    private void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventRaised()
    {
        responseEvent.Invoke();
    }
}