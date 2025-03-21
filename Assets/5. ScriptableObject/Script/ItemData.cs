using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ItemData", menuName = "Itme/Stat Item", order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public List<StatEntry> statEntries;
    public bool isTemporary;
    public float duration;
 
}
