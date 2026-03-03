using UnityEngine;
using UnityEngine.EventSystems;

public class SafeEventSystem : MonoBehaviour
{
    void Awake()
    {
        var allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (allEventSystems.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}