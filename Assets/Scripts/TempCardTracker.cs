using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCardTracker : MonoBehaviour
{
    public CardProvider cardProvider;
    public bool locked = false;
    public void CardLocked()
    {
        locked = true;
        cardProvider.SpawnCard(false);
    }
}
