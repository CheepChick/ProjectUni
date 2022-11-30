using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtcItem :CountableItem 
{
    public EtcItem(EtcItemData data, int amount = 1) : base(data, amount) { }

    public bool Use() 
    {
        Amount--;
        return true;
    }
    protected override CountableItem Clone(int amount)
    {
        return new EtcItem(CountableData as EtcItemData, amount);
    }
}
