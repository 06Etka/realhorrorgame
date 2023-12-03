using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] float triggerDestroyTime;
    [SerializeField] TriggerType triggerType;
    [SerializeField] UnityEvent triggerEvent;
    bool isTriggerHapenning = false;
    enum TriggerType 
    {
        Any,
        Player
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isTriggerHapenning) { return; }
        if(triggerType == TriggerType.Any)
        {
            if (other.tag != "Player")
            {
                Trigger();
            }
        } else if(triggerType == TriggerType.Player)
        {
            if(other.tag == "Player")
            {
                Trigger();
            }
        }
    }

    void Trigger()
    {
        isTriggerHapenning = true;
        triggerEvent?.Invoke();
        Destroy(gameObject, triggerDestroyTime);
    }
}
