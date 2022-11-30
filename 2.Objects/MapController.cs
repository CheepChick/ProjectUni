using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] Transform _playerTF;
    [SerializeField] Transform _respawnTF;
    [SerializeField] MapInfo _mapInfo;

    List<Transform> _respawnList;
    List<Transform> _playerRespawnList;
    private void Awake()
    {
        _respawnList = new List<Transform>();
        for(int i = 0; i < _respawnTF.childCount; i++)
        {
            _respawnList.Add(_respawnTF.GetChild(i));
        }
        _playerRespawnList = new List<Transform>();
        for(int i=0; i< _playerTF.childCount; i++)
        {
            _playerRespawnList.Add(_playerTF.GetChild(i));
        }
        for (int i=0; i< _respawnList.Count; i++)
        {
            RespawnMonster(i);
        }
        RespawnPlayer(0);
        MapBGSet();
    }
    void MapBGSet()
    {
        DefineEnum.eBGSound bgName = DefineEnum.StringToEnum<DefineEnum.eBGSound>(_mapInfo.MapSoundName);
        AudioClip bg = ResourcesPoolManager._instance.GetBGAudio(bgName);
        SoundManager._instance.BGSoundPlay(bg);
    }
    public void RespawnPlayer(int i)
    {
        GameObject go = Instantiate(ResourcesPoolManager._instance.Player,_playerRespawnList[i]);
        go.GetComponent<PlayerController>().GetRespwanPoint(_playerRespawnList);
    }
    public void RespawnMonster(int i)
    {
        string name = DataBaseManager._instance._monsterList[_mapInfo.CommonMonsterID[0]].Name;
        DefineEnum.eMonsterPrefab monsterPrefab = DefineEnum.StringToEnum<DefineEnum.eMonsterPrefab>(name);
        GameObject go = Instantiate(ResourcesPoolManager._instance.GetMonsterPrefab(monsterPrefab), _respawnList[i]);
        MonsterObj monsterObj = go.GetComponent<MonsterObj>();
        monsterObj.InitSetData(_mapInfo.MonsterLevel, DataBaseManager._instance._monsterList[_mapInfo.CommonMonsterID[0]],this,i);
    }
    public IEnumerator Respawn(int i)
    {
        yield return new WaitForSeconds(7.0f);
        RespawnMonster(i);
    }

}
