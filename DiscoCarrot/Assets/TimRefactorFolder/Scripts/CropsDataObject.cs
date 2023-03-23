using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropsDataObject", menuName = "ScriptableObjects/CropsDataObject")]
public class CropsDataObject : ScriptableObject
{
    public List<CropsGrowthNode> growthNodes;
}

[Serializable]
public class CropsGrowthNode
{
    public string stateName;
    public GameObject presentationPrefab;
    public float duration;
    public CropsNeedsType[] needs;
}

public enum CropsNeedsType
{
    Water, Fertilize, Pesticide
}