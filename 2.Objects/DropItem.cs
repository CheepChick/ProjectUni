using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{

    int _gold;
    ItemData _itemData;
    Vector3 _startPos;
    Vector3 _endPos;
    float _timer;

    private void Awake()
    {
        Destroy(gameObject, 60f);
        _startPos = transform.position;
        StartCoroutine("ItemMove");
    }

    // 포물선 그리기 (시작지점, 도착지점, 높이, 시간)
    Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    IEnumerator ItemMove()
    {
        _timer = 0;
        while (transform.position.y >= _startPos.y)
        {
            _timer += Time.deltaTime;
            Vector3 tempPos = Parabola(_startPos,_endPos, 1, _timer);
            transform.position = tempPos;
            yield return new WaitForEndOfFrame();
        }
        StopCoroutine(ItemMove());
    }

    public void SetGold(int gold) => _gold = gold;
    public void SetItem(ItemData itemData) => _itemData = itemData;
    public void SetEndPos(Vector3 dropPos)
    {
        float r =  UnityEngine.Random.Range(-0.5f, 0.5f);
        _endPos = new Vector3(dropPos.x - r, dropPos.y, dropPos.z - r);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InGameManager._instance.Inventory.GoldAdd(_gold);
            if (_itemData != null)
            {
                InGameManager._instance.Inventory.Add(_itemData);
            }
            Destroy(gameObject);
        }
    }
}
