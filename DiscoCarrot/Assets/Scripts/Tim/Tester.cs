using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    void Start()
    {
        GameEvents.OnReachedFarmTile += OnReachedFarmTile;
        GameEvents.OnLeaveFarmTile += OnLeaveFarmTile;
    }
    
    private void OnReachedFarmTile(FarmTile arg1, PlayerFarmAction arg2)
    {
        Debug.Log($"OnReachedFarmTile, detected need action {arg2}");
    }
    private void OnLeaveFarmTile(FarmTile obj)
    {
        Debug.Log($"OnLeaveFarmTile");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if(GameEvents.OnOneBeatPassed != null) GameEvents.OnOneBeatPassed.Invoke();
        }
    }
}
