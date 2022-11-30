using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item 
{
    public ItemData _data { get; private set; }
    public Item(ItemData data) => _data = data;
}
