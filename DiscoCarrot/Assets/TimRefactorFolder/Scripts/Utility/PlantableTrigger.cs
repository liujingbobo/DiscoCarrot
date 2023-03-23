using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableTrigger : InteractionTrigger
{
    public void Plant(int seedID)
    {
        if((interactableClassInstance is IPlantable) != null) 
            (interactableClassInstance as IPlantable).OnPlant(seedID);
    }
}

public interface IPlantable: IInteractable
{
    public void OnPlant(int seedID);
}
