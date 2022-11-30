using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] float[] _magnify;
    [SerializeField] float _hit = 1;

    Collider _collider;
    public GameObject[] _damageFont { get; set; }
    public StatBase _owner{ get; set; }

    public int[] _finalDamage
    {
        get; set;
    }
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _finalDamage = new int[(int)_hit];
        Invoke("EnabledCollider", 0.3f);
    }
    void EnabledCollider() => _collider.enabled = false;
    
    public void UserInitDataSet(PlayerController own,GameObject[] fonts )
    {
        _owner = own;
        for (int i = 0; i < _hit; i++)
        {
            _finalDamage[i] = (int)(own._finalDamage * _magnify[i]);
        }

        _damageFont = fonts;   
    }

    public void MonsterInitDataSet(MonsterObj own,GameObject[] font)
    {
        _owner = own;
        for (int i = 0; i < _hit; i++)
        {
            _finalDamage[i] = (int)(own._finalDamage * _magnify[i]);
        }
        _damageFont = font;
    }
}
