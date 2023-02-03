using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public int beatCountFromStart;
    public FarmTile[] targetTiles;
    public float bpm = 123;
    void Start()
    {
        beatCountFromStart = 0;
        StartCoroutine(AddBeat());
        
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
        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (var targetTile in targetTiles)
            {
                GameEvents.OnFarmActionDone.Invoke(targetTile, PlayerFarmAction.PlowLand, ActionLevel.Perfect);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (var targetTile in targetTiles)
            {
                GameEvents.OnFarmActionDone.Invoke(targetTile, PlayerFarmAction.PlantSeed, ActionLevel.Perfect);
            }
        }
    }

    IEnumerator AddBeat()
    {
        while (true)
        {
            beatCountFromStart += 1;
            if(GameEvents.OnOneBeatPassed != null) GameEvents.OnOneBeatPassed.Invoke();
            yield return new WaitForSeconds(  60f / bpm);
        }
    }
}
