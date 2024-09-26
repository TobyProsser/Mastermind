using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardController : MonoBehaviour
{
    public CardObject thisCardsObject;

    [SerializeField]
    public string cardName;
    public TextMeshProUGUI text;
    public enum CardType {Damage, Block, Heal, Dodge, Prep, Disrupt,}
    [SerializeField] public CardType cardType;
    public enum SubType {Close, Ranged, Area}
    [SerializeField] public SubType subType;

    [SerializeField]
    public float typeAmount; //Damage amount, block amount
    public enum TimeOption {instant, normal, delayed}
    [SerializeField] public int time;
    
    
}
