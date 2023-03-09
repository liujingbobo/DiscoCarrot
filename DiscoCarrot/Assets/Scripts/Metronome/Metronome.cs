using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Metronome")]
public class Metronome : SerializedScriptableObject
{
    public bool startOnDownbeat = true;
    public List<BeatAction> Actions = new List<BeatAction>();

    private string Detail()
    {
        if (Actions.IsNullOrEmpty()) return string.Empty;

        string detail = startOnDownbeat ? "O · O Start" : "· O · Start";

        string GetDirectionChar(ArrowDirection dir)
        {
            return dir switch
            {
                ArrowDirection.Up => "↑",
                ArrowDirection.Right => "→",
                ArrowDirection.Down => "↓",
                ArrowDirection.Left => "←",
            };
        }
        for (int i = 0; i < Actions.Count; i++)
        {
            if (Actions[i].Action == BeatActionType.Hold)
            {
                var StartAction = Actions[i];
                detail += $" {GetDirectionChar(Actions[i].Direction)}";

                int newIndex = i + 1;
                while (newIndex < Actions.Count)
                {
                                        
                }
            }
        }
    }
    
    
}
