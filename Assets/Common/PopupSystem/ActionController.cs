using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionController
{
    public static Action<Transform, Action> ClaimCoin;
    public static Action<int> UpdateCoinText;
    public static Action GetCurCoinText;
    public static Action UpdateBoosterInGameplay;
    // public static Action<List<ItemData>> OnClaimItem;
}
