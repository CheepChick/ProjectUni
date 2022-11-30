using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventoryUI _inventoryUI;
    [SerializeField] int _initalCapacity = 32;
    [SerializeField] int _maxCapacity = 64;
    [SerializeField] Item[] _items;
    private int _gold;
    public int Capacity { get; private set; }
    public Item[] Items { get { return _items; } }
    public int Gold { get { return _gold; } }
    public bool _invectoryActivated = false;

    private void Awake()
    {
        _items = new Item[_maxCapacity];
        Capacity = _initalCapacity;
        _gold = 0;
        _inventoryUI.SetInventoryReference(this);
    }
    private void Start()
    {
        DataBaseManager._instance.LoadInventory();
        InGameManager._instance.SetInventory(this);
        UpdateAccessibleStatesAll();
    }

    bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }  // �κ��丮 �����ִ��� Ȯ��
    int FindEmptySlotIndex(int startIndex = 0)
    {
        for(int i= startIndex; i<Capacity; i++)
        {
            if (_items[i] == null)
                return i;
            
        }
        return -1;
    } // ����ִ� ���� Ž��

    int FindCountableItemSlotIndex(CountableItemData target,int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;
            if (current._data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }
        return -1;
    } // �տ��� ���� ������ �ִ�  �Һ� ��Ÿ ������ Ž��
    void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;
       
        Item item = _items[index];
        if(item != null)
        {
            _inventoryUI.SetItemIcon(index, item._data.IconSprite);
            
            if(item is CountableItem ci)
            {
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                }
                else
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
            }
            else
            {
                _inventoryUI.HideItemAmountText(index);
            }

            _inventoryUI.UpdateSlotFilterState(index, item._data);
        }
        else
        {
            RemoveIcon();
        }
        void RemoveIcon()
        {
            _inventoryUI.RemoveItem(index);
            _inventoryUI.HideItemAmountText(index);
        }
    } // �ش��ϴ� ���� ����
    void UpdateSlot(params int[] indices)
    {
        foreach(var i in indices)
        {
            UpdateSlot(i);
        }
    } // �ش��ϴ� �ε����� ���Ե� ����
    void UpdateAllSlot()
    {
        for(int i=0; i< Capacity; i++)
        {
            UpdateSlot(i);
        }
    } // ��� ������ ���� 

    void SaveInventory()
    {
        DataBaseManager._instance.SaveInventory(_items , Gold);
    } // �κ��丮���� ����

    public void InitItem(int index,ItemData itemData,int amount=1)
    {
        if(itemData is CountableItemData ciData)
        {
            CountableItem ci = ciData.CreateItem() as CountableItem;
            ci.SetAmount(amount);
            _items[index] = ci;     
            
        }
        else
        {
            _items[index] = itemData.CreateItem();
        }
        UpdateSlot(index);
    } // �κ��丮 ���� �ʱ� ���� 
    public int Add (ItemData itemData, int amount = 1)
    {
        int index;

        if(itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;
            while(amount > 0)
            {
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    if(index == -1)
                    {
                        findNextCountable = false;
                    }
                    else
                    {
                        CountableItem ci = _items[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    if (index == -1) 
                    {
                        break; 
                    }
                    else
                    {
                        
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        _items[index] = ci;
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }

                }
            }
            QuickSlotUI quick = InGameManager._instance.QuickSlot;
            for(int i=0; i<quick.QuickSlotList.Count; i++)
            {
                SendQuick(quick.QuickSlotList[i], quick.QuickSlotList[i].DataIndex);
            }
        }
        else
        {
            if(amount == 1)
            {
                index = FindEmptySlotIndex();
                if(index == -1)
                {
                    _items[index] = itemData.CreateItem();
                    amount = 0;
                    UpdateSlot(index);
                }
            }

            index = -1;
            for(; amount>0; index--)
            {
                index = FindEmptySlotIndex(index + 1);
                if (index == -1) break;

                _items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }
        SaveInventory();
        return amount;
    } // �κ��丮�� ������ �߰� //�׿������� ����
   
    public void GoldAdd(int value)
    {
        _gold += value;
        _inventoryUI.UpdateGold(_gold);
        SaveInventory();
    } // �κ��丮�� ��� �߰�
    public void Swap(int indexA, int indexB)
    {
        if (!IsValidIndex(indexA)) return;
        if (!IsValidIndex(indexB)) return;

        Item itemA = _items[indexA];
        Item itemB = _items[indexB];
        if (itemA != null && itemB != null &&
            itemA._data == itemB._data &&
            itemA is CountableItem ciA && itemB is CountableItem ciB)
        {
            int maxAmount = ciB.MaxAmount;
            int sum = ciA.Amount + ciB.Amount;
            if (sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);
                ciB.SetAmount(maxAmount);
            }
        }
        else
        {
            _items[indexA] = itemB;
            _items[indexB] = itemA;
        }

        UpdateSlot(indexA, indexB);
        SaveInventory();
    } // �κ��丮 ���� ����
    public void UpdateAccessibleStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    
    public bool HasItem(int index)
    {
        return IsValidIndex(index) && _items[index] != null;
    }// ������ �ִ��� Ȯ��
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (_items[index] == null) return 0;

        CountableItem ci = _items[index] as CountableItem;
        if (ci == null) return 1;
        return ci.Amount;
    } // ������ ���� ��������

    public void SendQuick(QuickSlotData quick, int index)
    {
        Debug.Log(GetCurrentAmount(index));
        quick.SetItemAmount(GetCurrentAmount(index));
        quick.SetDataIndex(index);
    }

    public void Use(int index)
    {
        if (_items[index] == null) return;

        // ��� ������ �������� ���
        if (_items[index] is IUsableItem uItem)
        {
            // ������ ���
            bool succeeded = uItem.Use();

            
            if (succeeded)
            {
                if (_items[index]._data is PotionItemData potion)
                {
                    InGameManager._instance.Player.UsePotion(potion);
                }
                UpdateSlot(index);
            }
        }
        SaveInventory();
    }

}
