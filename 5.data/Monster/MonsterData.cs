using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Monster")]
public class MonsterData : ScriptableObject
{
    [SerializeField] int _index;
    [SerializeField] string _name;
    [SerializeField] int _atk;
    [SerializeField] int _def;
    [SerializeField] int _hp;
    [SerializeField] int _range;
    [SerializeField] int[] _dropPotionItem;
    [SerializeField] int[] _dropEtcItem;
    [SerializeField] int[] _dropEquipmentItem;
    public int Index => _index;
    public string Name => _name;
    public int Atk => _atk;
    public int Def => _def;
    public int Hp => _hp;
    public float Range => _range;
    public int[] DropPotionItem => _dropPotionItem;
    public int[] DropEtcItem => _dropEtcItem;
    public int[] DropEquipmentItem => _dropEquipmentItem;


}

