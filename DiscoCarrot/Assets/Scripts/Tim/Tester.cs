using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public Player rabbitPlayer;
    public TMP_Text textMeshProText;
    public int beatCountFromStart;
    public FarmTile targetTile;
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
        targetTile = arg1;
        Debug.Log($"OnReachedFarmTile, detected need action {arg2}");
    }
    private void OnLeaveFarmTile(FarmTile obj)
    {
        if (targetTile == obj) targetTile = null;
        Debug.Log($"OnLeaveFarmTile");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (targetTile != null)
            {
                rabbitPlayer.PlayAnim(Config.tmpFarmActionToAnim[targetTile.GetNeededPlayerFarmAction()]);
                GameEvents.OnFarmActionDone.Invoke(targetTile, targetTile.GetNeededPlayerFarmAction(), ActionLevel.Perfect);
            }
        }

        textMeshProText.text = beatCountFromStart.ToString();
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
