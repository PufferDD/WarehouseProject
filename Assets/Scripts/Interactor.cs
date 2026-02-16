using UnityEngine;

public class Interactor : MonoBehaviour
{
    public float range = 3f;
    public KeyCode key = KeyCode.E;

    void Update()
    {
        if (!Input.GetKeyDown(key)) return;

        if (Physics.Raycast(transform.position, transform.forward, out var hit, range))
        {
            var i = hit.collider.GetComponentInParent<IInteractable>();
            if (i != null)
            {
                Debug.Log(i.Prompt);
                i.Interact(gameObject);
            }
        }
    }
}


