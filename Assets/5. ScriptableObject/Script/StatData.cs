using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public enum StatType
{
    Health,
    Speed,
    ProjectileCount,
}

[CreateAssetMenu(fileName = "New StatData", menuName = "Stats/Character Stats", order = 1)]
public class StatData : ScriptableObject
{
    public string characterName;
    public List<StatEntry> stats;

} 
[Serializable]
public class StatEntry
{
    public StatType statType;
    public float baseValue;
}

    // Update is called once per frame