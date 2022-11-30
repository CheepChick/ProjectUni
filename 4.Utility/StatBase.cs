using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatBase : MonoBehaviour
{
    protected int _level;
    protected string _name;
    protected bool _isDead;
    protected int _att;
    protected int _def;
    protected int _hp;
    protected float _critical;
    protected float _cDamage;
    

    public virtual int _maxHP
    {
        get { return _hp; }
    }
    public abstract int _finalDamage { get; }
    public abstract int _finalDeffence { get; }
    public abstract float _criticalRate { get; }
    public abstract float _criticalDemage { get; }

    //public abstract void HittingMe(int finishDam);
    //public abstract IEnumerator HittingMonster(int finishDam);
    public virtual bool SightOn(StatBase target)
    {
        return false;
    }
}
