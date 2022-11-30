using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item_Etc",menuName ="Item Data/Etc",order = 3)]
public class EtcItemData : CountableItemData 
{
    [SerializeField] float _value;

    public float Value => _value;
    public override Item CreateItem()
    {
        return new EtcItem(this);
    }
}
