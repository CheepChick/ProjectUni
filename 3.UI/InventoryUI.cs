using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] int _horizontalSlotCount = 6;
    [SerializeField] int _verticalSlotCount = 6;
    [SerializeField] float _slotMargin = 8f;
    [SerializeField] float _contentAreaPadding = 20f;
    [SerializeField] float _slotSize = 64f;

    [Header("Connected Objects")]
    [SerializeField] RectTransform _contentAreaRT;
    [SerializeField] GameObject _slotUiPrefab;
    [SerializeField] TextMeshProUGUI _gold;

    Inventory _inventory;
    List<ItemSlotUI> _slotUIList;

    // 아이템 슬롯 옮기기 변수
    GraphicRaycaster _gr;
    
    PointerEventData _ped;
    List<RaycastResult> _rrList;


    ItemSlotUI _beginDragSlot;
    RectTransform _beginDragIconTransform; 

    Vector3 _beginDragIconPoint;   
    Vector3 _beginDragCursorPoint;
    int _beginDragSlotSiblingIndex;

    // 퀵슬롯 사용 변수
    GraphicRaycaster _quickGr;    
    List<RaycastResult> _quickList;

    //아이템 사용 변수
    ItemSlotUI _useSlot;
    float _clickTime = 0;
    private void Awake()
    {
        InitSlots();
        InitSet();
    }
    private void Update()
    {
        _ped.position = Input.mousePosition;
        OnDubleClick();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    void InitSet()
    {
        _gr = transform.parent.gameObject.GetComponent<GraphicRaycaster>();
        _quickGr = GameObject.FindGameObjectWithTag("MainUI").GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);
        _quickList = new List<RaycastResult>(10);
    }
    void InitSlots()
    {
        //아이템 슬롯 설정
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            _slotUiPrefab.AddComponent<ItemSlotUI>();

        _slotUiPrefab.SetActive(false);
        //아이템 슬롯 설정
        
        //아이템 슬롯 생성, 저장
        Vector2 beginPos = new Vector2(_contentAreaPadding, -_contentAreaPadding);
        Vector2 curPos = beginPos;
        
        _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);
        
        for (int i = 0; i < _verticalSlotCount; i++)
        {
            for (int j = 0; j < _horizontalSlotCount; j++)
            {
                int slotIndex = (_horizontalSlotCount * i) + j;
                GameObject slotGo = Instantiate(_slotUiPrefab,_contentAreaRT);
                RectTransform slotRT = slotGo.GetComponent<RectTransform>();

                slotRT.pivot = new Vector2(0f, 1f); // left Top
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot {slotIndex}";

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);
                _slotUIList.Add(slotUI);
                
                curPos.x += (_slotMargin + _slotSize);
            }
            curPos.x = beginPos.x;
            curPos.y -= (_slotMargin + _slotSize);
        }

        _gold.text = 0.ToString();
    }

    T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();
        _quickList.Clear();

        _gr.Raycast(_ped, _rrList);
        _quickGr.Raycast(_ped, _quickList);
        if (_rrList.Count == 0 && _quickList.Count ==0)
            return null;

        if (_rrList.Count >= 1)
        {
            return _rrList[0].gameObject.GetComponent<T>();
        }
        else
        {
            return _quickList[0].gameObject.GetComponent<T>();
        }
    }

    void OnDubleClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _useSlot = RaycastAndGetFirstComponent<ItemSlotUI>();
            if(_useSlot != null && _useSlot.HasItem)
            {
                if(Time.time - _clickTime < 0.3f)
                {
                    _inventory.Use(_useSlot.Index);
                    _clickTime = -1;
                }
                else
                {
                    _clickTime = Time.time;
                }
            }
            else
            {
                _useSlot = null;
            }
        }
    }
    void OnPointerDown()
    {
        // Left Click : Begin Drag
        if (Input.GetMouseButtonDown(0))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();
            // 아이템을 갖고 있는 슬롯만 해당
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                // 위치 기억, 참조 등록
                _beginDragIconTransform = _beginDragSlot.IconRect;
                _beginDragIconPoint = _beginDragIconTransform.anchoredPosition;
                _beginDragCursorPoint = Input.mousePosition;
                
                // 맨 위에 보이기
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
                //_beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                _beginDragSlot = null;
            }
        }
    }
    void OnPointerDrag()
    {
        if (_beginDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            // 위치 이동
            _beginDragIconTransform.anchoredPosition =
            _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }
    void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {

            if (_beginDragSlot != null)
            {
                // 위치 복원
                _beginDragIconTransform.anchoredPosition = _beginDragIconPoint;

                // UI 순서 복원
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // 드래그 끝처리
                EndDrag();

                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }

    void EndDrag()
    {
        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();
        QuickSlotData endDragQuickSlot = RaycastAndGetFirstComponent<QuickSlotData>();
        // 아이템 슬롯끼리 아이콘 교환 또는 이동
        if (endDragSlot != null && endDragSlot.IsAccessible)
        {

            TrySwapItems(_beginDragSlot, endDragSlot);
            QuickSlotUI quick = InGameManager._instance.QuickSlot;
            for(int i=0; i<quick.QuickSlotList.Count; i++)
            {
                if (quick.QuickSlotList[i].DataIndex == _beginDragSlot.Index)
                {
                    _inventory.SendQuick(quick.QuickSlotList[i], endDragSlot.Index);
                    if (_inventory.HasItem(endDragSlot.Index))
                    {
                        _inventory.SendQuick(quick.QuickSlotList[endDragSlot.Index], _beginDragSlot.Index);
                    }
                }
            }
           
            return;
        }

        if(endDragQuickSlot != null)
        {
            endDragQuickSlot.SetSlot(_beginDragSlot.IconImage);
            _inventory.SendQuick(endDragQuickSlot, _beginDragSlot.Index);
        }

    }
    void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        //같으면 아무것도 안함.
        if (from == to)  
            return;
        
        from.SwapOrMoveIcon(to);
        _inventory.Swap(from.Index, to.Index);
    }

    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }
    public void SetItemIcon(int index, Sprite icon) => _slotUIList[index].SetItem(icon);
    public void RemoveItem(int index)
    {
        _slotUIList[index].RemoveItem();
    }
    public void HideItemAmountText(int index)
    {
        _slotUIList[index].SetItemAmount(1);
    }
    public void SetItemAmountText(int index, int amount)
    {
        _slotUIList[index].SetItemAmount(amount);
    }
    public void UpdateSlotFilterState(int index, ItemData itemData)
    {
        bool isFiltered = true;

        // null인 슬롯은 타입 검사 없이 필터 활성화
        if (itemData != null)
            //switch (_currentFilterOption)
            //{
            //    case FilterOption.Equipment:
            //        isFiltered = (itemData is EquipmentItemData);
            //        break;

            //    case FilterOption.Portion:
            //        isFiltered = (itemData is EtcItemData);
            //        break;
            //}

        _slotUIList[index].SetItemAccessibleState(isFiltered);
    }
    public void UpdateGold(int value)
    {
        _gold.text = value.ToString();
    }

}
