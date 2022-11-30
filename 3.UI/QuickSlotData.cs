using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlotData : MonoBehaviour
{
    [SerializeField] Image _slotImage;
    [SerializeField] TextMeshProUGUI _slotAmount;
    [SerializeField] TextMeshProUGUI _keyText;
    [SerializeField] int _slotIndex;
    [SerializeField] KeyCode _slotKey;
    public bool HasSlot => _slotImage.sprite != null;
    public int Index => _slotIndex;
    public KeyCode SlotKey => _slotKey;
    public int DataIndex => _dataIndex;

    public RectTransform ImageRect => _imageRect;
    public RectTransform SlotRect => _slotRect;

    GameObject _slotImageGO;
    GameObject _slotTextGO;
    RectTransform _imageRect;
    RectTransform _slotRect;

    int _dataIndex;
    private void Awake()
    {
        _dataIndex = -1;
        InitSet();
    }

    void InitSet()
    {
        _keyText.text = _slotKey.ToString().Replace("Alpha","");
        _slotImageGO = _slotImage.gameObject;
        _slotTextGO = _slotAmount.gameObject;
        HideImage();
        HideText();

    }
    void ShowImage() => _slotImageGO.SetActive(true);
    void HideImage() => _slotImageGO.SetActive(false);
    void ShowText() => _slotTextGO.SetActive(true);
    void HideText() => _slotTextGO.SetActive(false);

    public void SwapOrMove(QuickSlotData data)
    {
        if (data == null) return;
        if (data == this) return;

        var temp = _slotImage.sprite;

        if (data.HasSlot) SetSlot(data._slotImage.sprite);

        else RemoveSlot();

        data.SetSlot(temp);
    }

    public void SetSlot(Sprite sprite)
    {
        if (sprite != null)
        {
            _slotImage.sprite = sprite;
            ShowImage();
        }
        else
        {
            RemoveSlot();
        }
    }
    public void RemoveSlot()
    {
        _slotImage.sprite = null;
        HideImage();
        HideText();
    }
    public void SetItemAmount(int amount)
    {
        if (HasSlot && amount >= 1)
            ShowText();
        else
            HideText();

        _slotAmount.text = amount.ToString();
    }
    public void SetDataIndex(int index)
    {
        _dataIndex = index;
    }

}
