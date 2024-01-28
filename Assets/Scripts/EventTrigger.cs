using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public bool oneTimeOnly = false;
    public UnityEvent eventsToCall;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            eventsToCall?.Invoke();
            if(oneTimeOnly)
                gameObject.SetActive(false);
        }
    }
}
