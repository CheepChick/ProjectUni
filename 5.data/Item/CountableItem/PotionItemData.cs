using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Potion", menuName = "Item Data/Potion", order = 4)]
public class PotionItemData : CountableItemData
{

    [SerializeField] int _intValue;
    [SerializeField] float _floatValue;

    public float IntValue => _intValue;
    public float FloatValue => _floatValue;
    public override Item CreateItem()
    {
        return new PotionItem(this);
    }
}
