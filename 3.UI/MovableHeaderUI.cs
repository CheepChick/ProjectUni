using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHeaderUI : MonoBehaviour ,IPointerDownHandler, IDragHandler
{
    [SerializeField] RectTransform _targetRt;

    Vector2 _beginPoint;
    Vector2 _moveBegin;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _beginPoint = _targetRt.anchoredPosition;
        _moveBegin = eventData.position;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _targetRt.anchoredPosition = _beginPoint + (eventData.position - _moveBegin);
    }
}
