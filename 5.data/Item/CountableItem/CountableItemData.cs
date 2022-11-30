using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountableItemData : ItemData
{
    [SerializeField] int _maxAmount = 99;
    public int MaxAmount => _maxAmount;
}
