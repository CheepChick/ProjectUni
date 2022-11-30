using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuickSlotUI : MonoBehaviour
{
    List<QuickSlotData> _quickSlotList;
    GraphicRaycaster _gr;

    PointerEventData _ped;
    List<RaycastResult> _rrList;

    QuickSlotData _beginQuickSlot;
    RectTransform _beginDragImageTransform;
    int _beginQuickSlotSiblingIndex;

    Vector3 _beginDragImagePoint;
    Vector3 _beginDragCursorPoint;
    public List<QuickSlotData> QuickSlotList =>  _quickSlotList;
    private void Awake()
    {
        InitSet();
    }

    private void Update()
    {
        _ped.position = Input.mousePosition;
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    void InitSet()
    {
        _quickSlotList = new List<QuickSlotData>();
        for(int i=0; i< transform.childCount; i++)
        {
            _quickSlotList.Add(transform.GetChild(i).GetComponent<QuickSlotData>());

        }
        InGameManager._instance.SetQuickSlot(this);

    }
    T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();


        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponent<T>();
    }
    void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beginQuickSlot = RaycastAndGetFirstComponent<QuickSlotData>(); // ������ Ȯ��

            if(_beginQuickSlot != null && _beginQuickSlot.DataIndex > -1) // �������� �ְ�, �����Ͱ� ���� ��,
            {
                //��ġ ���, ����
                _beginDragImageTransform = _beginQuickSlot.ImageRect;
                _beginDragImagePoint = _beginDragImageTransform.anchoredPosition;
                _beginDragCursorPoint = Input.mousePosition;

                //�� ���� �ø�
                _beginQuickSlotSiblingIndex = _beginQuickSlot.transform.GetSiblingIndex();
                _beginQuickSlot.transform.SetAsLastSibling();
            }
            else
            {
                _beginQuickSlot = null;
            }
        }
    }
    void OnPointerDrag()
    {
        if (_beginQuickSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            _beginDragImageTransform.anchoredPosition =
            _beginDragImagePoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }
    void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {

            if (_beginQuickSlot != null)
            {
                // ��ġ ����
                _beginDragImageTransform.anchoredPosition = _beginDragImagePoint;

                // UI ���� ����
                _beginQuickSlot.transform.SetSiblingIndex(_beginQuickSlotSiblingIndex);

                // �巡�� ��ó��
                EndDrag();

                _beginQuickSlot = null;
                _beginDragImageTransform = null;
            }
        }
    }

    void EndDrag()
    {
        QuickSlotData endDragQuickSlot = RaycastAndGetFirstComponent<QuickSlotData>();

        if (endDragQuickSlot != null)
        {
            TrySwapQuickSlot(_beginQuickSlot,endDragQuickSlot);
        }
    }

    void TrySwapQuickSlot(QuickSlotData quick1, QuickSlotData quick2)
    {
        
    }
}
