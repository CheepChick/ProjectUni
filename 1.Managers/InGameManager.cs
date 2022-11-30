using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : TSingleton<InGameManager>
{
    
    GameObject _inventory;
    GameObject _mainUI;
    GameObject _dead;
    PlayerController _player;
    QuickSlotUI _quickSlotUI;

    public GameObject MainUI => _mainUI;
    public Inventory Inventory => _inventory.GetComponent<Inventory>();
    public PlayerController Player => _player;
    public QuickSlotUI QuickSlot => _quickSlotUI;

    bool _invectoryActivated = false;
    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
    }
    public void GetPlayer(PlayerController player) => _player = player;
    public void SetMainUI(GameObject mainUI) => _mainUI = mainUI;
    public void SetDead(GameObject dead) => _dead = dead;
    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory.gameObject;
        _inventory.SetActive(false);
        _invectoryActivated = false;
    }
    public void SetQuickSlot(QuickSlotUI quick)
    {
        _quickSlotUI = quick;
    }
    public void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _invectoryActivated = !_invectoryActivated;
            if (_invectoryActivated)
                _inventory.SetActive(true);
            else
                _inventory.SetActive(false);
        }
    }
}
