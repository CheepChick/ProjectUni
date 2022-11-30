using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Maps", menuName = "ScriptableObjects/MapInfo")]
public class MapInfo : ScriptableObject
{
    [SerializeField] int _mapStage;
    [SerializeField] int _mapArea;
    [SerializeField] int _monsterLevel;
    [SerializeField] int[] _commonMonsterID;
    [SerializeField] int _bossMonsterID;
    [SerializeField] string _mapSoundName;


    public int MapStage => _mapStage;
    public int MapArea => _mapArea;
    public int MonsterLevel => _monsterLevel;
    public int[] CommonMonsterID => _commonMonsterID;
    public int BossMonsterID => _bossMonsterID;

    public string MapSoundName => _mapSoundName;
}
