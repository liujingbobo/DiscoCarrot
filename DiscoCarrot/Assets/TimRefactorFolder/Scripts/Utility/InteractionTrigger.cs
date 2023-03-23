using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public bool debugMode = false;
    public IInteractable interactableClassInstance; 
        
    private List<InteractionTrigger> _markedInteractionTriggers = new List<InteractionTrigger>();

    public void RegisterAsInteractable(IInteractable i)
    {
        interactableClassInstance = i;
    }
    
    public void Interact()
    {
        if(interactableClassInstance != null) interactableClassInstance.OnInteract();
    }
    
    public List<InteractionTrigger> GetAllMarkedInteractionTrigger()
    {
        return _markedInteractionTriggers;
    }

    public InteractionTrigger GetClosestInteractionTrigger()
    {
        var dis = float.MaxValue;
        InteractionTrigger ret = null;
        foreach (var io in _markedInteractionTriggers)
        {
            var newDis = (io.transform.position - transform.position).magnitude;
            if (newDis < dis)
            {
                dis = newDis;
                ret = io;
            } 
        }
        return ret;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (debugMode) Debug.Log($"[InteractionObject]:{gameObject.name} OnTriggerEnter {other.gameObject.name}");
        var interactionObj = other.GetComponent<InteractionTrigger>();
        if (interactionObj && !_markedInteractionTriggers.Contains(interactionObj))
        {
            if(interactableClassInstance != null) interactableClassInstance.OnAddMarkedInteractionObject(interactionObj);
            _markedInteractionTriggers.Add(interactionObj);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (debugMode) Debug.Log($"[InteractionObject]:{gameObject.name} OnTriggerExit {other.gameObject.name}");
        var interactionObj = other.GetComponent<InteractionTrigger>();
        if (interactionObj && _markedInteractionTriggers.Contains(interactionObj))
        {
            if(interactableClassInstance != null) interactableClassInstance.OnRemoveMarkedInteractionObject(interactionObj);
            _markedInteractionTriggers.Remove(interactionObj);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debugMode)
        {
            Gizmos.color = Color.yellow;
            foreach (var it in _markedInteractionTriggers)
            {
                Gizmos.DrawSphere(it.transform.position, 0.3f);
            }
            Gizmos.color = Color.red;
            InteractionTrigger closet = GetClosestInteractionTrigger();
            if(closet  != null)
                Gizmos.DrawSphere(GetClosestInteractionTrigger().transform.position, 0.3f);
        }
        
    }
}

public interface IInteractable
{
    public void OnInteract();
    public void OnAddMarkedInteractionObject(InteractionTrigger interactionTrigger);
    public void OnRemoveMarkedInteractionObject(InteractionTrigger interactionTrigger);
}
