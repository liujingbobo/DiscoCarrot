using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    public LayerMask[] triggerLayerMasks;
    public LayerMask[] ignoreLayerMasks;
    
    public List<IGameTriggerEventReceiver> receivers = new List<IGameTriggerEventReceiver>();

    public void RegisterTrigger(IGameTriggerEventReceiver receiver)
    {
        receivers.Add(receiver);
    }
    public bool LayerChecked(int layer)
    {
        if (triggerLayerMasks.Length > 0 && !triggerLayerMasks.Contains(layer)) return false;
        if (ignoreLayerMasks.Length > 0 && ignoreLayerMasks.Contains(layer)) return false;
        return true;
    }
    public void OnTriggerEnter(Collider other)
    {
        var l = other.gameObject.layer;
        if (LayerChecked(other.gameObject.layer))
        {
            foreach (var receiver in receivers)
            {
                receiver.TriggeredEnter(other);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        var l = other.gameObject.layer;
        if (LayerChecked(other.gameObject.layer))
        {
            foreach (var receiver in receivers)
            {
                receiver.TriggeredStay(other);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var l = other.gameObject.layer;
        if (LayerChecked(other.gameObject.layer))
        {
            foreach (var receiver in receivers)
            {
                receiver.TriggeredExit(other);
            }
        }
    }
}

public interface IGameTriggerEventReceiver
{
    public void TriggeredEnter(Collider other);
    public void TriggeredStay(Collider other);
    public void TriggeredExit(Collider other);
}