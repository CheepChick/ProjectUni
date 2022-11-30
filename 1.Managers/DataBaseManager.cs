using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataBaseManager : TSingleton<DataBaseManager>
{
    [Header("Scriptable Object")]
    [SerializeField] MonsterData[] _monsterData;
    [SerializeField] EtcItemData[] _etcItems;
    [SerializeField] PotionItemData[] _potionItems;

    public Dictionary<int, MonsterData> _monsterList;
    public Dictionary<int, EtcItemData> _etcList;
    public Dictionary<int, PotionItemData> _potionItemList;

    public PlayerInfo _playerInfo{ get; set;  }

    JsonFileManager _json;
    string _path;


    private void Awake()
    {
        Init();
        _json = new JsonFileManager();
        _path = Application.dataPath;
        _playerInfo = new PlayerInfo();
        LoadPlayerStat();
        SetMonsterData();
        SetItemData();
    }
    //임시
    private void Start()
    {
        SceneControllManager._instance.StartIngameScene(); 

    }

    protected override void Init()
    {
        base.Init();

    }
    
    public void SavePlayerStat()
    {
        string jsonData = JsonConvert.SerializeObject(_playerInfo);
        // JsonFileManager._instance.SaveJsonFile(_path,"PlayerStat",jsonData);
        _json.SaveJsonFile(_path, "PlayerStat", jsonData);
    }
    public void LoadPlayerStat()
    {
       // PlayerInfo player = JsonFileManager._instance.LoadJsonFile<PlayerInfo>(_path, "PlayerStat");
       PlayerInfo player = _json.LoadJsonFile<PlayerInfo>(_path, "PlayerStat");
        _playerInfo.SetPlayerInfo(player);
        SavePlayerStat();
    }
    public void SaveInventory(Item[] item , int gold)
    {
        InventoryDataSave iSave = new InventoryDataSave();
        for(int i=0; i<item.Length; i++)
        {
            iSave._inventoryArr.Add(i); 
            if (item[i] != null)
            {
                if (item[i] is CountableItem ci)
                {
                    if (ci is EtcItem etc)
                    {
                        iSave._itemType.Add((int)DefineEnum.eItemType.EtcItem);
                    }
                    else
                    {
                        iSave._itemType.Add((int)DefineEnum.eItemType.PotionItem);
                    }
                    iSave._itemID.Add(ci._data.ID);
                    iSave._itemAmount.Add(ci.Amount);
                }
                else
                {
                    iSave._itemType.Add(0); // 장비아이템 ... 아직 정해진게 없음
                    iSave._itemID.Add(item[i]._data.ID);
                    iSave._itemAmount.Add(0);
                }
            }
            else //아이템 정보가 없을때
            {
                iSave.nullInventoryData();
            }
        }
        iSave._gold = gold;
        string jsonData = JsonConvert.SerializeObject(iSave);
       _json.SaveJsonFile(_path, "InventoryData", jsonData);
    }
    public void LoadInventory()
    {
        InventoryDataSave iSave = _json.LoadJsonFile<InventoryDataSave>(_path, "InventoryData");
        Inventory inven = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        if(iSave != null)
        {
            for(int i=0; i < iSave._inventoryArr.Count; i++)
            {
                if (iSave._itemID[i] != 0)
                {
                    switch (iSave._itemType[i])
                    {
                        case (int)DefineEnum.eItemType.EtcItem:
                            inven.InitItem(i, _etcItems[iSave._itemID[i] - 1], iSave._itemAmount[i]);
                            break;
                        case (int)DefineEnum.eItemType.PotionItem:
                            inven.InitItem(i, _potionItems[iSave._itemID[i] - 1], iSave._itemAmount[i]);
                            break;
                    }
                }
            }
            inven.GoldAdd(iSave._gold);
        }
    }

    void SetMonsterData()
    {
        _monsterList = new Dictionary<int, MonsterData>();
        for(int i=0; i < _monsterData.Length; i++)
        {
           _monsterList.Add(_monsterData[i].Index, _monsterData[i]);
        }
    }
    void SetItemData()
    {
        _potionItemList = new Dictionary<int, PotionItemData>();
        for(int i=0;i < _potionItems.Length; i++)
        {
            _potionItemList.Add(_potionItems[i].ID, _potionItems[i]);
        }
    }
    public void LoadMap()
    {

    }

}
