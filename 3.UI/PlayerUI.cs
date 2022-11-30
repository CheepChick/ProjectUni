using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider _hp;
    [SerializeField] TextMeshProUGUI _nowHP;
    [SerializeField] TextMeshProUGUI _maxHP;
    [SerializeField] Slider _exp;
    [SerializeField] TextMeshProUGUI _expValue;
    [SerializeField] TextMeshProUGUI _lvValue;
    [SerializeField] GameObject _dead;
    private void Awake()
    {
        InGameManager._instance.SetMainUI(gameObject);
    }
    public void SetPlayerUI(float nowHP, float maxHP, float nowExp,float maxExp , int level )
    {
        _nowHP.text = nowHP.ToString();
        _maxHP.text = maxHP.ToString();
        _hp.value = nowHP / maxHP;
        _exp.value = nowExp / maxExp;
        _expValue.text = $"{(_exp.value * 100):0.00}%";
        _lvValue.text = level.ToString();
    }
    public void SetHPRate(float nowHP, float maxHP)
    {
        _hp.value = nowHP/ maxHP;
        _maxHP.text =maxHP.ToString();
        _nowHP.text = nowHP.ToString();
    }
    public void SetExpRate(int level,float nowExp, float reExp)
    {
        _lvValue.text = level.ToString();
        _exp.value = nowExp/ reExp;
        _expValue.text = $"{(_exp.value *100):0.00}%";
    }
    public void PlayerDead()
    {
        _dead.SetActive(true);
    }



    public void DeadOK()
    {
        InGameManager._instance.Player.RespawnPlayer();
        _dead.SetActive(false);
    }
}
