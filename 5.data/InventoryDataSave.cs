using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDataSave
{
    public List<int> _inventoryArr = new List<int>();
    public List<int> _itemID = new List<int>();
    public List<int> _itemAmount = new List<int>();
    public List<int> _itemType = new List<int>();
    public int _gold;
    public void nullInventoryData()
    {
        _itemID.Add(0);
        _itemAmount.Add(0);
        _itemType.Add(0);
    }
}
