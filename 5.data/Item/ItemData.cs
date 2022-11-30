using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField] int _id;
    [SerializeField] string _name;
    [SerializeField] string _tooltip;
    [SerializeField] Sprite _iconSprite;
    [SerializeField] GameObject _dropItemPrefab;
    [SerializeField] int _dropRate;

    public int ID => _id;
    public string Name => _name;
    public string Tooltip => _tooltip;
    public Sprite IconSprite => _iconSprite;
    public int DropRate => _dropRate;
    public abstract Item CreateItem();
}
