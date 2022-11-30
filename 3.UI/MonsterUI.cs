using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Slider _hp;

    float _visibleTime = 5;
    Transform _target;
    private void Update()
    {
        _visibleTime -= Time.deltaTime;
        if (_visibleTime <= 0)
        {
            HpVisible(false);
            _visibleTime = 5;
        }
        if (_target != null)
        {
            //Vector3 pos = new Vector3(_target.position.x, transform.position.y, _target.position.z);
            //transform.LookAt(pos);
        }
    }
    void HpVisible(bool isVisi)
    {
        gameObject.SetActive(isVisi);
    }
    public void SetMonsterUI(string name)
    {
        _name.text = name;
        _hp.value = 1;
        HpVisible(false);
    }
    public void SetHPRate(float rate, Transform t)
    {
        _hp.value = rate;
        HpVisible(true);
        if (_target != t)
        {
            _target = t;
        }
        _visibleTime = 5;

    }
   
}
