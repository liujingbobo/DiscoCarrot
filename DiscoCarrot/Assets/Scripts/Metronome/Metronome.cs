using System;
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
    public readonly List<BeatAction> Actions = new List<BeatAction>();
}
