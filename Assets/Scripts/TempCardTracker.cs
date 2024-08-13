using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCardTracker : MonoBehaviour
{
    public CardProvider cardProvider;
    public void CardLocked()
    {
        cardProvider.SpawnCard();
    }
}
