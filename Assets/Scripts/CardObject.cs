using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardObject
{   [SerializeField]
    public string cardName;
    [SerializeField]
    public string cardType; //Damage, Block, Heal, Dodge, Prep, Disrupt
    [SerializeField]
    public string subType; //Ranged, Close, Area
    [SerializeField]
    public float typeAmount; //Damage amount, block amount
    [SerializeField]
    public int time; // 0, 1, 2, Time also determinds if heavy, medium or light.

    [SerializeField]
    public Sprite cardIcon;
}
